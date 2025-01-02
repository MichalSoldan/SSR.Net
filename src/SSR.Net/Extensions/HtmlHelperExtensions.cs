//using Microsoft.AspNetCore.Html;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;

//using System;
//using System.IO;
//using System.Linq;
//using System.Text;

//namespace SSR.Net.Extensions
//{
//    //
//    // Summary:
//    //     HTML Helpers for utilising React from an ASP.NET MVC application.
//    public static class HtmlHelperExtensions
//    {
//        [ThreadStatic]
//        private static StringWriter _sharedStringWriter;

//        //
//        // Summary:
//        //     Gets the React environment
//        private static IReactEnvironment Environment => ReactEnvironment.GetCurrentOrThrow;

//        //
//        // Summary:
//        //     Renders the specified React component
//        //
//        // Parameters:
//        //   htmlHelper:
//        //     HTML helper
//        //
//        //   componentName:
//        //     Name of the component
//        //
//        //   props:
//        //     Props to initialise the component with
//        //
//        //   htmlTag:
//        //     HTML tag to wrap the component in. Defaults to <div>
//        //
//        //   containerId:
//        //     ID to use for the container HTML tag. Defaults to an auto-generated ID
//        //
//        //   clientOnly:
//        //     Skip rendering server-side and only output client-side initialisation code. Defaults
//        //     to false
//        //
//        //   serverOnly:
//        //     Skip rendering React specific data-attributes, container and client-side initialisation
//        //     during server side rendering. Defaults to false
//        //
//        //   containerClass:
//        //     HTML class(es) to set on the container tag
//        //
//        //   exceptionHandler:
//        //     A custom exception handler that will be called if a component throws during a
//        //     render. Args: (Exception ex, string componentName, string containerId)
//        //
//        //   renderFunctions:
//        //     Functions to call during component render
//        //
//        // Type parameters:
//        //   T:
//        //     Type of the props
//        //
//        // Returns:
//        //     The component's HTML
//        public static IHtmlContent React<T>(this IHtmlHelper htmlHelper, string componentName, T props, string htmlTag = null, string containerId = null, bool clientOnly = false, bool serverOnly = false, string containerClass = null, Action<Exception, string, string> exceptionHandler = null, IRenderFunctions renderFunctions = null)
//        {
//            try
//            {
//                IReactComponent reactComponent = Environment.CreateComponent(componentName, props, containerId, clientOnly, serverOnly);
//                if (!string.IsNullOrEmpty(htmlTag))
//                {
//                    reactComponent.ContainerTag = htmlTag;
//                }

//                if (!string.IsNullOrEmpty(containerClass))
//                {
//                    reactComponent.ContainerClass = containerClass;
//                }

//                return RenderToString(delegate (StringWriter writer)
//                {
//                    reactComponent.RenderHtml(writer, clientOnly, serverOnly, exceptionHandler, renderFunctions);
//                });
//            }
//            finally
//            {
//                Environment.ReturnEngineToPool();
//            }
//        }

//        //
//        // Summary:
//        //     Renders the specified React component, along with its client-side initialisation
//        //     code. Normally you would use the React.AspNet.HtmlHelperExtensions.React``1(Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper,System.String,``0,System.String,System.String,System.Boolean,System.Boolean,System.String,System.Action{System.Exception,System.String,System.String},React.IRenderFunctions)
//        //     method, but React.AspNet.HtmlHelperExtensions.ReactWithInit``1(Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper,System.String,``0,System.String,System.String,System.Boolean,System.Boolean,System.String,System.Action{System.Exception,System.String,System.String},React.IRenderFunctions)
//        //     is useful when rendering self-contained partial views.
//        //
//        // Parameters:
//        //   htmlHelper:
//        //     HTML helper
//        //
//        //   componentName:
//        //     Name of the component
//        //
//        //   props:
//        //     Props to initialise the component with
//        //
//        //   htmlTag:
//        //     HTML tag to wrap the component in. Defaults to <div>
//        //
//        //   containerId:
//        //     ID to use for the container HTML tag. Defaults to an auto-generated ID
//        //
//        //   clientOnly:
//        //     Skip rendering server-side and only output client-side initialisation code. Defaults
//        //     to false
//        //
//        //   serverOnly:
//        //     Skip rendering React specific data-attributes, container and client-side initialisation
//        //     during server side rendering. Defaults to false
//        //
//        //   containerClass:
//        //     HTML class(es) to set on the container tag
//        //
//        //   exceptionHandler:
//        //     A custom exception handler that will be called if a component throws during a
//        //     render. Args: (Exception ex, string componentName, string containerId)
//        //
//        //   renderFunctions:
//        //     Functions to call during component render
//        //
//        // Type parameters:
//        //   T:
//        //     Type of the props
//        //
//        // Returns:
//        //     The component's HTML
//        public static IHtmlContent ReactWithInit<T>(this IHtmlHelper htmlHelper, string componentName, T props, string htmlTag = null, string containerId = null, bool clientOnly = false, bool serverOnly = false, string containerClass = null, Action<Exception, string, string> exceptionHandler = null, IRenderFunctions renderFunctions = null)
//        {
//            try
//            {
//                IReactComponent reactComponent = Environment.CreateComponent(componentName, props, containerId, clientOnly);
//                if (!string.IsNullOrEmpty(htmlTag))
//                {
//                    reactComponent.ContainerTag = htmlTag;
//                }

//                if (!string.IsNullOrEmpty(containerClass))
//                {
//                    reactComponent.ContainerClass = containerClass;
//                }

//                return RenderToString(delegate (StringWriter writer)
//                {
//                    reactComponent.RenderHtml(writer, clientOnly, serverOnly, exceptionHandler, renderFunctions);
//                    writer.WriteLine();
//                    WriteScriptTag(writer, delegate (TextWriter bodyWriter)
//                    {
//                        reactComponent.RenderJavaScript(bodyWriter, waitForDOMContentLoad: true);
//                    });
//                });
//            }
//            finally
//            {
//                Environment.ReturnEngineToPool();
//            }
//        }

//        //
//        // Summary:
//        //     Renders the JavaScript required to initialise all components client-side. This
//        //     will attach event handlers to the server-rendered HTML.
//        //
//        // Returns:
//        //     JavaScript for all components
//        public static IHtmlContent ReactInitJavaScript(this IHtmlHelper htmlHelper, bool clientOnly = false)
//        {
//            try
//            {
//                return RenderToString(delegate (StringWriter writer)
//                {
//                    WriteScriptTag(writer, delegate (TextWriter bodyWriter)
//                    {
//                        Environment.GetInitJavaScript(bodyWriter, clientOnly);
//                    });
//                });
//            }
//            finally
//            {
//                Environment.ReturnEngineToPool();
//            }
//        }

//        //
//        // Summary:
//        //     Returns script tags based on the webpack asset manifest
//        //
//        // Parameters:
//        //   htmlHelper:
//        //
//        //   urlHelper:
//        //     Optional IUrlHelper instance. Enables the use of tilde/relative (~/) paths inside
//        //     the expose-components.js file.
//        public static IHtmlContent ReactGetScriptPaths(this IHtmlHelper htmlHelper, IUrlHelper urlHelper = null)
//        {
//            string nonce = ((Environment.Configuration.ScriptNonceProvider != null) ? (" nonce=\"" + Environment.Configuration.ScriptNonceProvider() + "\"") : "");
//            return new HtmlString(string.Join("", from scriptPath in Environment.GetScriptPaths()
//                                                  select "<script" + nonce + " src=\"" + ((urlHelper == null) ? scriptPath : urlHelper.Content(scriptPath)) + "\"></script>"));
//        }

//        //
//        // Summary:
//        //     Returns style tags based on the webpack asset manifest
//        //
//        // Parameters:
//        //   htmlHelper:
//        //
//        //   urlHelper:
//        //     Optional IUrlHelper instance. Enables the use of tilde/relative (~/) paths inside
//        //     the expose-components.js file.
//        public static IHtmlContent ReactGetStylePaths(this IHtmlHelper htmlHelper, IUrlHelper urlHelper = null)
//        {
//            return new HtmlString(string.Join("", from stylePath in Environment.GetStylePaths()
//                                                  select "<link rel=\"stylesheet\" href=\"" + ((urlHelper == null) ? stylePath : urlHelper.Content(stylePath)) + "\" />"));
//        }

//        private static IHtmlContent RenderToString(Action<StringWriter> withWriter)
//        {
//            StringWriter stringWriter = _sharedStringWriter;
//            if (stringWriter == null)
//            {
//                stringWriter = (_sharedStringWriter = new StringWriter(new StringBuilder(128)));
//            }
//            else
//            {
//                stringWriter.GetStringBuilder().Clear();
//            }

//            withWriter(stringWriter);
//            return new HtmlString(stringWriter.ToString());
//        }

//        private static void WriteScriptTag(TextWriter writer, Action<TextWriter> bodyWriter)
//        {
//            writer.Write("<script");
//            if (Environment.Configuration.ScriptNonceProvider != null)
//            {
//                writer.Write(" nonce=\"");
//                writer.Write(Environment.Configuration.ScriptNonceProvider());
//                writer.Write("\"");
//            }

//            writer.Write(">");
//            bodyWriter(writer);
//            writer.Write("</script>");
//        }
//    }
//}