using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Nancy;
using NbiMapApi.Server.Models;
using NbiMapApi.Server.Utils;
using Newtonsoft.Json;
using RestSharp;

namespace NbiMapApi.Server
{
    public sealed class HomeModule : NancyModule

    {
        public HomeModule()
        {
            Get("/", _ => View["index"]);

            Get("/geojson", _ =>
            {
                var client = new RestClient();
                var jsonData = LoadJson();

                var tmp = new GeoModel
                {
                    Type = "FeatureCollection",
                    Features = new List<Feature>()
                };

                foreach (var item in jsonData)
                {
                    var request =
                        new RestRequest(
                            $"https://geocode-maps.yandex.ru/1.x/?format=json&apikey=0972a63d-5f64-4dfb-9020-8669bb2ad1ae&geocode={item.Address}",
                            Method.GET, DataFormat.Json);
                    var response = client.Get<GeolocatorResponse>(request);
                    response.Data = GeolocatorResponse.FromJson(response.Content);
                    var posArray = response.Data.Response.GeoObjectCollection.FeatureMember.First().GeoObject.Point.Pos
                        .Split(' ');

                    var doublePosArray = new[]
                    {
                        double.Parse(posArray[1], NumberStyles.Any, CultureInfo.InvariantCulture),
                        double.Parse(posArray[0], NumberStyles.Any, CultureInfo.InvariantCulture)
                    };


                    tmp.Features.Add(new Feature
                    {
                        Type = FeatureType.Feature,
                        Geometry = new Geometry
                        {
                            Type = GeometryType.Point,
                            Coordinates = doublePosArray.ToList()
                        },
                        Properties = new Properties
                        {
                            BalloonContent = $"{item.Name}:{item.Address}",
                            HintContent = $"{item.Name}"
                        }
                    });
                }

                return tmp.ToJson();
            });
        }


        public static List<FileItem> LoadJson()
        {
            using (var r = new StreamReader("data.json"))
            {
                return JsonConvert.DeserializeObject<List<FileItem>>(r.ReadToEnd());
            }
        }
    }
}