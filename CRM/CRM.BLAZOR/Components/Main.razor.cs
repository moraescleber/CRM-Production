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
using static CRM.BLAZOR.Controllers.AccountController;
using System.Text.Json;
using Blazored.LocalStorage;
using CRM.BLAZOR.Services;
using CRM.DAL.Entities;
using System.ComponentModel.DataAnnotations;
using BlazorInputFile;
using System.IO;

namespace CRM.BLAZOR.Components
{
    public class MainBase : ComponentBase, IDisposable
    {
        #region INJECTS
        [Inject]
        protected CRM.BLL.Interfaces.IUserRegistrationService UserRegistrationService { get; set; }
        [Inject]
        AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject]
        protected CRM.BLL.Interfaces.ILogService LogService { get; set; }
        [Inject]
        protected CRM.BLL.Interfaces.ITempService TempService { get; set; }
        [Inject]
        IAuthService AuthService { get; set; }
        [Inject]
        NavigationManager NavigationManager { get; set; }
        [Inject]
        CRM.BLL.Interfaces.ICompanyService CompanyService { get; set; }
        [Inject]
        protected CRM.BLL.Interfaces.IMailFindService MailFindService { get; set; }
        [Inject]
        protected CRM.BLL.Interfaces.ICsvService CsvService { get; set; }
        [Inject]
        protected CRM.BLL.Interfaces.ILemlistIntegrationService LemlistIntegrationService { get; set; }
        [Inject]
        protected CRM.BLL.Interfaces.IHunterIntegrationService HunterIntegrationService { get; set; }
        #endregion
        #region VARIABLES
        /// companies div BEGIN
        protected Timer timer;
        protected CancellationTokenSource _cts = new CancellationTokenSource();
        protected AuthenticationState authState;
        protected ClaimsPrincipal user;
        public CompanyModel SelectedCompany;
        protected int SelectedId;

        protected string login;
        protected string password;
        protected string LeftStyle { get; set; } = @"'title-new-companies' 'content' 'content' 'content' 'content' 'content' 'content' 'content' 'content' 'content'
'title-qualified'
'title-not-qualified'";
        protected string NewCompanyDisplay { get; set; } = "block";
        protected string QualifiedDisplay { get; set; } = "none";
        protected string NotQualifiedDisplay { get; set; } = "none";
        protected string ExceptionLabelDisplay { get; set; } = "none";
        protected string UploadStatus { get; set; }
        protected IEnumerable<CompanyDTO> NewCompanies;
        protected IEnumerable<CompanyDTO> QualifiedCompanies;
        protected IEnumerable<CompanyDTO> NotQualifiedCompanies;
        /// companies div END
        /// company-information div BEGIN
        /// company-information div END
        /// controls BEGIN
        public bool IsDisabled { get; set; }
        public string SendLemmlistModalDisplay = "none";
        public string AddContactModalDisplay = "none";
        public string ImportContactsModalDisplay = "none";
        public string AddLemlistStatisticModalDisplay = "none";
        public string MessageModalDisplay = "none";
        public string MessageForModal = "";
        public string ActionMessage = "Добавил новую компанию";
        public string ExceptionLabel = "";
        protected IEnumerable<ContactDTO> contacts;
        protected IEnumerable<CountryDTO> countries;
        protected List<int> checkedContacts;
        protected IEnumerable<Linkedin> Linkedins;
        protected IEnumerable<ContactDTO> SendForContacts;
        public AddLemlistStatistic AddLemlistStatistic;
        /// controls END
        #endregion
        #region BASE_METHODS
        protected override async Task OnInitializedAsync()
        {
            NewCompany = new CompanyRegistrationDTO();
            AddLemlistStatistic = new AddLemlistStatistic();
            checkedContacts = new List<int>();
            await TempService.UpdateCompanies();
            await RenderUpdate();
            authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            user = authState.User;
            if (user.Identity.IsAuthenticated)
            {
                TempService.CurrentUser = await UserRegistrationService.GetCurrent(user.Identity.Name);
            }
            await StartCountdown();
        }
        public void Dispose()
        {
            if (timer != null)
            {
                timer.Dispose();
            }
            _cts.Cancel();
        }
        #endregion
        #region OTHER_METHODS
        /// companies div BEGIN

        /// logs div BEGIN
        public IEnumerable<LogDTO> logs;
        public GetUserDTO currentUser;
        /// logs div END
        /// prospect-finder div BEGIN

        public CompanyRegistrationDTO NewCompany;

        /// prospect-finder div END
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

            }), null, 3000, 500);
        }
        public async Task RenderUpdate()
        {
            NewCompanies = TempService.NewCompanies;
            QualifiedCompanies = TempService.QualifiedCompanies;
            NotQualifiedCompanies = TempService.NotQualifiedCompanies;
            countries = TempService.Countries;
            SelectedId = TempService.GetSelectedId();
            if (SelectedId != 0)
            {
                SelectedCompany = TempService.CompanyModels.Where(p => p.Id == SelectedId).FirstOrDefault();
                await InvokeAsync(StateHasChanged);
                contacts = await TempService.GetCompanyContacts(SelectedId);
            }
            else
            {
                SelectedCompany = null;
            }
            if (TempService.CurrentUser != null)
            {
                currentUser = TempService.CurrentUser;
                var logTemp = TempService.logs;
                if (logTemp != null)
                {
                    logs = TempService.logs.Where(p => p.UserId == currentUser.Id);
                }
            }
            SelectedCompany = TempService.CompanyModels.Where(p => p.Id == SelectedId).FirstOrDefault();
            Linkedins = TempService.Linkedins;
        }
        public async Task Login()
        {
            LoginModel loginModel = new LoginModel
            {
                Email = login,
                Password = password
            };
            var result = await AuthService.Login(loginModel);

            if (result.Successful)
            {
                NavigationManager.NavigateTo("/");
            }
        }
        public async Task Logout()
        {
            await AuthService.Logout();
            NavigationManager.NavigateTo("/");
        }
        public async Task SelectCompanyElement(int id)
        {
            TempService.SetId(id);
            SelectedId = id;
            SelectedCompany = TempService.CompanyModels.Where(p => p.Id == SelectedId).FirstOrDefault();
            await AddLog(id);
        }
        
        public async Task AddLog(int CompanyId = 0, string WebSite = null, string LinkedinOfTradingName = null,
            QualifyCompanyModel qualifyCompany = null, int count = 0, string LinkedinOfUser = null, string ActionMesage = null)
        {
            if (WebSite != null)
            {
                LogDTO logDTO = new LogDTO
                {
                    Action = "Перешел на сайт " + WebSite,
                    UserId = TempService.CurrentUser.Id
                };
                await LogService.AddLog(logDTO);
                //Thread.Sleep(3000);
            }
            else if (WebSite == null && CompanyId != 0)
            {
                LogDTO logDTO = new LogDTO
                {
                    Action = "Просмотрел компанию " + TempService.AllCompanies.Where(p => p.Id == SelectedId).FirstOrDefault().TradingName,
                    UserId = TempService.CurrentUser.Id
                };
                await LogService.AddLog(logDTO);
            }
            else if (LinkedinOfTradingName != null)
            {
                LogDTO logDTO = new LogDTO
                {
                    Action = "Просмотрел аккаунт LinkedIn компании  " + LinkedinOfTradingName,
                    UserId = TempService.CurrentUser.Id
                };
                await LogService.AddLog(logDTO);
            }
            else if (qualifyCompany != null)
            {
                LogDTO logDTO;
                if (qualifyCompany.IsQualify)
                {
                    logDTO = new LogDTO
                    {
                        Action = "Изменил статус компании " + qualifyCompany.CompanyTradingName + " на Квалифицированный",
                        UserId = TempService.CurrentUser.Id
                    };
                }
                else
                {
                    logDTO = new LogDTO
                    {
                        Action = "Изменил статус компании " + qualifyCompany.CompanyTradingName + " на НЕ квалифицированный",
                        UserId = TempService.CurrentUser.Id
                    };
                }
                await LogService.AddLog(logDTO);
            }
            else if (count != 0)
            {
                LogDTO logDTO = new LogDTO
                {
                    Action = "Добавил " + count + " контактов в lemmlist",
                    UserId = TempService.CurrentUser.Id
                };
                await LogService.AddLog(logDTO);
            }
            else if (LinkedinOfUser != null)
            {
                LogDTO logDTO = new LogDTO
                {
                    Action = "Просмотрел аккаунт LinkedIn пользователя  " + LinkedinOfUser,
                    UserId = TempService.CurrentUser.Id
                };
                await LogService.AddLog(logDTO);
            }
            else if (ActionMesage != null)
            {
                LogDTO logDTO = new LogDTO
                {
                    Action = ActionMesage,
                    UserId = TempService.CurrentUser.Id
                };
                await LogService.AddLog(logDTO);
            }
            await TempService.UpdateLogs();
            await RenderUpdate();

        }
        public async Task ImportCSV()
        {
            //var companies = await CsvService.ImportCSV("Paymob.csv", Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"));
            //await CsvService.CreateCompanies(companies);
        }
        /// companies div END
        /// controls div BEGIN
        public async void SetQualify()
        {
            if (SelectedId != 0)
            {
                await CompanyService.SetQualified(SelectedId);
                var company = await CompanyService.GetCompany(SelectedId);
                QualifyCompanyModel qualified = new QualifyCompanyModel { IsQualify = true, CompanyTradingName = company.TradingName };
                await AddLog(qualifyCompany: qualified);
            }

            await TempService.UpdateCompanies();
            TempService.SetId(0);
            Pause();

        }
        public async void SetNotQualify()
        {
            if (SelectedId != 0)
            {
                await CompanyService.SetNotQualified(SelectedId);
                var company = await CompanyService.GetCompany(SelectedId);
                QualifyCompanyModel notQualified = new QualifyCompanyModel { IsQualify = false, CompanyTradingName = company.TradingName };
                await AddLog(qualifyCompany: notQualified);
            }
            await TempService.UpdateCompanies();
            TempService.SetId(0);
            Pause();
        }
        async void Pause()
        {
            IsDisabled = true;
            await InvokeAsync(StateHasChanged);
            Thread.Sleep(1200);
            IsDisabled = false;
            await InvokeAsync(StateHasChanged);
        }
        /// controls div END
        public async Task AddCompany()
        {
            if (NewCompany != null)
            {
                try
                {
                    Validator.ValidateObject(NewCompany, new ValidationContext(NewCompany));
                    await CompanyService.CreateCompany(NewCompany);
                    await AddLog(ActionMesage: ActionMessage + ": " + NewCompany.TradingName);
                    await TempService.UpdateCompanies();
                    await Close();
                }
                catch (Exception ex)
                {
                    ExceptionLabel = ex.Message;
                    ExceptionLabelDisplay = "block";
                }
                await InvokeAsync(StateHasChanged);
            }
        }
        /// prospect-finder div BEGIN
        public async Task Check(int value)
        {
            if (checkedContacts.Contains(value))
            {
                checkedContacts.Remove(value);
            }
            else
            {
                checkedContacts.Add(value);
            }
            await InvokeAsync(StateHasChanged);
        }
        public async Task Send()
        {
            SendForContacts = await MailFindService.FindContactsForId(checkedContacts.ToArray());
            SendLemmlistModalDisplay = "block";
            await InvokeAsync(StateHasChanged);
        }
        public async Task SendLemlist()
        {
            var results = (await LemlistIntegrationService.AddLeadsInCampaign(SendForContacts.ToList())).ToList();
            int successResults = results.Where(p => p.Result == true).Count();
            int failResults = results.Where(p => p.Result == false).Count();
            AddLemlistStatistic = new AddLemlistStatistic
            {
                successCount = successResults,
                failedCount = failResults
            };
            await AddLog(count: successResults);
            await Close();
            await OpenModalForAddLemlistStatistic();
        }
        public async Task FindHunter()
        {
            
            if (SelectedCompany!=null&&SelectedCompany.Website!=null)
            {
                List<Contact> FoundContacts = (await HunterIntegrationService.FindDomainContacts(SelectedCompany.Website)).ToList();
                MessageForModal = "Найдены "+ FoundContacts.Count+" новых контактов";
                MessageModalDisplay = "block";
                await TempService.UpdateCompanies();
                
                await InvokeAsync(StateHasChanged);
            }
            
        }
        public async Task OpenModalForNewCompany()
        {
            AddContactModalDisplay = "block";
            await InvokeAsync(StateHasChanged);
        }
        public async Task OpenModalForImportFile()
        {
            ImportContactsModalDisplay = "block";
            await InvokeAsync(StateHasChanged);
        }
        public async Task OpenModalForAddLemlistStatistic()
        {
            AddLemlistStatisticModalDisplay = "block";
            await InvokeAsync(StateHasChanged);
        }

        public async Task Close()
        {
            AddContactModalDisplay = "none";
            SendLemmlistModalDisplay = "none";
            ImportContactsModalDisplay = "none";
            ExceptionLabelDisplay = "none";
            AddLemlistStatisticModalDisplay = "none";
            MessageModalDisplay = "none";
            await InvokeAsync(StateHasChanged);
        }
        public async Task HandleSelection(IFileListEntry[] files)
        {
            string EndDirectory = "wwwroot/files/";
            var file = files.FirstOrDefault();
            if (file != null)
            {
                // Just load into .NET memory to show it can be done
                // Alternatively it could be saved to disk, or parsed in memory, or similar
                /*var ms = new MemoryStream();
                await file.Data.CopyToAsync(ms);*/
                using (FileStream DestinationStream = File.Create(EndDirectory + "file.csv"))
                {
                    await file.Data.CopyToAsync(DestinationStream);
                }
                await CsvService.ImportCSV();
                UploadStatus = $"Finished loading {file.Size} bytes from {file.Name}";
            }
        }
        /// prospect-finder div END
        #endregion
        #region DISPLAY
        public void OpenNewCompaniesDiv()
        {
            NewCompanyDisplay = "block";
            QualifiedDisplay = "none";
            NotQualifiedDisplay = "none";
            LeftStyle = @"'title-new-companies''content' 'content' 'content' 'content' 'content' 'content' 'content' 'content' 'content'
'title-qualified'
'title-not-qualified'";
        }
        public void OpenQualifiedDiv()
        {
            NewCompanyDisplay = "none";
            QualifiedDisplay = "block";
            NotQualifiedDisplay = "none";
            LeftStyle = @"'title-new-companies'
'title-qualified''content' 'content' 'content' 'content' 'content' 'content' 'content' 'content' 'content'
'title-not-qualified'";
        }
        public void OpenNotQualifiedDiv()
        {
            NewCompanyDisplay = "none";
            QualifiedDisplay = "none";
            NotQualifiedDisplay = "block";
            LeftStyle = @"'title-new-companies'
'title-qualified'
'title-not-qualified''content' 'content' 'content' 'content' 'content' 'content' 'content' 'content' 'content'";
        }
        #endregion
    }
}
