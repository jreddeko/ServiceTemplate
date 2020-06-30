using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceTemplate.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTemplate.Services
{
    public class ApiService : IApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ApiService> _logger;

        public ApiService(IHttpClientFactory httpClientFactory, ILogger<ApiService> logger)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets the API client from the service collection
        /// </summary>
        /// <returns></returns>
        public HttpClient GetHttpClient()
            => _httpClientFactory.CreateClient(ConfigurationConsts.MyHttpClient);

        /// <summary>
        /// Delete async
        /// </summary>
        /// <param name="path"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> DeleteAsync(string path, object body = null)
           => await SendAsync(path, HttpMethod.Delete, body);

        /// <summary>
        /// Put async
        /// </summary>
        /// <param name="path"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PutAsync(string path, object body = null)
            => await SendAsync(path, HttpMethod.Put, body);

        /// <summary>
        /// Post async
        /// </summary>
        /// <param name="path"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PostAsync(string path, object body = null)
            => await SendAsync(path, HttpMethod.Post, body);

        /// <summary>
        /// Get async
        /// </summary>
        /// <param name="path"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetAsync(string path, object body = null)
            => await SendAsync(path, HttpMethod.Get, body);

        /// <summary>
        /// Get async
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string path, object body = null)
        {
            var response = await SendAsync(path, HttpMethod.Get, body);
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        /// <summary>
        /// Post async
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(string path, object body = null)
        {
            var response = await SendAsync(path, HttpMethod.Post, body);
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        /// <summary>
        /// Send async
        /// </summary>
        /// <param name="path"></param>
        /// <param name="httpMethod"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> SendAsync(string path, HttpMethod httpMethod, object body = null)
        {
            // get http client
            using var client = _httpClientFactory.CreateClient(ConfigurationConsts.MyHttpClient);

            /// create request
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(client.BaseAddress, path),
                Method = httpMethod
            };

            // add request body
            if (body != null)
                request.Content = CreateByteArrayContent(body);

            // send request
            var response = await client.SendAsync(request);

            // sucuessful, return response
            if (response.IsSuccessStatusCode)
                return response;

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var result = await response.Content.ReadAsStringAsync();
                throw new Exception($"validationException: {result}");
            }

            // get content
            var stringContent = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException(stringContent);

            var exceptionMessage = string.IsNullOrEmpty(stringContent) ? response.ReasonPhrase : stringContent;
            throw new Exception($"{response.StatusCode}: {exceptionMessage}");
        }

        static ByteArrayContent CreateByteArrayContent(object resourceDto)
        {
            var myContent = JsonConvert.SerializeObject(resourceDto);
            var buffer = Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return byteContent;
        }
    }
}
