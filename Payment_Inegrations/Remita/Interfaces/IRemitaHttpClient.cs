using Remita.Model.Response;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Remita.Interfaces
{
    public interface IRemitaHttpClient
    {
        Task<TRes> SendRequest<TReq, TRes>(HttpMethod method, string relativeUrl, string operationName, TReq requestBody = default, Func<object, string, string, string, Task> cleanUp = null) where TRes : ErrorResponse, new();
        Task<TRes> SendRequest<TRes>(HttpMethod method, string relativeUrl, string operationName, Func<object, string, string, string, Task> cleanUp = null) where TRes : ErrorResponse, new();
    }
}
