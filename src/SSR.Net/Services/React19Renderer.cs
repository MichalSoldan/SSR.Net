namespace SSR.Net.Services
{
    public class React19Renderer : ReactRendererBase
    {
        public React19Renderer(IJavaScriptEnginePool javaScriptEnginePool) : base(javaScriptEnginePool)
        {
        }

        protected override string CSRHydrateScript => "ReactDOMClient.hydrateRoot({0}, React.createElement({1},{2}))";//id, componentName, propsAsJson
        protected override string CSRRenderScript => "ReactDOMClient.createRoot({0}).render(React.createElement({1},{2}))";//id, componentName, propsAsJson
    }
}
