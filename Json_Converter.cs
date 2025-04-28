using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace ImgAnalyzer
{
    public class JsonInterfaceConverter<T> : JsonConverter<T>
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                var root = doc.RootElement;
                var typeDiscriminator = root.GetProperty("$type").GetString();
                Type actualType = Type.GetType(typeDiscriminator);

                var rawJson = root.GetRawText();
                return (T)JsonSerializer.Deserialize(rawJson, actualType, options);
            }
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("$type", value.GetType().AssemblyQualifiedName);

            foreach (var property in value.GetType().GetProperties())
            {
                writer.WritePropertyName(property.Name);
                JsonSerializer.Serialize(writer, property.GetValue(value), options);
            }

            writer.WriteEndObject();
        }
    }
}
