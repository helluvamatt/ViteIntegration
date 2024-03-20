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

        // Required if using static assets not in the `public` directory. This sets the maximum size of images to inline as data URIs. 0 means no inlining.
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
