using SSR.Net.Exceptions;
using SSR.Net.Models;

using System;

namespace SSR.Net.Services
{
    public class Vue3Renderer : IVueRenderer
    {
        private readonly IJavaScriptEnginePool _javaScriptEnginePool;

        public Vue3Renderer(IJavaScriptEnginePool javaScriptEnginePool) => _javaScriptEnginePool = javaScriptEnginePool;

        protected virtual string CSRHtml => "<div id=\"{0}\"></div>";//id
        protected virtual string SSRRenderScript => "renderToString(createSSRApp({0}, {1})).then(html => {2}= '<div id={3}>' + html + '</div>').catch(err => {2}= 'Error ' + err)";//componentName, propsAsJson, resultVariableName, containerId
        protected virtual string CSRHydrateScript => "createSSRApp({0}, {1}).mount({2})";//componentName, propsAsJson, id
        protected virtual string CSRRenderScript => "createApp({0}, {1}).mount({2})";//id, componentName, propsAsJson

        public virtual RenderedComponent RenderComponent<T>(string componentName, T props, int waitForEngineTimeoutMs = 50, bool fallbackToClientSideRender = true, int asyncTimeoutMs = 200) where T : class, new()
        {
            System.Text.Json.JsonSerializerOptions options = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
                AllowTrailingCommas = true,
                WriteIndented = false
            };

            var propsAsJson = System.Text.Json.JsonSerializer.Serialize(props, options);

            return RenderComponent(componentName, propsAsJson, waitForEngineTimeoutMs, fallbackToClientSideRender, asyncTimeoutMs);
        }

        public virtual RenderedComponent RenderComponent(string componentName, string propsAsJson, int waitForEngineTimeoutMs = 50, bool fallbackToClientSideRender = true, int asyncTimeoutMs = 200)
        {
            var result = new RenderedComponent();
            var id = CreateId();
            var variableId = CreateId();
            var script = string.Format(SSRRenderScript, componentName, propsAsJson, variableId, id);
            string html = null;

            try
            {
                html = _javaScriptEnginePool.EvaluateAsyncJs(script, variableId, asyncTimeoutMs, waitForEngineTimeoutMs);
            }

            catch (Exception ex)
            {
                if (!fallbackToClientSideRender)
                {
                    throw ex;
                }
                return FallbackToCSRWithException(componentName, propsAsJson, ex);
            }

            if (html is null)
            {
                return RenderComponentCSR(componentName, propsAsJson);
            }

            result.Html = html;
            result.InitScript = string.Format(CSRHydrateScript, componentName, propsAsJson, id);
            return result;
        }

        public virtual RenderedComponent RenderComponentCSR(string componentName, string propsAsJson)
        {
            var id = CreateId();
            return new RenderedComponent
            {
                Html = string.Format(CSRHtml, id),
                InitScript = string.Format(CSRRenderScript, componentName, propsAsJson, id)
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
            return "vue_" + Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
