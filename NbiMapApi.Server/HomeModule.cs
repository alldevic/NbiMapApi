using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Nancy;
using NbiMapApi.Server.Models;
using NbiMapApi.Server.Models.TSP;
using NbiMapApi.Server.Utils;
using Newtonsoft.Json;
using RestSharp;
using Point = NbiMapApi.Server.Models.TSP.Point;

namespace NbiMapApi.Server
{
    public sealed class HomeModule : NancyModule
    {
        private static RestClient _rest = new RestClient();

        private static List<double[]> _coords = new List<double[]>();

        public HomeModule()
        {
            Get("/", _ => View["index"]);

            Get("/geojson", _ =>
            {
                var jsonData = LoadJson();

                var geoModel = new GeoModel
                {
                    Type = "FeatureCollection",
                    Features = new List<Feature>()
                };

                foreach (var item in jsonData)
                {
                    var coords = GetCoords(item);
                    _coords.Add(coords.ToArray());
                    geoModel.Features.Add(new Feature
                    {
                        Type = FeatureType.Feature,
                        Geometry = new Geometry
                        {
                            Type = GeometryType.Point,
                            Coordinates = coords
                        },
                        Properties = new Properties
                        {
                            BalloonContent = $"{item.Name}:{item.Address}",
                            HintContent = $"{item.Name}"
                        }
                    });
                }

                return geoModel.ToJson();
            });

            Get("/geopath", _ =>
            {
                var points = new Point[_coords.Count];
                for (var i = 0; i < points.Length; i++)
                {
                    points[i] = new Point(_coords[i][0], _coords[i][1]);
                }

                var dest = new Tour(points.ToList());
                var p = Population.Randomized(dest, TspSettings.PopSize);

                var gen = 0;
                var better = true;

                var bestP = p;

                while (gen < 100)
                {
                    if (better)
                    {
                        bestP = p;
                    }

                    better = false;
                    var oldFit = p.MaxFit;

                    p = p.Evolve();
                    if (p.MaxFit > oldFit)
                    {
                        better = true;
                    }

                    gen++;
                }

                var tmp = bestP.Paths.Last().Points;
                var coords = new List<double[]>();

                for (int i = 0; i < tmp.Count; i++)
                {
                    coords.Add(new[]
                    {
                        tmp[i].X, tmp[i].Y
                    });
                }

                _coords = new List<double[]>();
                return JsonConvert.SerializeObject(coords);
            });
        }


        private static List<double> GetCoords(FileItem item)
        {
            var request =
                new RestRequest(
                    $"https://geocode-maps.yandex.ru/1.x/?format=json&apikey=0972a63d-5f64-4dfb-9020-8669bb2ad1ae&geocode={item.Address}",
                    Method.GET, DataFormat.Json);
            var response = _rest.Get<GeolocatorResponse>(request);
            response.Data = GeolocatorResponse.FromJson(response.Content);
            var posArray = response.Data.Response.GeoObjectCollection.FeatureMember.First().GeoObject.Point.Pos
                .Split(' ');

            return new[]
            {
                double.Parse(posArray[1], NumberStyles.Any, CultureInfo.InvariantCulture),
                double.Parse(posArray[0], NumberStyles.Any, CultureInfo.InvariantCulture)
            }.ToList();
        }

        private static List<FileItem> LoadJson()
        {
            using (var r = new StreamReader("data.json"))
            {
                return JsonConvert.DeserializeObject<List<FileItem>>(r.ReadToEnd());
            }
        }
    }
}