using JavaScriptEngineSwitcher.V8;

using Microsoft.Extensions.DependencyInjection;

using SSR.Net.Services;

using System;

namespace SSR.Net.Extensions
{
    public static class ServiceCollectionExtensionsVue3SSR
    {
        public static void AddVue3Renderer(this IServiceCollection services, Func<JavaScriptEnginePoolConfig, JavaScriptEnginePoolConfig> config)
        {
            var pool = new JavaScriptEnginePool(new V8JsEngineFactory(), config);

            services.AddSingleton<IVue3Renderer>(new Vue3Renderer(pool));
        }
    }
}
