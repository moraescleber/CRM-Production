using Blazored.LocalStorage;
using CRM.BLL.DTO;
using CRM.BLL.Interfaces;
using CRM.DAL.Entities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CRM.BLAZOR.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;
        private readonly ITempService _tempService;
        private readonly IUserRegistrationService _userRegistrationService;
        private readonly UserManager<User> _userManager;
        private readonly ILogService _logService;
        public AuthService(HttpClient httpClient,
                           AuthenticationStateProvider authenticationStateProvider,
                           ILocalStorageService localStorage, ITempService tempService, 
                           IUserRegistrationService userRegistrationService, UserManager<User> userManager,
                           ILogService logService)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
            _tempService = tempService;
            _userRegistrationService = userRegistrationService;
            _userManager = userManager;
            _logService = logService;
        }
        public async Task<LoginResult> Login(LoginModel loginModel)
        {
            var loginAsJson = JsonSerializer.Serialize(loginModel);
            var response = await _httpClient.PostAsync("Login", new StringContent(loginAsJson, Encoding.UTF8, "application/json"));
            var loginResult = JsonSerializer.Deserialize<LoginResult>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (!response.IsSuccessStatusCode)
            {
                return loginResult;
            }

            await _localStorage.SetItemAsync("authToken", loginResult.Token);
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginModel.Email);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.Token);

            _tempService.CurrentUser = await _userRegistrationService.GetCurrent(loginModel.Email);
            User user = await _userManager.FindByEmailAsync(loginModel.Email);
            LogDTO logDTO = new LogDTO
            {
                Action = "Успешно авторизовался",
                UserId = user.Id
            };
            await _logService.AddLog(logDTO);

            return loginResult;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
            LogDTO logDTO = new LogDTO
            {
                Action = "Вышел из системы",
                UserId = _tempService.CurrentUser.Id
            };
            await _logService.AddLog(logDTO);
            _tempService.CurrentUser = null;
        }
    }
}
