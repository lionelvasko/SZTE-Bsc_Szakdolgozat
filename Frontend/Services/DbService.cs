using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Szakdoga.Models;
using Contracts;

namespace Szakdoga.Services
{
    public class DbService
    {
        private readonly HttpClient _httpClient;
        public DbService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<HttpResponseMessage> Register(RegisterRequest request)
        {
            var json = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:5223/api/register", json);
            return response;
        }
        public async Task<HttpResponseMessage> Login(LoginRequest request)
        {
            var json = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:5223/api/login", json);
            return response;
        }
        public async Task<List<Models.Device>> GetAllDeviceForUSer()
        {
            var response = await _httpClient.GetAsync("http://localhost:5223/api/MainDevice");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var devices = JsonSerializer.Deserialize<List<Models.Device>>(json);
                return devices;
            }
            return new List<Models.Device>();
        }
        public async Task<List<Models.Entity>> GetAllEntitesForUser()
        {
            var response = await _httpClient.GetAsync("http://localhost:5223/api/SubDevice");
            var json = await response.Content.ReadAsStringAsync();
            var entities = JsonSerializer.Deserialize(
                json,
                PolymorphicEntityJsonContext.Default.ListEntity
            );
            return entities;
        }

        public async Task<List<Entity>> GetAllEntitiesForDevice(string deviceId)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5223/api/SubDevice/{deviceId}");
            var json = await response.Content.ReadAsStringAsync();
            var entities = JsonSerializer.Deserialize(
                json,
                PolymorphicEntityJsonContext.Default.ListEntity
            );
            return entities;
        }
        public async Task<UserModel> GetUserInfos()
        {
            var response = await _httpClient.GetAsync("http://localhost:5223/api/User");
            Debug.WriteLine(await response.Content.ReadAsStringAsync());
            var json = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<UserModel>(json);
            return user;
        }
        public async Task<Guid> AddDevice(AddDeviceRequest request)
        {
            var json = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:5223/api/MainDevice", json);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Dictionary<string, Guid>>(jsonResponse);
            var id = result["id"];
            return id;
        }

        public async Task<HttpResponseMessage> AddEntity(Guid deviceId, AddEntityRequest request)
        {
            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };

            var requestData = JsonSerializer.Serialize(request, options);
            var json = new StringContent(requestData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"http://localhost:5223/api/SubDevice/{deviceId}", json);
            return response;
        }
        public async Task<HttpResponseMessage> DeleteDevice(string deviceId)
        {
            var response = await _httpClient.DeleteAsync($"http://localhost:5223/api/MainDevice/{deviceId}");
            return response;
        }
        public async Task<HttpResponseMessage> DeleteEntity(string entityId)
        {
            var response = await _httpClient.DeleteAsync($"http://localhost:5223/api/SubDevice/{entityId}");
            return response;
        }
        public async Task<HttpResponseMessage> UpdateUserPassword(UpdatePasswordRequest request)
        {
            var json = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("http://localhost:5223/api/User/Password", json);
            return response;
        }
        public async Task<HttpResponseMessage> UpdateUserName(UpdateNameRequest request)
        {
            var json = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("http://localhost:5223/api/User/Name", json);
            return response;
        }
    }
}
