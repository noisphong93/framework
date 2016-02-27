﻿import * as React from "react"
import { Router, Route, Redirect, IndexRoute } from "react-router"
import { Button, OverlayTrigger, Tooltip, MenuItem } from "react-bootstrap"
import { IEntity, Lite, Entity, ModifiableEntity, EmbeddedEntity, LiteMessage, EntityPack, toLite, JavascriptMessage,
    OperationSymbol, ConstructSymbol_From, ConstructSymbol_FromMany, ConstructSymbol_Simple, ExecuteSymbol, DeleteSymbol, OperationMessage, getToString, SearchMessage } from '../Signum.Entities';
import { PropertyRoute, PseudoType, EntityKind, TypeInfo, IType, Type, getTypeInfo, OperationInfo, OperationType, LambdaMemberType  } from '../Reflection';
import {classes} from '../Globals';
import * as Navigator from '../Navigator';
import Notify from '../Frames/Notify';
import { ContextualItemsContext, MenuItemBlock } from '../SearchControl/ContextualItems';
import { EntityFrame }  from '../Lines';
import { Dic }  from '../Globals';
import { ajaxPost, ValidationError }  from '../Services';
import { operationInfos, getSettings, ContextualOperationSettings, ContextualOperationContext, EntityOperationSettings, EntityOperationContext, 
    CreateGroup, API, autoStyleFunction, isEntityOperation } from '../Operations'


export function getConstructFromManyContextualItems(ctx: ContextualItemsContext): Promise<MenuItemBlock> {
    if (ctx.lites.length == 0)
        return null;

    const types = ctx.lites.groupByArray(lite => lite.EntityType);

    if (types.length != 1)
        return null;

    const ti = getTypeInfo(types[0].key);

    const menuItems = operationInfos(ti)
        .filter(oi => oi.operationType == OperationType.ConstructorFromMany)
        .map(oi => {
            const os = getSettings(oi.key) as ContextualOperationSettings<Entity>;
            const coc = {
                context: ctx,
                operationInfo: oi,
                settings: os,
            } as ContextualOperationContext<Entity>;

            if (os == null || os.isVisible == null || os.isVisible(coc))
                return coc;

            return null;
        })
        .filter(coc => coc != null)
        .orderBy(coc => coc.settings && coc.settings.order)
        .map((coc, i) => createContextualButton(coc, defaultConstructFromMany, i));


    if (!menuItems.length)
        return null;

    return Promise.resolve({
        header: SearchMessage.Create.niceToString(),
        menuItems: menuItems
    } as MenuItemBlock);
}



function defaultConstructFromMany(coc: ContextualOperationContext<Entity>, event: React.MouseEvent) {

    if (!confirmInNecessary(coc))
        return;

    API.constructFromMany(coc.context.lites, coc.operationInfo.key).then(pack => {

        if (!pack || !pack.entity)
            return;

        var es = Navigator.getSettings(pack.entity.Type);
        if (es.avoidPopup || event.ctrlKey || event.button == 1) {
            Navigator.currentHistory.pushState(pack, '/Create/');
            return;
        }
        else {
            Navigator.navigate({
                entity: pack
            });
        }
    }).done();
}


export function getEntityOperationsContextualItems(ctx: ContextualItemsContext): Promise<MenuItemBlock> {
    if (ctx.lites.length == 0)
        return null;

    const types = ctx.lites.groupByArray(coc => coc.EntityType);

    if (types.length != 1)
        return null;

    const ti = getTypeInfo(types[0].key);

    const contexts = operationInfos(ti)
        .filter(oi => isEntityOperation(oi.operationType))
        .map(oi => {
            const eos = getSettings(oi.key) as EntityOperationSettings<Entity>;
            const cos = eos == null ? null :
                ctx.lites.length == 1 ? eos.contextual : eos.contextualFromMany
            const coc = {
                context: ctx,
                operationInfo: oi,
                settings: cos,
                entityOperationSettings: eos,
            } as ContextualOperationContext<Entity>;

            var visibleByDefault = oi.lite && (ctx.lites.length == 1 || oi.operationType != OperationType.ConstructorFrom)

            if (cos == null ? visibleByDefault :
                cos.isVisible == null ? visibleByDefault && eos.isVisible == null && (eos.onClick == null || cos.onClick != null) :
                    cos.isVisible(coc))
                return coc;

            return null;
        })
        .filter(coc=>coc!= null)
        .orderBy(coc => coc.settings && coc.settings.order);
    
    if (!contexts.length)
        return null;

    var contextPromise: Promise<ContextualOperationContext<Entity>[]> = null;
    if (ctx.lites.length == 1 && contexts.some(coc => coc.operationInfo.hasCanExecute)) {
        contextPromise = Navigator.API.fetchEntityPack(ctx.lites[0]).then(ep => {
            contexts.forEach(coc => coc.canExecute = ep.canExecute[coc.operationInfo.key]);
            return contexts;
        });
    } else if (ctx.lites.length > 1 && contexts.some(coc => coc.operationInfo.hasStates)) {
        contextPromise = API.stateCanExecutes(ctx.lites, contexts.filter(coc => coc.operationInfo.hasStates).map(a => a.operationInfo.key))
            .then(response => {
                contexts.forEach(coc => coc.canExecute = response.canExecutes[coc.operationInfo.key]);
                return contexts;
            });
    } else {
        contextPromise = Promise.resolve(contexts);
    }


    return contextPromise.then(ctxs => {
        var menuItems = ctxs.filter(coc => coc.canExecute == null || !hideOnCanExecute(coc))
            .orderBy(coc => coc.settings && coc.settings.order != null ? coc.settings.order :
                coc.entityOperationSettings && coc.entityOperationSettings.order != null ? coc.entityOperationSettings.order : 0)
            .map((coc, i) => createContextualButton(coc, defaultEntityClick, i));

        if (menuItems.length == 0)
            return null;

        return {
            header: SearchMessage.Operations.niceToString(),
            menuItems: menuItems
        } as MenuItemBlock; 
    }); 
}


function hideOnCanExecute(coc: ContextualOperationContext<Entity>) {
    if (coc.settings && coc.settings.hideOnCanExecute != null)
        return coc.settings.hideOnCanExecute;

    if (coc.entityOperationSettings && coc.entityOperationSettings.hideOnCanExecute != null)
        return coc.entityOperationSettings.hideOnCanExecute;

    return false;
}



export function confirmInNecessary(coc: ContextualOperationContext<Entity>): boolean {

    var confirmMessage = getConfirmMessage(coc);

    return confirmMessage == null || confirm(confirmMessage);
}

function getConfirmMessage(coc: ContextualOperationContext<Entity>) {
    if (coc.settings && coc.settings.confirmMessage === null)
        return null;

    if (coc.settings && coc.settings.confirmMessage != null)
        return coc.settings.confirmMessage(coc);

    //eoc.settings.confirmMessage === undefined
    if (coc.operationInfo.operationType == OperationType.Delete)
        return coc.context.lites.length > 1 ?
            OperationMessage.PleaseConfirmYouDLikeToDeleteTheSelectedEntitiesFromTheSystem.niceToString() :
            OperationMessage.PleaseConfirmYouDLikeToDeleteTheEntityFromTheSystem.niceToString();

    return null;
}


function createContextualButton(coc: ContextualOperationContext<Entity>, defaultClick: (coc: ContextualOperationContext<Entity>, event: React.MouseEvent) => void, key: any) {

    var text = coc.settings && coc.settings.text ? coc.settings.text() :
        coc.entityOperationSettings && coc.entityOperationSettings.text ? coc.entityOperationSettings.text() :
            coc.operationInfo.niceName;

    var bsStyle = coc.settings && coc.settings.style || autoStyleFunction(coc.operationInfo);

    var disabled = !!coc.canExecute;

    var onClick = coc.settings && coc.settings.onClick ?
        (me: React.MouseEvent) => coc.settings.onClick(coc, me) :
        (me: React.MouseEvent) => defaultClick(coc, me)

    var menuItem = <MenuItem
        className={classes("btn-" + bsStyle, disabled ? "disabled" : null) }
        onClick={disabled ? null : onClick}
        data-operation={coc.operationInfo.key}
        key={key}>{text}</MenuItem>;

    if (!coc.canExecute)
        return menuItem;

    const tooltip = <Tooltip id={"tooltip_" + coc.operationInfo.key.replace(".", "_") }>{coc.canExecute}</Tooltip>;

    return <OverlayTrigger placement="right" overlay={tooltip} >{menuItem}</OverlayTrigger>;
}



function defaultEntityClick(coc: ContextualOperationContext<Entity>, event: React.MouseEvent) {

    if (!confirmInNecessary(coc))
        return;
    
    var promise: Promise<API.ErrorReport>;
    switch (coc.operationInfo.operationType) {
        case OperationType.ConstructorFrom: promise = API.constructFromMultiple(coc.context.lites, coc.operationInfo.key); break;
        case OperationType.Execute: promise = API.executeMultiple(coc.context.lites, coc.operationInfo.key); break;
        case OperationType.Delete: promise = API.deleteMultiple(coc.context.lites, coc.operationInfo.key); break;
    }

    promise
        .then(report => coc.context.markRows(report.errors))
        .done();
}




//function onClick(eoc: EntityOperationContext<Entity>): void {

//    if (eoc.settings && eoc.settings.onClick)
//        return eoc.settings.onClick(eoc);

//    if (eoc.operationInfo.lite) {
//        switch (eoc.operationInfo.operationType) {
//            case OperationType.ConstructorFrom: defaultConstructFromLite(eoc); return;
//            case OperationType.Execute: defaultExecuteLite(eoc); return;
//            case OperationType.Delete: defaultDeleteLite(eoc); return;
//        }
//    } else {
//        switch (eoc.operationInfo.operationType) {
//            case OperationType.ConstructorFrom: defaultConstructFromEntity(eoc); return;
//            case OperationType.Execute: defaultExecuteEntity(eoc); return;
//            case OperationType.Delete: defaultDeleteEntity(eoc); return;
//        }
//    }

//    throw new Error("Unexpected OperationType");
//}