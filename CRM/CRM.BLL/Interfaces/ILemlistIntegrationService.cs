using CRM.BLL.DTO;
using CRM.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BLL.Interfaces
{
    public interface ILemlistIntegrationService
    {
        Task<IEnumerable<AdvertisingCompany>> GetAdvertisingCompanies();
        Task<IEnumerable<AddLeadInCampaignResult>> AddLeadsInCampaign(List<ContactDTO> contacts);
    }
}
