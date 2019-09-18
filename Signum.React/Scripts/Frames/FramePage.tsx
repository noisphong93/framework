import * as React from 'react'
import { RouteComponentProps } from 'react-router'
import * as Navigator from '../Navigator'
import * as Constructor from '../Constructor'
import * as Finder from '../Finder'
import { ButtonBar, ButtonBarHandle } from './ButtonBar'
import { Entity, Lite, getToString, EntityPack, JavascriptMessage, entityInfo } from '../Signum.Entities'
import { TypeContext, StyleOptions, EntityFrame, ButtonBarElement } from '../TypeContext'
import { getTypeInfo, TypeInfo, PropertyRoute, ReadonlyBinding, GraphExplorer, parseId } from '../Reflection'
import { renderWidgets, renderEmbeddedWidgets, WidgetContext } from './Widgets'
import ValidationErrors from './ValidationErrors'
import * as QueryString from 'query-string'
import { ErrorBoundary } from '../Components';
import "./Frames.css"
import { AutoFocus } from '../Components/AutoFocus';
import { FunctionalAdapter } from './FrameModal';
import { useStateWithPromise, useForceUpdate } from '../Hooks'

interface FramePageProps extends RouteComponentProps<{ type: string; id?: string }> {

}

interface FramePageState {
  pack: EntityPack<Entity>;
  getComponent: (ctx: TypeContext<Entity>) => React.ReactElement<any>;
  refreshCount: number;
}

export default function FramePage(p: FramePageProps) {

  const [state, setState] = useStateWithPromise<FramePageState | undefined>(undefined);
  const buttonBar = React.useRef<ButtonBarHandle>(null);
  const entityComponent = React.useRef<React.Component | null>(null);
  const validationErrors = React.useRef<React.Component>(null);

  const forceUpdate = useForceUpdate();

  const ti = getTypeInfo(p.match.params.type);

  React.useEffect(() => {

    loadEntity()
      .then(pack => { Navigator.setTitle(pack.entity.toStr); return pack; })
      .then(pack => loadComponent(pack).then(getComponent => setState({ pack: pack, getComponent: getComponent, refreshCount: state ? state.refreshCount + 1 : 0 })))
      .done();

    return () => Navigator.setTitle();
  }, [p.match.params.type, p.match.params.id, p.location.search]);



  React.useEffect(() => {
    window.addEventListener("keydown", handleKeyDown);
  }, []);


  function handleKeyDown(e: KeyboardEvent) {
    if (!e.openedModals && buttonBar.current)
      buttonBar.current.handleKeyDown(e);
  }


  function loadComponent(pack: EntityPack<Entity>): Promise<(ctx: TypeContext<Entity>) => React.ReactElement<any>> {
    const viewName = QueryString.parse(p.location.search).viewName || undefined;
    return Navigator.getViewPromise(pack.entity, viewName && Array.isArray(viewName) ? viewName[0] : viewName).promise;
  }


  function loadEntity(): Promise<EntityPack<Entity>> {

    if (QueryString.parse(p.location.search).waitData) {
      if (window.opener.dataForChildWindow == undefined) {
        throw new Error("No dataForChildWindow in parent found!")
      }

      var pack = window.opener.dataForChildWindow;
      window.opener.dataForChildWindow = undefined;
      return Promise.resolve(pack);
    }

    if (p.match.params.id) {

      const lite: Lite<Entity> = {
        EntityType: ti.name,
        id: parseId(ti, p.match.params.id!),
      };

      return Navigator.API.fetchEntityPack(lite)
        .then(pack => {
          return Promise.resolve(pack);
        });

    } else {
      return Constructor.constructPack(ti.name)
        .then(pack => {
          return Promise.resolve(pack as EntityPack<Entity>);
        });
    }
  }

  function onClose() {
    if (Finder.isFindable(p.match.params.type, true))
      Navigator.history.push(Finder.findOptionsPath({ queryName: p.match.params.type }));
    else
      Navigator.history.push("~/");
  }

  function setComponent(c: React.Component | null) {
    if (c && entityComponent.current != c) {
      entityComponent.current = c;
      forceUpdate();
    }
  }



  if (!state) {
    return (
      <div className="normal-control">
        {renderTitle()}
      </div>
    );
  }

  const entity = state.pack.entity;


  const frame: EntityFrame = {
    frameComponent: { forceUpdate },
    entityComponent: entityComponent.current,
    pack: state.pack,
    onReload: (pack, reloadComponent, callback) => {

      var packEntity = (pack || state.pack) as EntityPack<Entity>;

      if (packEntity.entity.id != null && entity.id == null)
        Navigator.history.push(Navigator.navigateRoute(packEntity.entity));
      else {
        if (reloadComponent) {
          setState(undefined)
            .then(() => loadComponent(packEntity).then(gc => setState({ getComponent: gc, pack: packEntity, refreshCount: state.refreshCount + 1 })))
            .then(callback)
            .done();
        }
        else {
          setState({ pack: packEntity, getComponent: state.getComponent, refreshCount: state.refreshCount + 1 }).then(callback).done();
        }
      }
    },
    onClose: () => onClose(),
    revalidate: () => validationErrors.current && validationErrors.current.forceUpdate(),
    setError: (ms, initialPrefix) => {
      GraphExplorer.setModelState(entity, ms, initialPrefix || "");
      forceUpdate()
    },
    refreshCount: state.refreshCount,
    allowChangeEntity: true,
  };


  const styleOptions: StyleOptions = {
    readOnly: Navigator.isReadOnly(state.pack),
    frame: frame
  };

  const ctx = new TypeContext<Entity>(undefined, styleOptions, PropertyRoute.root(ti), new ReadonlyBinding(entity, "framePage"));

  const wc: WidgetContext<Entity> = {
    ctx: ctx,
    pack: state.pack,
  };

  const embeddedWidgets = renderEmbeddedWidgets(wc);

  return (
    <div className="normal-control">
      {renderTitle()}
      {renderWidgets(wc)}
      {entityComponent.current && <ButtonBar ref={buttonBar} frame={frame} pack={state.pack} />}
      {FunctionalAdapter.withRef(< ValidationErrors entity={state.pack.entity} prefix="framePage" />, validationErrors)}
        {embeddedWidgets.top}
      <div className="sf-main-control" data-test-ticks={new Date().valueOf()} data-main-entity={entityInfo(ctx.value)}>
        <ErrorBoundary>
          {state.getComponent && <AutoFocus>{FunctionalAdapter.withRef(state.getComponent(ctx), c => setComponent(c))}</AutoFocus>}
        </ErrorBoundary>
      </div>
      {embeddedWidgets.bottom}
    </div>
  );

  function renderTitle() {

    if (!state)
      return <h3 className="display-6 sf-entity-title">{JavascriptMessage.loading.niceToString()}</h3>;

    const entity = state.pack.entity;

    return (
      <h4>
        <span className="display-6 sf-entity-title">{getToString(entity)}</span>
        <br />
        <small className="sf-type-nice-name text-muted">{Navigator.getTypeTitle(entity, undefined)}</small>
      </h4>
    );
  }
}




