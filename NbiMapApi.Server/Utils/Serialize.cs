using NbiMapApi.Server.Converters;
using NbiMapApi.Server.Models;
using Newtonsoft.Json;

namespace NbiMapApi.Server.Utils
{
    public static class Serialize
    {
        public static string ToJson(this GeolocatorResponse self) =>
            JsonConvert.SerializeObject(self, Converter.Settings);
        
        public static string ToJson(this GeoModel self) =>
            JsonConvert.SerializeObject(self, Converter.Settings);
    }
}