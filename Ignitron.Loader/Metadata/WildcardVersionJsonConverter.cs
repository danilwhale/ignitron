using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ignitron.Loader.Metadata;

public sealed class WildcardVersionJsonConverter : JsonConverter<WildcardVersion>
{
    public override WildcardVersion Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return WildcardVersion.Parse(reader.GetString(), CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, WildcardVersion value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}