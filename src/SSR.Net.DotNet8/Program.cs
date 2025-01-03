using SSR.Net.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddReact17Renderer(config =>
    config.AddScriptFile(Path.Combine(builder.Environment.WebRootPath, "react17example.js"))
);

builder.Services.AddReact18Renderer(config =>
    config
        .AddScriptFile(Path.Combine(builder.Environment.WebRootPath, "React18TextEncoderPolyfill.js"))
        .AddScriptFile(Path.Combine(builder.Environment.WebRootPath, "react18example.js"))
);

builder.Services.AddReact19Renderer(config =>
    config
        .AddScriptFile(Path.Combine(builder.Environment.WebRootPath, "React19TextEncoderPolyfill.js"))
        .AddScriptFile(Path.Combine(builder.Environment.WebRootPath, "React19MessageChannelPolyfill.js"))
        .AddScriptFile(Path.Combine(builder.Environment.WebRootPath, "react19example.js"))
);

builder.Services.AddVue3Renderer(config =>
    config
        .AddScriptFile(Path.Combine(builder.Environment.WebRootPath, "vue3example.js"))
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
