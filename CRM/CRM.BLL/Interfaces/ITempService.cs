using CRM.BLL.DTO;
using CRM.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BLL.Interfaces
{
    public interface ITempService
    {
        GetUserDTO CurrentUser { get; set; }
        IEnumerable<CompanyDTO> AllCompanies { get; set; }
        IEnumerable<CompanyDTO> NewCompanies { get; set; }
        IEnumerable<CompanyDTO> QualifiedCompanies { get; set; }
        IEnumerable<CompanyDTO> NotQualifiedCompanies { get; set; }
        IEnumerable<ContactDTO> Contacts { get; set; }
        IEnumerable<CompanyModel> CompanyModels { get; set; }
        IEnumerable<CompanyContactLink> CompanyContactLinks { get; set; }
        IEnumerable<Linkedin> Linkedins { get; set; }
        IEnumerable<CountryDTO> Countries { get; set; }
        IEnumerable<LogDTO> logs { get; set; }
        Task UpdateLogs();
        void SetId(int Id);
        int GetSelectedId();
        Task UpdateCompanies();
        Task<IEnumerable<ContactDTO>> GetCompanyContacts(int CompanyId);
    }
}
