using System;
using NbiMapApi.Server.Models;
using Newtonsoft.Json;

namespace NbiMapApi.Server.Converters
{
    internal class PresetConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Preset) || t == typeof(Preset?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "islands#blueDotIcon":
                    return Preset.IslandsBlueDotIcon;
                case "islands#greenDotIcon":
                    return Preset.IslandsGreenDotIcon;
                case "islands#pinkDotIcon":
                    return Preset.IslandsPinkDotIcon;
                case "islands#pinkIcon":
                    return Preset.IslandsPinkIcon;
                case "islands#violetDotIcon":
                    return Preset.IslandsVioletDotIcon;
                case "islands#violetIcon":
                    return Preset.IslandsVioletIcon;
            }
            throw new Exception("Cannot unmarshal type Preset");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Preset)untypedValue;
            switch (value)
            {
                case Preset.IslandsBlueDotIcon:
                    serializer.Serialize(writer, "islands#blueDotIcon");
                    return;
                case Preset.IslandsGreenDotIcon:
                    serializer.Serialize(writer, "islands#greenDotIcon");
                    return;
                case Preset.IslandsPinkDotIcon:
                    serializer.Serialize(writer, "islands#pinkDotIcon");
                    return;
                case Preset.IslandsPinkIcon:
                    serializer.Serialize(writer, "islands#pinkIcon");
                    return;
                case Preset.IslandsVioletDotIcon:
                    serializer.Serialize(writer, "islands#violetDotIcon");
                    return;
                case Preset.IslandsVioletIcon:
                    serializer.Serialize(writer, "islands#violetIcon");
                    return;
            }
            throw new Exception("Cannot marshal type Preset");
        }

        public static readonly PresetConverter Singleton = new PresetConverter();
    }
}