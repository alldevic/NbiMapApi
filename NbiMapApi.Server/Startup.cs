using System;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.StaticFiles;
using Newtonsoft.Json;
using Owin;


namespace NbiMapApi.Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseFileServer(new FileServerOptions
                {
                    RequestPath = new PathString("/js"),
                    FileSystem = new PhysicalFileSystem("js")
                })
                .UseCors(CorsOptions.AllowAll)
                .UseNancy();
        }

        public static IDisposable Start(string url)
        {
            return WebApp.Start(url, app => new Startup().Configuration(app));
        }
    }
}