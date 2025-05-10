using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TestAppMVVM.Models;

namespace TestAppMVVM.Services
{
    internal class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        private const string ApiUrl = "https://jsonplaceholder.typicode.com/users";

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<User>> GetUsersAsync()
        {
            var response = await _httpClient.GetStringAsync(ApiUrl);
            return JsonConvert.DeserializeObject<List<User>>(response);
        }
    }
}
