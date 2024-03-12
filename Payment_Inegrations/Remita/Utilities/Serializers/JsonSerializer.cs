using System.Text.Json;

namespace Remita.Utilities.Serializers
{
    public static class JsonSerializer
    {
        public static JsonSerializerOptions Options => new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new DateOnlyDateTimeConverter() }
        };

        public static string ToNoFormatJsonString(object data) => System.Text.Json.JsonSerializer.Serialize(data);
        public static string ToJsonString(object data) => System.Text.Json.JsonSerializer.Serialize(data, Options);
        public static T Deserialize<T>(string data) => System.Text.Json.JsonSerializer.Deserialize<T>(data, Options);
    }
}
