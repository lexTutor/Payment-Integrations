using Integrations.Utilities.Serializers;
using System.Text.Json.Serialization;

namespace Integrations.Model.Common
{
    public class PaymentBaseResponse<T> : ErrorResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public PaymentBaseResponse()
        {

        }

        public PaymentBaseResponse(string message, T data = default, string error = null)
        {
            Message = message;
            Data = data;
            Error = error ?? Error;
        }

        public static PaymentBaseResponse<T> Failed(string message, T data = default, string error = null) => new PaymentBaseResponse<T>(message, data: data, error: error);

        public static PaymentBaseResponse<T> Successful(string message, T data = default) => new PaymentBaseResponse<T>(message, data: data);

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
