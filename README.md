# ViteIntegration

Integrate [Vite](https://vitejs.dev/) with [ASP.NET Core](https://dotnet.microsoft.com/en-us/apps/aspnet).

## Prerequisites

* .NET 8 or later is required.
* Only tested with MVC projects, but should work with Razor Pages as well.
* You'll need a [Vite](https://vitejs.dev) project setup. See [Recommended Vite Setup](#recommended-vite-setup) for more information. 

## Installation

1. Install the [ViteIntegration](https://www.nuget.org/packages/ViteIntegration/) NuGet package.
2. Add something like the following to your `Program.cs` file:

```csharp
//file: Program.cs

using ViteIntegration;

builder.Services.AddViteIntegration(options => {
    // Required: Add assets from your Vite manifest file. In your vite.config.ts file, you must set manifest: true.
    // For Vite v5+: The manifest file will be named `.vite/manifest.json` and will be placed in the `outDir`. The path should be defined relative to the application working directory.
    options.AddAssetsFromManifest("wwwroot/.vite/manifest.json");
    // For Vite v4: The manifest file will be named `manifest.json` and will be placed in the `outDir`. The path should be defined relative to the application working directory.
    //options.AddAssetsFromManifest("wwwroot/manifest.json");

    // Alternative: Add asset definitions manually, you can add as many as you want.
    options.AddAsset("src/site.ts", new ViteAsset { Src = "src/site.ts", File = "assets/site.js" });

    // Optional: Let ViteIntegration know the URL of your Vite dev server. This is only used in Development, and it only used when writing <script> tags.
    options.WithDevServer("http://localhost:5173");

    // Optional: Include these assets on every Razor page. This is useful for things like your main entry point, or a shared CSS file.
    options.WithDefaultAssets("src/site.ts");

    // Optional: Configure <script> tag rendering, value shown below are the defaults if you omit this call.
    options.WithScriptMode(ScriptMode.Module);

    // Optional: Configure where default assets are served from
    options.WithAssetsServedFrom(ServeFrom.Default);

    // Optional: Configure where tag helper-referenced assets are served from
    options.WithTagHelperAssetsServedFrom(ServeFrom.Default);
});
```

If you have a `Startup.cs` file, the above configuration can also add the following to your `ConfigureServices` method with changes since `builder.Services` will just be `services`.

You may also need to add the following to your `.csproj` file to ensure that the Vite manifest file is included in the publish output:

```xml
<ItemGroup>
    <Content Include="wwwroot/.vite/manifest.json">
        <CopyToPublishDirectory>PreserveNewest</CopyToPublishDirectory>
    </Content>
</ItemGroup>
```

## Recommended Vite Setup

The following is a minimal setup for a Vite project that will work with this library. See the comments for explanations.

```ts
//file: vite.config.ts

import { defineConfig } from "vite";

export default defineConfig({
    build: {

        // Not required, but assuming your Vite project is within your ASP.NET Core project, this will write Vite output to the default location for ASP.NET Core static files.
        // You could set this to anything, (or even leave it as the default 'dist') but you will need to configure the ASP.NET Core static files middleware to serve the files from this directory.
        outDir: "wwwroot",

        // Not required, but cleans out the output directory before each build.
        emptyOutDir: true,

        // Required if using AddAssetsFromManifest(), which reads the manifest file to determine which assets to include. You can also specify assets manually.
        manifest: true,

        // Required if using static assets not in the `public` directory with the vite-src and vite-href tag helpers. This sets the maximum size of images to inline as data URIs. 0 means no inlining.
        assetsInlineLimit: 0,

        rollupOptions: {
            input: {
                // Entry points for assets. The key is the name of the bundle, and the value is the source path.
                // When referring to entry points in your Razor views (either using the vite-src/vite-href tag helpers, or ViewContext.RequireViteAssets()), use the source path here to refer to the asset.
                "site": "assets/site.js"
            },
            output: {
                // Default settings for ViteIntegration are to write <script> tags with type="module" which requires ES modules.
                format: "esm"
            }
        }
    },

    // Optional: Relative path to the public directory. This is where Vite will look for static assets to copy to the output directory.
    publicDir: "assets/public",

    // Optional: ASP.NET Core MVC or Razor Pages are considered "mpa" (multi-page application) types.
    appType: "mpa"
});
```

## License

This project is licensed under the [MIT license](https://github.com/helluvamatt/ViteIntegration/blob/main/LICENSE). Full terms are available in the [license file](https://github.com/helluvamatt/ViteIntegration/blob/main/LICENSE).
