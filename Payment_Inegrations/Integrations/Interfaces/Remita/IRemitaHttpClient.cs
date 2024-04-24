using Integrations.Model.Common;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Integrations.Interfaces.Remita
{
    public interface IRemitaHttpClient
    {
        Task<TRes> SendRequest<TReq, TRes>(HttpMethod method, string relativeUrl, string operationName, TReq requestBody = default, string hashToken = null, Func<object, string, string, string, Task> cleanUp = null)
            where TRes : ErrorResponse, new();

        Task<TRes> SendRequest<TRes>(HttpMethod method, string relativeUrl, string operationName, string hashToken = null, Func<object, string, string, string, Task> cleanUp = null) where TRes : ErrorResponse, new();
    }
}
