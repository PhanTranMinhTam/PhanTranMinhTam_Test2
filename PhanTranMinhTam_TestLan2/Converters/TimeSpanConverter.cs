using System.Text.Json;
using System.Text.Json.Serialization;

public class TimeSpanConverter : JsonConverter<TimeSpan>
{
    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            return TimeSpan.Parse(reader.GetString());
        }
        throw new JsonException("Invalid token type for TimeSpan.");
    }

    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("hh\\:mm\\:ss"));
    }
}
