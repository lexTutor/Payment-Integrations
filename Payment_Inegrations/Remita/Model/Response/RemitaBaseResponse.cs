using Remita.Utilities.Serializers;
using System.Text.Json.Serialization;

namespace Remita.Model.Response
{
    public class RemitaBaseResponse<T> : ErrorResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public RemitaBaseResponse()
        {

        }

        public RemitaBaseResponse(string message, T data = default, string error = null)
        {
            Message = message;
            Data = data;
            Error = error ?? Error;
        }

        public static RemitaBaseResponse<T> Failed(string message, T data = default, string error = null) => new RemitaBaseResponse<T>(message, data: data, error: error);

        public static RemitaBaseResponse<T> Successful(string message, T data = default) => new RemitaBaseResponse<T>(message, data: data);

    }

    public class ErrorResponse
    {
        public string Error { get; set; }
        public bool IsSuccessful => string.IsNullOrWhiteSpace(Error);
    }

    public class ResponseData
    {
        public string Error { get; set; }

        [JsonPropertyName("error_description")]
        public string ErrorDescription { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }

    public class RemitaErrorResponse
    {
        public string ResponseData { get; set; }

        public int ResponseCode { get; set; }

        public string ResponseMsg { get; set; }

        public ResponseData ParsedResponseData => JsonSerializer.Deserialize<ResponseData>(ResponseData);
    }
}
