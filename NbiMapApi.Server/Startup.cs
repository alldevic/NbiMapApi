using System;

namespace NbiMapApi.Server
{
    public class Startup
    {
        public static IDisposable Start(string url)
        {

            return WebApp.Start(url, app => new Startup(rep).Configuration(app));
        }
    }
}