using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;
using Szakdoga.Models;
using Szakdoga.Requests;

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
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:5223/api/register", content);
            return response;
        }
        public async Task<HttpResponseMessage> Login(LoginRequest request)
        {
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:5223/api/login", content);
            return response;
        }
        public async Task<HttpResponseMessage> GetAllDevicesForUSer()
        {
            var response = await _httpClient.GetAsync("http://localhost:5223/api/Devices");
            return response;
        }
        public async Task<List<Models.Entity>> GetAllEntitesForUser()
        {
            var response = await _httpClient.GetAsync("http://localhost:5223/api/Entity/UserEntities");
            var content = await response.Content.ReadAsStringAsync();

            var entities = JsonConvert.DeserializeObject<List<Szakdoga.Models.Entity>>(content,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto,
                        SerializationBinder = new CustomSerializationBinder()
                    });
            return entities;
        }

        public class CustomSerializationBinder : ISerializationBinder
        {
            public Type BindToType(string assemblyName, string typeName)
            {
                if (typeName.Contains("SomfyEntity"))
                {
                    return typeof(SomfyEntity);
                }
                else if (typeName.Contains("TuyaEntity"))
                {
                    return typeof(TuyaEntity);
                }
                throw new ArgumentException("Unknown type");
            }

            public void BindToName(Type serializedType, out string assemblyName, out string typeName)
            {
                assemblyName = null;
                typeName = serializedType.Name;
            }
        }

        public async Task<HttpResponseMessage> GetAllEntitiesForDevice(string deviceId)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5223/api/Entities/DeviceEntities/{deviceId}");
            return response;
        }
        public async Task<UserModel> GetUserInfos()
        {
            var response = await _httpClient.GetAsync("http://localhost:5223/api/User");
            var content = await response.Content.ReadAsStringAsync();
            var user = System.Text.Json.JsonSerializer.Deserialize<UserModel>(content);
            return user;
        }
        public async Task<HttpResponseMessage> AddDevice(AddDeviceRequest request)
        {
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:5223/api/Device", content);
            return response;
        }
        public async Task<HttpResponseMessage> AddEntity(AddEntityRequest request)
        {
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:5223/api/Entity", content);
            return response;
        }
        public async Task<HttpResponseMessage> DeleteDevice(string deviceId)
        {
            var response = await _httpClient.DeleteAsync($"http://localhost:5223/api/Device/{deviceId}");
            return response;
        }
        public async Task<HttpResponseMessage> DeleteEntity(string entityId)
        {
            var response = await _httpClient.DeleteAsync($"http://localhost:5223/api/Entity/{entityId}");
            return response;
        }
        public async Task<HttpResponseMessage> UpdateUserPassword(UpdatePasswordRequest request)
        {
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("http://localhost:5223/api/User/UpdatePassword", content);
            return response;
        }
        public async Task<HttpResponseMessage> UpdateUserName(UpdateNameRequest request)
        {
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("http://localhost:5223/api/User/UpdateName", content);
            return response;
        }
    }
}
