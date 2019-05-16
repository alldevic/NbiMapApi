using System;
using NbiMapApi.Server;

namespace NbiMapApi.Selfhost
{
    public class MainService
    {
        private IDisposable _owin;

        public void Start(string url)
        {
            _owin = Startup.Start(url);
        }

        public void Stop()
        {
            _owin?.Dispose();
        }
    }
}