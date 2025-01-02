using JavaScriptEngineSwitcher.V8;
using SSR.Net.Services;

namespace SSR.Net.DotNet8.Services
{
    public static class ServiceCollectionExtensionsReact19SSR
    {
        public class TextEncoder
        {
            public readonly string encoding = "utf-8";
            public byte[] encode(string text) => System.Text.Encoding.UTF8.GetBytes(text);

            public object encodeInto(string text, byte[] uint8Array)
            {
                uint8Array = encode(text);

                return new { read = text.Length, written = uint8Array.Length };
            }
        }

        public static void AddReact19Renderer(this IServiceCollection services, IWebHostEnvironment webHostEnvironment)
        {
            var pool = new JavaScriptEnginePool(new V8JsEngineFactory(), config =>
                config
                    //.AddHostType("TextEncoder", typeof(TextEncoder))
                    .AddScriptFile(Path.Combine(webHostEnvironment.WebRootPath, "React19TextEncoderPolyfill.js"))
                    .AddScriptFile(Path.Combine(webHostEnvironment.WebRootPath, "React19MessageChannelPolyfill.js"))
                    //.AddScriptFile(Path.Combine(webHostEnvironment.WebRootPath, "react19example.js"))
                    .AddScriptFile(Path.Combine(webHostEnvironment.WebRootPath, "main.net.js"))
            );
            services.AddSingleton(new React19Renderer(pool));
        }
    }
}
