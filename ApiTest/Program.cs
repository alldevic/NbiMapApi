using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using RestSharp;

namespace ApiTest
{
    internal class Program
    {
        

        public static void Main(string[] args)
        {
            var client = new RestClient();
            var jsonData = LoadJson();
//            foreach (var item in jsonData)
//            {
            var item = jsonData.First();       
                Console.WriteLine($"'{item.Name}': {item.Address}");
                var request =
                    new RestRequest(
                        $"https://geocode-maps.yandex.ru/1.x/?format=json&apikey=0972a63d-5f64-4dfb-9020-8669bb2ad1ae&geocode={item.Address}",
                        Method.GET, DataFormat.Json);
                var response = client.Get(request);
                Console.WriteLine(response.Content);
//            }
        }

        public static List<Item> LoadJson()
        {
            using (var r = new StreamReader("data.json"))
            {
                return JsonConvert.DeserializeObject<List<Item>>(r.ReadToEnd());
            }
        }

        public class Item
        {
            public string Name { get; set; }
            public string Address { get; set; }
        }
    }
}