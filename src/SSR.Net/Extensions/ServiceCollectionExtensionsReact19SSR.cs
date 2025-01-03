using JavaScriptEngineSwitcher.V8;

using Microsoft.Extensions.DependencyInjection;

using SSR.Net.Services;

using System;

namespace SSR.Net.Extensions
{
    public static class ServiceCollectionExtensionsReact19SSR
    {
        public static void AddReact19Renderer(this IServiceCollection services, Func<JavaScriptEnginePoolConfig, JavaScriptEnginePoolConfig> config)
        {
            // configure Renderer
            var pool = new JavaScriptEnginePool(new V8JsEngineFactory(), config);

            // init Renderer
            var renderer = new React19Renderer(pool);

            // register Renderer as singleton
            services.AddSingleton<IReactRenderer>(renderer);
        }
    }
}
