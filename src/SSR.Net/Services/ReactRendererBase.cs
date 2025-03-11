using SSR.Net.Exceptions;
using SSR.Net.Models;

using System;

namespace SSR.Net.Services
{
    public abstract class ReactRendererBase : IReactRenderer
    {
        private readonly IJavaScriptEnginePool _javaScriptEnginePool;

        public ReactRendererBase(IJavaScriptEnginePool javaScriptEnginePool) => _javaScriptEnginePool = javaScriptEnginePool;

        protected virtual string SSRHtml => "<{2} id=\"{0}\" class=\"{1}\">{3}</{2}>";//id, html
        protected virtual string CSRHtml => "<{2} id=\"{0}\" class=\"{1}\"></{2}>";//id

        protected virtual string SSRRenderScript => "ReactDOMServer.renderToString(React.createElement({0},{1}))";//componentName, propsAsJson
        protected virtual string CSRHydrateScript => "ReactDOM.hydrate(React.createElement({2},{1}), {0})";//id, componentName, propsAsJson
        protected virtual string CSRRenderScript => "ReactDOM.render(React.createElement({2},{1}), {0})";//id, componentName, propsAsJson

        public virtual RenderedComponent RenderComponent(string componentName, string propsAsJson, int waitForEngineTimeoutMs = 50, bool fallbackToClientSideRender = true, string cssClass = null, string id = null, string tagName = null)
        {
            var result = new RenderedComponent();
            id = string.IsNullOrWhiteSpace(id) ? CreateId() : id;
            tagName = string.IsNullOrWhiteSpace(tagName) ? "div" : tagName;

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

            result.Html = string.Format(SSRHtml, id, cssClass, tagName, html);
            result.InitScript = string.Format(CSRHydrateScript, id, componentName, propsAsJson);

            return result;
        }

        public virtual RenderedComponent RenderComponent<T>(string componentName, T props, int waitForEngineTimeoutMs = 50, bool fallbackToClientSideRender = true, string cssClass = null, string id = null, string tagName = null) where T : class, new()
        {
            System.Text.Json.JsonSerializerOptions options = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
                AllowTrailingCommas = true,
                WriteIndented = false
            };

            var propsAsJson = System.Text.Json.JsonSerializer.Serialize(props, options);

            return RenderComponent(componentName, propsAsJson, waitForEngineTimeoutMs, fallbackToClientSideRender, cssClass, id, tagName);
        }

        public virtual RenderedComponent RenderComponentCSR(string componentName, string propsAsJson, string cssClass = null, string id = null, string tagName = null)
        {
            tagName = string.IsNullOrWhiteSpace(tagName) ? "div" : tagName;
            id = string.IsNullOrWhiteSpace(id) ? CreateId() : id;

            return new RenderedComponent
            {
                Html = string.Format(CSRHtml, id, cssClass, tagName),
                InitScript = string.Format(CSRRenderScript, id, componentName, propsAsJson)
            };
        }

        public virtual RenderedComponent RenderComponentCSR<T>(string componentName, T props, string cssClass = null, string id = null, string tagName = null) where T : class, new()
        {
            System.Text.Json.JsonSerializerOptions options = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
                AllowTrailingCommas = true,
                WriteIndented = false
            };

            var propsAsJson = System.Text.Json.JsonSerializer.Serialize(props, options);

            return RenderComponentCSR(componentName, propsAsJson);
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
