namespace SSR.Net.Services
{
    public class React17Renderer : ReactRendererBase
    {
        public React17Renderer(IJavaScriptEnginePool javaScriptEnginePool) : base(javaScriptEnginePool)
        {
        }

        protected override string CSRHydrateScript => "ReactDOM.hydrate(React.createElement({1},{2}), {0})";//id, componentName, propsAsJson
        protected override string CSRRenderScript => "ReactDOM.render(React.createElement({1},{2}), {0})";//id, componentName, propsAsJson
    }
}
