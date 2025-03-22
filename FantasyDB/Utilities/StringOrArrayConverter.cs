using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FantasyDB.Utilities
{
    public class StringOrArrayConverter : JsonConverter<List<string>>
    {
        public override List<string> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return new List<string> { reader.GetString() ?? string.Empty }; // ✅ Handle null values safely
            }
            else if (reader.TokenType == JsonTokenType.StartArray)
            {
                var list = new List<string>();
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndArray)
                    {
                        return list;
                    }
                    if (reader.TokenType == JsonTokenType.String)
                    {
                        list.Add(reader.GetString() ?? string.Empty);
                    }
                    else
                    {
                        throw new JsonException("Expected string inside array.");
                    }
                }
            }
            throw new JsonException("Unexpected JSON format for List<string> property.");
        }

        public override void Write(Utf8JsonWriter writer, List<string> value, JsonSerializerOptions options)
        {
            if (value.Count == 1)
            {
                writer.WriteStringValue(value[0]); // ✅ Store single value as string
            }
            else
            {
                writer.WriteStartArray();
                foreach (var item in value)
                {
                    writer.WriteStringValue(item);
                }
                writer.WriteEndArray();
            }
        }
    }
}
