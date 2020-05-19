using CRM.BLL.DTO;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace CRM.BLAZOR.Components
{
    public class CompaniesBase : ComponentBase 
    {
        [Inject]
        protected CRM.BLL.Interfaces.ITempService TempService { get; set; }
        [Inject]
        protected CRM.BLL.Interfaces.ILogService LogService { get; set; }
        protected string LeftStyle { get; set; } = @"'title-new-companies' 'content' 'content' 'content' 'content' 'content' 'content' 'content' 'content' 'content'
'title-qualified'
'title-not-qualified'";
        protected string NewCompanyDisplay { get; set; } = "block";
        protected string QualifiedDisplay { get; set; } = "none";
        protected string NotQualifiedDisplay { get; set; } = "none";
        [Parameter]
        public IEnumerable<CompanyDTO> NewCompanies { get; set; }
        [Parameter]
        public IEnumerable<CompanyDTO> QualifiedCompanies { get; set; }
        [Parameter]
        public IEnumerable<CompanyDTO> NotQualifiedCompanies { get; set; }
        protected int SelectedId { get; set; }
        /*public override async Task SetParametersAsync(ParameterView parameters)
        {
            //await base.SetParametersAsync(parameters);
        }*/
        protected async Task selectCompanyElement(int id)
        {
            TempService.SetId(id);
            SelectedId = id;
            await AddLog(id);
        }
        private async Task AddLog(int CompanyId = 0, string WebSite = null)
        {
            if (WebSite != null)
            {
                LogDTO logDTO = new LogDTO
                {
                    Action = "Перешел на сайт " + WebSite,
                    UserId = TempService.CurrentUser.Id
                };
                await LogService.AddLog(logDTO);
                Thread.Sleep(3000);
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
            await TempService.UpdateLogs();
        }
        #region Display
        private void openNewCompaniesDiv()
        {
            NewCompanyDisplay = "block";
            QualifiedDisplay = "none";
            NotQualifiedDisplay = "none";
            LeftStyle = @"'title-new-companies''content' 'content' 'content' 'content' 'content' 'content' 'content' 'content' 'content'
'title-qualified'
'title-not-qualified'";
        }
        private void openQualifiedDiv()
        {
            NewCompanyDisplay = "none";
            QualifiedDisplay = "block";
            NotQualifiedDisplay = "none";
            LeftStyle = @"'title-new-companies'
'title-qualified''content' 'content' 'content' 'content' 'content' 'content' 'content' 'content' 'content'
'title-not-qualified'";
        }
        private void openNotQualifiedDiv()
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
