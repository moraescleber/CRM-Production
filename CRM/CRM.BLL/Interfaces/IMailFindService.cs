using CRM.BLL.DTO;
using CRM.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BLL.Interfaces
{
    public interface IMailFindService
    {
        Task<IEnumerable<ContactDTO>> FindContactsForCompany(int CompanyId);
        Task<IEnumerable<ContactDTO>> FindContactsForId(params int[] ContactId);
        Task<IEnumerable<Email>> SendToLemmlist(params int[] ContactId);
        Task<IEnumerable<ContactDTO>> GetCompanyContacts(int CompanyId);
        Task<IEnumerable<ContactDTO>> GetAllContacts();
        Task<IEnumerable<CompanyContactLink>> GetCompanyContactLinks();
        Task<ContactDTO> Map(Contact contact);
    }
}
