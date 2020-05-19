using CRM.BLAZOR.Models;
using CRM.BLL.DTO;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace CRM.BLAZOR.Components
{
    public class CompanyInformationBase : ComponentBase
    {
        public int SelectedId;
        public CompanyModel CompanyModel;

        [Inject]
        protected CRM.BLL.Interfaces.ITempService TempService { get; set; }
        [Inject]
        protected CRM.BLL.Interfaces.ILogService LogService { get; set; }

        private async Task AddLog(string TradingName)
        {
            if (TradingName != null)
            {
                LogDTO logDTO = new LogDTO
                {
                    Action = "Просмотрел аккаунт LinkedIn компании  " + TradingName,
                    UserId = TempService.CurrentUser.Id
                };
                await LogService.AddLog(logDTO);
            }
            await TempService.UpdateLogs();
        }
    }
}
