using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ApiTest
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var jsonData = LoadJson();
            foreach (var item in jsonData)
            {
                Console.WriteLine($"'{item.Name}': {item.Address}");
            }
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