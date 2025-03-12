
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common.Helper;

public class DecimalPrecisionConverter : JsonConverter<decimal>
{
    private readonly int _precision;

    public DecimalPrecisionConverter(int precision = 3)
    {
        _precision = precision;
    }

    public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return decimal.Round(reader.GetDecimal(), _precision);
    }

    public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(decimal.Round(value, _precision));
    }
}
