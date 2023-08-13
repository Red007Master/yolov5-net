using Newtonsoft.Json;
using System;
using SixLabors.ImageSharp;

namespace CustomJsonConverter
{
    public class ColorConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Color);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                string colorName = (string)reader.Value;
                return ColorFromString(colorName);
            }

            throw new JsonSerializationException($"Unexpected token type: {reader.TokenType}");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is Color color)
            {
                writer.WriteValue(ColorToString(color));
            }
            else
            {
                throw new JsonSerializationException("Unexpected value type");
            }
        }

        private Color ColorFromString(string colorName)
        {
            return Color.Parse(colorName);
        }

        private string ColorToString(Color color)
        {
            return color.ToString();
        }
    }
}
