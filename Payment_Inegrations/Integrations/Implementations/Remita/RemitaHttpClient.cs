using Integrations.Exceptions;
using Integrations.Interfaces.Remita;
using Integrations.Model.Common;
using Integrations.Model.Api.Request;
using Integrations.Model.Api.Response;
using Integrations.Utilities;
using Integrations.Utilities.Serializers;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Integrations.Implementations.Remita
{
    internal class RemitaHttpClient : IRemitaHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly RemitaConfiguration _configuration;
        private readonly IDistributedCache _cache;
        private readonly ILogger<RemitaHttpClient> _logger;

        public RemitaHttpClient(
            IHttpClientFactory httpClientfactory,
            IOptions<RemitaConfiguration> configuration,
            IDistributedCache cache,
            ILogger<RemitaHttpClient> logger = null)
        {
            _httpClient = httpClientfactory.CreateClient(GenericConstants.RemitaHttpClientName);
            _configuration = configuration.Value;
            _cache = cache;
            _logger = logger;
        }

        /// <summary>
        /// Sends an HTTP request to the specified <paramref name="relativeUrl"/> with the given <paramref name="method"/>.
        /// </summary>
        /// <typeparam name="TReq">The type of the request body.</typeparam>
        /// <typeparam name="TRes">The type of the response body.</typeparam>
        /// <param name="method">The HTTP method to use for the request.</param>
        /// <param name="relativeUrl">The relative URL of the endpoint.</param>
        /// <param name="operationName">The name of the operation.</param>
        /// <param name="requestBody">The request body (optional).</param>
        /// <param name="cleanUp">An optional cleanup function. Parameters are in the following order: RawRequestObject, RawResponseObject, MethodName, RelativeUrl.</param>
        /// <returns>A task representing the asynchronous operation, returning the response object of type <typeparamref name="TRes"/>.</returns>
        /// <exception cref="AuthenticationFailedException">Thrown when authentication fails due to Invalid credentials or otherwise.</exception>
        /// <exception cref="InvalidServerResponseException">Thrown when the server returns incomprehensible data.</exception>
        public async Task<TRes> SendRequest<TReq, TRes>(HttpMethod method, string relativeUrl, string operationName,
            TReq requestBody = default, Func<object, string, string, string, Task> cleanUp = null)
            where TRes : ErrorResponse, new()
        {
            try
            {
                using (var request = await CreateRequest(method, relativeUrl, requestBody))
                {
                    using (var response = await _httpClient.SendAsync(request))
                    {
                        var (parsedResponse, rawResponse) = await HandleApiResponse<TRes>(response, operationName);

                        if (cleanUp != null)
                        {
                            await cleanUp(requestBody, rawResponse, method.Method, relativeUrl);
                        }

                        return parsedResponse;
                    }
                }
            }
            catch (AuthenticationFailedException) { throw; }
            catch (Exception ex)
            {
                _logger?.LogError(ex, ex.Message, nameof(SendRequest));

                throw new InvalidServerResponseException("Response from server is invalid please try again");
            }
        }

        /// <summary>
        /// Sends an HTTP request to the specified <paramref name="relativeUrl"/> with the given <paramref name="method"/>.
        /// </summary>
        /// <typeparam name="TReq">The type of the request body.</typeparam>
        /// <typeparam name="TRes">The type of the response body.</typeparam>
        /// <param name="method">The HTTP method to use for the request.</param>
        /// <param name="relativeUrl">The relative URL of the endpoint.</param>
        /// <param name="operationName">The name of the operation.</param>
        /// <param name="requestBody">The request body (optional).</param>
        /// <param name="cleanUp">An optional cleanup function. Parameters are in the following order: RawRequestObject, RawResponseObject, MethodName, RelativeUrl.</param>
        /// <returns>A task representing the asynchronous operation, returning the response object of type <typeparamref name="TRes"/>.</returns>
        /// <exception cref="AuthenticationFailedException">Thrown when authentication fails due to Invalid credentials or otherwise.</exception>
        /// <exception cref="InvalidServerResponseException">Thrown when the server returns incomprehensible data.</exception>
        public async Task<TRes> SendRequest<TRes>(HttpMethod method, string relativeUrl, string operationName, Func<object, string, string, string, Task> cleanUp = null)
            where TRes : ErrorResponse, new()
        {
            try
            {
                using (var request = await CreateRequest(method, relativeUrl))
                {
                    using (var response = await _httpClient.SendAsync(request))
                    {
                        var (parsedResponse, rawResponse) = await HandleApiResponse<TRes>(response, operationName);

                        if (cleanUp != null)
                        {
                            await cleanUp(null, rawResponse, method.Method, relativeUrl);
                        }

                        return parsedResponse;
                    }
                }

            }
            catch (AuthenticationFailedException) { throw; }
            catch (Exception ex)
            {
                _logger?.LogError(ex, ex.Message, nameof(SendRequest));

                throw new InvalidServerResponseException("Response from server is invalid please try again");
            }
        }

        private async Task<HttpRequestMessage> CreateRequest(HttpMethod method, string relativeUrl, object body = null)
        {
            var token = await Authenticate();
            var request = new HttpRequestMessage(method, relativeUrl);
            request.Headers.Add("Authorization", $"Bearer {token}");

            if (body != null)
            {
                var jsonString = JsonSerializer.ToJsonString(body);
                request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            }

            return request;
        }

        private async Task<(T, string)> HandleApiResponse<T>(HttpResponseMessage response, string operationName) where T : ErrorResponse, new()
        {
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent || typeof(T) == typeof(bool))
                {
                    return ((T)(object)true, "");
                }
                else
                {
                    return (string.IsNullOrWhiteSpace(responseContent) ? default : JsonSerializer.Deserialize<T>(responseContent), responseContent);
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden || response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await _cache.SetStringAsync(CacheConstants.AccessToken, string.Empty);
                return (default, responseContent);
            }
            else
            {
                var parsedData = string.IsNullOrWhiteSpace(responseContent) ? null : JsonSerializer.Deserialize<RemitaErrorResponse>(responseContent)?.ParsedResponseData;

                return (new T()
                {
                    Error = parsedData?.ErrorDescription ?? parsedData?.Message ??
                        $"Unable to complete {operationName} operation"
                }, responseContent);
            }
        }

        private async Task<string> Authenticate()
        {
            try
            {
                var token = await _cache.GetStringAsync(CacheConstants.AccessToken);

                if (!string.IsNullOrEmpty(token))
                    return token;

                var requestBody = new AuthenticationRequest
                {
                    Username = _configuration.UserName,
                    Password = _configuration.Password
                };

                var request = new HttpRequestMessage(HttpMethod.Post, EndpointConstants.AuthUrl)
                {
                    Content = new StringContent(JsonSerializer.ToJsonString(requestBody), Encoding.UTF8, "application/json")
                };

                using (var responseMessage = await _httpClient.SendAsync(request))
                {
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseStr = await responseMessage.Content.ReadAsStringAsync();
                        var response = JsonSerializer.Deserialize<PaymentBaseResponse<List<AuthenticationResponse>>>(responseStr);
                        if (response.Status == "00" && !response.Data.IsNullOrEmpty())
                        {
                            token = response.Data[0].AccessToken;
                            await _cache.SetStringAsync(CacheConstants.AccessToken, token, new DistributedCacheEntryOptions
                            {
                                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(response.Data[0].ExpiresIn)
                            });
                            return token;
                        }
                        else
                        {
                            throw new AuthenticationFailedException(response?.Message);
                        }
                    }
                    else
                    {
                        throw new AuthenticationFailedException("Unable to complete remita authentication");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, ex.Message, nameof(Authenticate));

                throw new AuthenticationFailedException("Unable to complete remita authentication");
            }
        }
    }
}
