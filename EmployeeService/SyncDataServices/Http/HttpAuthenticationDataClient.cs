using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using EmployeeService.Dto;
using EmployeeService.SyncDataServices.Http;
using System.Net.Http.Headers;

namespace EmployeeService.SyncDataServices.Http
{
    public class HttpAuthenticationDataClient : IAuthenticationDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpAuthenticationDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }


        public async Task<string?> GetAuthFromAuthenticationService(object auth)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(auth),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync($"{_configuration["AuthService"]}/login", httpContent);

            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to AuthService was OK!");

                var result = await response.Content.ReadAsStringAsync();

                return result;
            }
            else
            {
                Console.WriteLine("--> Sync POST to AuthService was NOT OK!");
                return null;
            }
        }

        public async Task<object?> GetTestUserFromAuthenticationService()
        { 
            //  var httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.Timeout = TimeSpan.FromMilliseconds(3000);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + okraTokenSandbox);

            var response = await _httpClient.GetAsync($"{_configuration["AuthService"]}/test");

            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync Get to AuthService was OK!");

                var result = await response.Content.ReadFromJsonAsync<object>() ;

                return result;
            }
            else
            {
                Console.WriteLine("--> Sync GET to AuthService was NOT OK!");
                return null;
            }
        }
    }
}