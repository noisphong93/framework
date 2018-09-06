﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
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
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using Signum.Entities;
    
    #line 1 "..\..\Signum\Views\Exception.cshtml"
    using Signum.Entities.Basics;
    
    #line default
    #line hidden
    using Signum.Utilities;
    using Signum.Web;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Signum/Views/Exception.cshtml")]
    public partial class _Signum_Views_Exception_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {
        public _Signum_Views_Exception_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Signum\Views\Exception.cshtml"
 using (var e = Html.TypeContext<ExceptionEntity>())
{
    e.LabelColumns = new BsColumn(4);

            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"col-sm-6\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 7 "..\..\Signum\Views\Exception.cshtml"
       Write(Html.ValueLine(e, f => f.Environment));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 8 "..\..\Signum\Views\Exception.cshtml"
       Write(Html.ValueLine(e, f => f.CreationDate, vl => vl.UnitText = e.Value.CreationDate.ToUserInterface().ToAgoString()));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 9 "..\..\Signum\Views\Exception.cshtml"
       Write(Html.EntityLine(e, f => f.User));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 10 "..\..\Signum\Views\Exception.cshtml"
       Write(Html.ValueLine(e, f => f.Version));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 11 "..\..\Signum\Views\Exception.cshtml"
       Write(Html.ValueLine(e, f => f.ThreadId));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 12 "..\..\Signum\Views\Exception.cshtml"
       Write(Html.ValueLine(e, f => f.MachineName));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 13 "..\..\Signum\Views\Exception.cshtml"
       Write(Html.ValueLine(e, f => f.ApplicationName));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n        <div");

WriteLiteral(" class=\"col-sm-6\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 16 "..\..\Signum\Views\Exception.cshtml"
       Write(Html.ValueLine(e, f => f.ActionName));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 17 "..\..\Signum\Views\Exception.cshtml"
       Write(Html.ValueLine(e, f => f.ControllerName));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 18 "..\..\Signum\Views\Exception.cshtml"
       Write(Html.ValueLine(e, f => f.UserHostAddress));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 19 "..\..\Signum\Views\Exception.cshtml"
       Write(Html.ValueLine(e, f => f.UserHostName));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 20 "..\..\Signum\Views\Exception.cshtml"
       Write(Html.ValueLine(e, f => f.UserAgent, vl => vl.ValueLineType = ValueLineType.TextArea));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n    </div>\r\n");

            
            #line 23 "..\..\Signum\Views\Exception.cshtml"
    e.LabelColumns = new BsColumn(2);
    
            
            #line default
            #line hidden
            
            #line 24 "..\..\Signum\Views\Exception.cshtml"
Write(Html.ValueLine(e, f => f.RequestUrl));

            
            #line default
            #line hidden
            
            #line 24 "..\..\Signum\Views\Exception.cshtml"
                                         
    
            
            #line default
            #line hidden
            
            #line 25 "..\..\Signum\Views\Exception.cshtml"
Write(Html.ValueLine(e, f => f.UrlReferer));

            
            #line default
            #line hidden
            
            #line 25 "..\..\Signum\Views\Exception.cshtml"
                                         


            
            #line default
            #line hidden
WriteLiteral("    <h3");

WriteLiteral(" style=\"color: rgb(139,0,0)\"");

WriteLiteral(">");

            
            #line 27 "..\..\Signum\Views\Exception.cshtml"
                               Write(e.Value.ExceptionType);

            
            #line default
            #line hidden
WriteLiteral("</h3>\r\n");

WriteLiteral("    <pre><code>");

            
            #line 28 "..\..\Signum\Views\Exception.cshtml"
          Write(e.Value.ExceptionMessage);

            
            #line default
            #line hidden
WriteLiteral("</code></pre>\r\n");

            
            #line 29 "..\..\Signum\Views\Exception.cshtml"

    using (var tabs = Html.Tabs(e))
    {
        tabs.Tab(new Tab("stackTrace", "StackTrace", 
            
            #line default
            #line hidden
item => new System.Web.WebPages.HelperResult(__razor_template_writer => {

WriteLiteralTo(__razor_template_writer, "<pre><code>");

            
            #line 32 "..\..\Signum\Views\Exception.cshtml"
                                       WriteTo(__razor_template_writer, e.Value.StackTrace);

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "</code></pre>");

})
            
            #line 32 "..\..\Signum\Views\Exception.cshtml"
                                                                                                              ));

        if (e.Value.Data.HasText())
        {
            tabs.Tab(new Tab("data", "Data", 
            
            #line default
            #line hidden
item => new System.Web.WebPages.HelperResult(__razor_template_writer => {

WriteLiteralTo(__razor_template_writer, "<pre><code>");

            
            #line 36 "..\..\Signum\Views\Exception.cshtml"
                               WriteTo(__razor_template_writer, e.Value.Data);

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "</code></pre>");

})
            
            #line 36 "..\..\Signum\Views\Exception.cshtml"
                                                                                                ));
        }
        if (e.Value.QueryString.HasText())
        {
            tabs.Tab(new Tab("queryString", "QueryString", 
            
            #line default
            #line hidden
item => new System.Web.WebPages.HelperResult(__razor_template_writer => {

WriteLiteralTo(__razor_template_writer, "<pre><code>");

            
            #line 40 "..\..\Signum\Views\Exception.cshtml"
                                             WriteTo(__razor_template_writer, e.Value.QueryString);

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "</code></pre>");

})
            
            #line 40 "..\..\Signum\Views\Exception.cshtml"
                                                                                                                     ));
        }
        if (e.Value.Form.HasText())
        {
            tabs.Tab(new Tab("form", "Form", 
            
            #line default
            #line hidden
item => new System.Web.WebPages.HelperResult(__razor_template_writer => {

WriteLiteralTo(__razor_template_writer, "<pre><code>");

            
            #line 44 "..\..\Signum\Views\Exception.cshtml"
                               WriteTo(__razor_template_writer, e.Value.Form);

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "</code></pre>");

})
            
            #line 44 "..\..\Signum\Views\Exception.cshtml"
                                                                                                ));
        }
        if (e.Value.Session.HasText())
        {
            tabs.Tab(new Tab("session", "Session", 
            
            #line default
            #line hidden
item => new System.Web.WebPages.HelperResult(__razor_template_writer => {

WriteLiteralTo(__razor_template_writer, "<pre><code>");

            
            #line 48 "..\..\Signum\Views\Exception.cshtml"
                                     WriteTo(__razor_template_writer, e.Value.Session);

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "</code></pre>");

})
            
            #line 48 "..\..\Signum\Views\Exception.cshtml"
                                                                                                         ));
        }
    }
}
            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591