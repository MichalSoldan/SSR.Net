using SSR.Net.Models;

namespace SSR.Net.Services
{
    public interface IReactRenderer
    {
        RenderedComponent RenderComponent(string componentName, string propsAsJson, int waitForEngineTimeoutMs = 50, bool fallbackToClientSideRender = true, string cssClass = null, string id = null, string tagName = null);

        RenderedComponent RenderComponent<T>(string componentName, T props, int waitForEngineTimeoutMs = 50, bool fallbackToClientSideRender = true, string cssClass = null, string id = null, string tagName = null) where T : class, new();

        RenderedComponent RenderComponentCSR(string componentName, string propsAsJson, string cssClass = null, string id = null, string tagName = null);

        RenderedComponent RenderComponentCSR<T>(string componentName, T props, string cssClass = null, string id = null, string tagName = null) where T : class, new();
    }
}
