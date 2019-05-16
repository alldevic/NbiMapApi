using Nancy;

namespace NbiMapApi.Server
{
    public class HomeModule : NancyModule

    {
        public HomeModule()
        {
            Get("/", _ => "Hello world!");
        }
    }
}