using ViteIntegration;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Vite Integration
builder.Services.AddViteIntegration(options => {
    // Required: Add assets from your Vite manifest file. In your vite.config.ts file, you must set manifest: true.
    // The manifest file will be named `manifest.json` and will be placed in the `outDir`. The path should be defined relative to the application working directory.
    options.AddAssetsFromManifest("wwwroot/.vite/manifest.json");

    // Alternative: Add asset definitions manually, you can add as many as you want.
    options.AddAsset("assets/vite-logo.svg", new ViteAsset { Src = "assets/vite-logo.svg", File = "assets/site.js" });

    // Optional: Let ViteIntegration know the URL of your Vite dev server. This is only used in Development, and it only used when writing <script> tags.
    options.WithDevServer("http://localhost:5173");

    // Optional: Include these assets on every Razor page. This is useful for things like your main entry point, or a shared CSS file.
    options.WithDefaultAssets("assets/site.js");

    // Optional: Configure <script> tag attributes, values shown below are the defaults if you omit this call.
    options.WithScriptOptions(isModule: true, isDefer: true);
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStatusCodePagesWithReExecute("/error", "?code={0}");
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
