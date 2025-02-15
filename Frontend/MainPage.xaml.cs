using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Szakdoga
{
    public partial class MainPage : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string ApiBaseUrl = "http://localhost:5000/api/auth";
        public MainPage()
        {
            InitializeComponent();
        }
        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var loginData = new
            {
                Username = "user", // Replace with actual username input
                PasswordHash = "password" // Replace with actual password input
            };

            var content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync($"{ApiBaseUrl}/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadAsStringAsync();
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    // Save the token for future requests (e.g., Secure Storage, etc.)
                    // Navigate or update UI to reflect successful login
                }
                else
                {
                    // Handle login failure
                    // You can show an error message
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., network error)
                // You can show an error message
            }
        }
    }
}
