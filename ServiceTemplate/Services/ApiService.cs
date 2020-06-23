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

        public ApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public HttpClient GetHttpClient()
            => _httpClientFactory.CreateClient(ConfigurationConsts.MyHttpClient);
        public async Task<HttpResponseMessage> DeleteAsync(string path, object body = null)
           => await SendAsync(path, HttpMethod.Delete, body);
        public async Task<HttpResponseMessage> PutAsync(string path, object body = null)
            => await SendAsync(path, HttpMethod.Put, body);
        public async Task<HttpResponseMessage> PostAsync(string path, object body = null)
            => await SendAsync(path, HttpMethod.Post, body);
        public async Task<HttpResponseMessage> GetAsync(string path, object body = null)
            => await SendAsync(path, HttpMethod.Get, body);

        public async Task<T> GetAsync<T>(string path, object body = null)
        {
            var response = await SendAsync(path, HttpMethod.Get, body);
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseBody);
        }
        public async Task<T> PostAsync<T>(string path, object body = null)
        {
            var response = await SendAsync(path, HttpMethod.Post, body);
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        public async Task<HttpResponseMessage> SendAsync(string path, HttpMethod httpMethod, object body = null)
        {
            using var client = _httpClientFactory.CreateClient(ConfigurationConsts.MyHttpClient);
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(client.BaseAddress, path),
                Method = httpMethod
            };

            if (body != null)
                request.Content = CreateByteArrayContent(body);

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
                return response;

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var result = await response.Content.ReadAsStringAsync();
                throw new Exception($"validationException: {result}");
            }

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
