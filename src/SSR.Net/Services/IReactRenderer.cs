using SSR.Net.Models;

namespace SSR.Net.Services
{
    public interface IReactRenderer
    {
        RenderedComponent RenderComponent(string componentName, string propsAsJson, int waitForEngineTimeoutMs = 50, bool fallbackToClientSideRender = true);

        RenderedComponent RenderComponent<T>(string componentName, T props, int waitForEngineTimeoutMs = 50, bool fallbackToClientSideRender = true) where T : class, new();

        RenderedComponent RenderComponentCSR(string componentName, string propsAsJson);

        RenderedComponent RenderComponentCSR<T>(string componentName, T props) where T : class, new();
    }
}
