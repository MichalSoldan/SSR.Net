using SSR.Net.Models;

namespace SSR.Net.Services
{
    public interface IVueRenderer
    {
        RenderedComponent RenderComponent<T>(string componentName, T props, int waitForEngineTimeoutMs = 50, bool fallbackToClientSideRender = true, int asyncTimeoutMs = 200) where T : class, new();

        RenderedComponent RenderComponent(string componentName, string propsAsJson, int waitForEngineTimeoutMs = 50, bool fallbackToClientSideRender = true, int asyncTimeoutMs = 200);

        RenderedComponent RenderComponentCSR(string componentName, string propsAsJson);
    }
}
