using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BLL.Interfaces
{
    public interface ILemlistIntegrationService
    {
        Task GetAdvertisingCompanies();
        Task AddLeadInCampaign(string contact);
    }
}
