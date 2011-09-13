﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.235
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASP
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using Signum.Utilities;
    using Signum.Entities;
    using Signum.Web;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel.DataAnnotations;
    using System.Configuration;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.Caching;
    using System.Web.DynamicData;
    using System.Web.SessionState;
    using System.Web.Profile;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using System.Web.UI.HtmlControls;
    using System.Xml.Linq;
    using Signum.Entities.Reflection;
    using Signum.Web.Properties;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MvcRazorClassGenerator", "1.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Signum/Views/PopupControl.cshtml")]
    public class _Page_Signum_Views_PopupControl_cshtml : System.Web.Mvc.WebViewPage<TypeContext>
    {


        public _Page_Signum_Views_PopupControl_cshtml()
        {
        }
        protected System.Web.HttpApplication ApplicationInstance
        {
            get
            {
                return ((System.Web.HttpApplication)(Context.ApplicationInstance));
            }
        }
        public override void Execute()
        {



WriteLiteral("<div id=\"");


    Write(Model.Compose("panelPopup"));

WriteLiteral("\" data-title=\"");


                                              Write(Navigator.Manager.GetTypeTitle(Model.UntypedValue as ModifiableEntity));

WriteLiteral("\">\r\n    <h2><span class=\"sf-entity-title\">");


                                  Write(ViewBag.Title ?? Model.UntypedValue.TryToString());

WriteLiteral("</span></h2>\r\n    <div class=\"sf-button-bar\">\r\n");


         if (ViewData.ContainsKey(ViewDataKeys.OkVisible) && (bool)ViewData[ViewDataKeys.OkVisible])
        {  

WriteLiteral("            <button id=\"");


                   Write(Model.Compose("btnOk"));

WriteLiteral("\" class=\"sf-entity-button sf-ok-button\" ");


                                                                                   Write(ViewData[ViewDataKeys.OnOk] != null ? Html.Raw("onclick=\"" + ViewData[ViewDataKeys.OnOk] + "\"") : null);

WriteLiteral(">\r\n                OK</button>                \r\n");


        }


         if (ViewData.ContainsKey(ViewDataKeys.SaveVisible) && (bool)ViewData[ViewDataKeys.SaveVisible] && Navigator.Manager.ShowSave(Model.UntypedValue.GetType(), false))
        {  

WriteLiteral("            <button id=\"");


                   Write(Model.Compose("ebSave"));

WriteLiteral("\" class=\"sf-entity-button sf-save\" ");


                                                                               Write(ViewData[ViewDataKeys.OnSave] != null ? Html.Raw("onclick=\"" + ViewData[ViewDataKeys.OnSave] + "\"") : null);

WriteLiteral(">\r\n                ");


           Write(Resources.Save);

WriteLiteral("</button>                \r\n");


        }

WriteLiteral("        ");


   Write(ButtonBarEntityHelper.GetForEntity(new ToolBarButtonContext
        { 
           Buttons = (ViewButtons)ViewData[ViewDataKeys.ViewButtons],
           ControllerContext = this.ViewContext,
           PartialViewName = ViewData[ViewDataKeys.PartialViewName].ToString(),
           Prefix =  Model.ControlID
        },  (ModifiableEntity)Model.UntypedValue).ToString(Html));

WriteLiteral("\r\n    </div>\r\n    ");


Write(Html.ValidationSummaryAjax(Model));

WriteLiteral("\r\n    ");


Write(Html.PopupHeader());

WriteLiteral("\r\n    <div id=\"");


        Write(Model.Compose("divMainControl"));

WriteLiteral("\" class=\"sf-main-control\">\r\n");


           Html.RenderPartial(ViewData[ViewDataKeys.PartialViewName].ToString(), Model);

WriteLiteral("    </div>\r\n</div>\r\n");


        }
    }
}
