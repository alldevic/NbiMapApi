using Topshelf;

namespace NbiMapApi.Selfhost
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var url = "http://+:8080";

            var host = HostFactory.New(x =>
            {
                x.UseNLog();

                x.Service<MainService>(s =>
                {
                    s.ConstructUsing(settings => new MainService());
                    s.WhenStarted(service => service.Start(url));
                    s.WhenStopped(service => service.Stop());
                });
                x.StartAutomatically();
                x.SetServiceName("nbimapsvc");
                x.SetDisplayName("NbiMapApi Server");
                x.SetDescription("NbiMapApi Service for Hackathon Light 2019");
                x.RunAsNetworkService();
            });

            host.Run();
        }
    }
}