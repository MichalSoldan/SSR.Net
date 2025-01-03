using SSR.Net.Exceptions;
using SSR.Net.Models;

using System;

namespace SSR.Net.Services
{
    public abstract class ReactRendererBase : IReactRenderer
    {
        private readonly IJavaScriptEnginePool _javaScriptEnginePool;

        public ReactRendererBase(IJavaScriptEnginePool javaScriptEnginePool) => _javaScriptEnginePool = javaScriptEnginePool;

        protected virtual string SSRHtml => "<div id=\"{0}\">{1}</div>";//id, html
        protected virtual string CSRHtml => "<div id=\"{0}\"></div>";//id

        protected virtual string SSRRenderScript => "ReactDOMServer.renderToString(React.createElement({0},{1}))";//componentName, propsAsJson
        protected virtual string CSRHydrateScript => "ReactDOM.hydrate(React.createElement({2},{1}), {0})";//id, componentName, propsAsJson
        protected virtual string CSRRenderScript => "ReactDOM.render(React.createElement({2},{1}), {0})";//id, componentName, propsAsJson

        public virtual RenderedComponent RenderComponent<T>(string componentName, T props, int waitForEngineTimeoutMs = 50, bool fallbackToClientSideRender = true) where T : class, new() 
        {
            System.Text.Json.JsonSerializerOptions options = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase, 
                AllowTrailingCommas = true,
                WriteIndented = false
            };

            var propsAsJson = System.Text.Json.JsonSerializer.Serialize(props, options);

            return RenderComponent(componentName, propsAsJson, waitForEngineTimeoutMs, fallbackToClientSideRender);
        }

        public virtual RenderedComponent RenderComponent(string componentName, string propsAsJson, int waitForEngineTimeoutMs = 50, bool fallbackToClientSideRender = true)
        {
            var result = new RenderedComponent();
            var id = CreateId();
            var script = string.Format(SSRRenderScript, componentName, propsAsJson);
            string html = null;

            try
            {
                html = _javaScriptEnginePool.EvaluateJs(script, waitForEngineTimeoutMs);
            }
            catch (Exception ex)
            {
                if (!fallbackToClientSideRender)
                {
                    throw;
                }
                return FallbackToCSRWithException(componentName, propsAsJson, ex);
            }

            if (html is null)
            {
                return RenderComponentCSR(componentName, propsAsJson);
            }

            result.Html = string.Format(SSRHtml, id, html);
            result.InitScript = string.Format(CSRHydrateScript, id, componentName, propsAsJson);
            return result;
        }

        public virtual RenderedComponent RenderComponentCSR(string componentName, string propsAsJson)
        {
            var id = CreateId();
            return new RenderedComponent
            {
                Html = string.Format(CSRHtml, id),
                InitScript = string.Format(CSRRenderScript, id, componentName, propsAsJson)
            };
        }

        protected virtual RenderedComponent FallbackToCSRWithException(string componentName, string propsAsJson, Exception ex)
        {
            var result = RenderComponentCSR(componentName, propsAsJson);

            if (ex is AcquireJavaScriptEngineTimeoutException timeoutException)
            {
                result.TimeoutException = timeoutException;
            }
            else
            {
                result.RenderException = ex;
            }

            return result;
        }

        protected virtual string CreateId()
        {
            return "react_" + Guid.NewGuid().ToString().Replace("-", string.Empty);
        }
    }
}
