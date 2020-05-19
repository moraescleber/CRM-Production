using Microsoft.AspNetCore.Components;
using CRM.BLAZOR.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Threading;
using CRM.BLL.DTO;
using System.Net.Http;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using CRM.BLAZOR.Models;
using Newtonsoft.Json;
using System.Text;

namespace CRM.BLAZOR.Components
{
    public class MainBase : ComponentBase, IDisposable
    {
        [Inject]
        protected CRM.BLL.Interfaces.IUserRegistrationService UserRegistrationService { get; set; }
        [Inject]
        AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject]
        CRM.BLL.Interfaces.ITempService TempService { get; set; }
        [Inject]
        HttpClient Http { get; set; }

        protected Timer timer;
        protected CancellationTokenSource _cts = new CancellationTokenSource();
        protected AuthenticationState authState;
        protected ClaimsPrincipal user;
        protected int SelectedId;

        protected string login;
        protected string password;

        protected IEnumerable<CompanyDTO> NewCompanies;
        protected IEnumerable<CompanyDTO> QualifiedCompanies;
        protected IEnumerable<CompanyDTO> NotQualifiedCompanies;
        protected override async Task OnInitializedAsync()
        {
            authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            //authState = await AuthenticationStateProvider.
            user = authState.User;
            await TempService.UpdateCompanies();
            if (user.Identity.IsAuthenticated)
            {
                TempService.CurrentUser = await UserRegistrationService.GetCurrent(user.Identity.Name);
            }
            await StartCountdown();
        }

        async Task StartCountdown()
        {
            timer = new Timer(new TimerCallback(async _ =>
            {
                try
                {
                    authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    user = authState.User;
                    if (user.Identity.IsAuthenticated)
                    {
                        await RenderUpdate();
                    }
                }
                catch
                {
                    _cts.Cancel();
                    timer.Dispose();
                }
                await InvokeAsync(StateHasChanged);

            }), null, 5000, 1200);
        }
        public async Task RenderUpdate()
        {
            if (user.Identity.IsAuthenticated)
            {
                NewCompanies = TempService.NewCompanies;
                QualifiedCompanies = TempService.QualifiedCompanies;
                NotQualifiedCompanies = TempService.NotQualifiedCompanies;
            }

            SelectedId = TempService.GetSelectedId();
        }
        public async Task Login()
        {
            LoginModel loginModel = new LoginModel 
            { 
                Email=login,
                Password=password
            };
            HttpContent content = new StringContent(JsonConvert.SerializeObject(loginModel), Encoding.UTF8, "application/json");
            await Http.PostAsync("/Login",content);
        }
        public void Dispose()
        {
            if(timer!=null)
            {
                timer.Dispose();
            }
            _cts.Cancel();
        }
    }
}
