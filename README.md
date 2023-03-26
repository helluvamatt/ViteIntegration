# ViteIntegration

Integrate [Vite](https://vitejs.dev/) with [ASP.NET Core](https://dotnet.microsoft.com/en-us/apps/aspnet).

## Prerequisites

* .NET 6 or later is required (for now).
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
    // The manifest file will be named `manifest.json` and will be placed in the `outDir`. The path should be defined relative to the application working directory.
    options.AddAssetsFromManifest("wwwroot/manifest.json");

    // Alternative: Add asset definitions manually, you can add as many as you want.
    options.AddAsset("src/site.ts", new ViteAsset { Src = "src/site.ts", File = "assets/site.js" });

    // Optional: Let ViteIntegration know the URL of your Vite dev server. This is only used in Development, and it only used when writing <script> tags.
    options.WithDevServer("http://localhost:5173");

    // Optional: Include these assets on every Razor page. This is useful for things like your main entry point, or a shared CSS file.
    options.WithDefaultAssets("src/site.ts");

    // Optional: Configure <script> tag attributes, values shown below are the defaults if you omit this call.
    options.WithScriptOptions(isModule: true, isDefer: true);
});
```

If you have a `Startup.cs` file, the above configuration can also add the following to your `ConfigureServices` method with changes since `builder.Services` will just be `services`.

## Recommended Vite Setup

The following is a minimal setup for a Vite project that will work with this library. See the comments for explanations.

```ts
//file: vite.config.ts

import {defineConfig} from "vite";

export default defineConfig({
    build: {
        // Not required, but assuming your Vite project is within your ASP.NET Core project, this will write Vite output to the default location for ASP.NET Core static files.
        // You could set this to anything, (or even leave it as the default 'dist') but you will need to configure the ASP.NET Core static files middleware to serve the files from this directory.
        outDir: "wwwroot",
        // Not required, but cleans out the output directory before each build.
        emptyOutDir: true,
        // Required if using AddAssetsFromManifest(), which reads the manifest file to determine which assets to include. You can also specify assets manually.
        manifest: true,
        rollupOptions: {
            output: {
                // Default settings for ViteIntegration are to write <script> tags with type="module" which requires ES modules.
                format: "esm"
            }
        }
    }
});
```

## License

This project is licensed under the [MIT license](https://github.com/helluvamatt/ViteIntegration/blob/main/LICENSE). Full terms are available in the [license file](https://github.com/helluvamatt/ViteIntegration/blob/main/LICENSE).
