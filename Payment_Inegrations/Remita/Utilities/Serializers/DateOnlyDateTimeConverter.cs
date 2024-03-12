using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Remita.Utilities.Serializers
{
    public class DateOnlyDateTimeConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException();
            }

            string dateTimeStr = reader.GetString();
            if (DateTime.TryParseExact(dateTimeStr, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out DateTime dateTime))
            {
                return dateTime;
            }
            else if (DateTime.TryParseExact(dateTimeStr, "yyyy-MM-dd HH:mm:ss.fff", null, System.Globalization.DateTimeStyles.None, out DateTime dateTimeWithMilliSeconds))
            {
                return dateTimeWithMilliSeconds;
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
