using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;

namespace KantynaLaser.Web.HttpPipeline;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSpaPipeline(this IApplicationBuilder app, bool isDevelopment)
    {
        app.UseSpaStaticFiles();
        app.UseRouting();

        app.UseSpa(spa =>
        {
            spa.Options.SourcePath = "ClientApp";

            if(isDevelopment)
                spa.UseReactDevelopmentServer("start");
        });

        return app;
    }
}