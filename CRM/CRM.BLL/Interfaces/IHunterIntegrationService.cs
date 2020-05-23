using CRM.BLL.DTO;
using CRM.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BLL.Interfaces
{
    public interface IHunterIntegrationService
    {
        Task<IEnumerable<Contact>> FindDomainContacts(string DomainName);
    }
}
