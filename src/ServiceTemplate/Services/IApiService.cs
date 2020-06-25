using System.Net.Http;
using System.Threading.Tasks;

namespace ServiceTemplate.Services
{
    public interface IApiService
    {
        Task<HttpResponseMessage> DeleteAsync(string path, object body = null);
        Task<HttpResponseMessage> GetAsync(string path, object body = null);
        Task<T> GetAsync<T>(string path, object body = null);
        Task<T> PostAsync<T>(string path, object body = null);
        Task<HttpResponseMessage> PostAsync(string path, object body = null);
        Task<HttpResponseMessage> PutAsync(string path, object body = null);
        Task<HttpResponseMessage> SendAsync(string path, HttpMethod httpMethod, object body = null);
    }
}