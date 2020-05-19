using CRM.BLL.DTO;
using CRM.BLL.Interfaces;
using CRM.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BLL.Services
{
    public class TempService : ITempService
    {
        IMailFindService mailFindServ;
        ICompanyService companyServ;
        ILogService _logService;
        public TempService(IMailFindService mailFindService, ICompanyService companyService,
            ILogService logService)
        {
            mailFindServ = mailFindService;
            companyServ = companyService;
            _logService = logService;
        }
        public GetUserDTO CurrentUser { get; set; }
        public IEnumerable<CompanyDTO> AllCompanies { get; set; }
        public IEnumerable<ContactDTO> Contacts { get; set; }
        public IEnumerable<CompanyContactLink> CompanyContactLinks { get; set; }
        public IEnumerable<Linkedin> Linkedins { get; set; }
        public IEnumerable<LogDTO> logs { get; set; }
        public IEnumerable<CompanyDTO> NewCompanies { get; set; }
        public IEnumerable<CompanyDTO> QualifiedCompanies { get; set; }
        public IEnumerable<CompanyDTO> NotQualifiedCompanies { get; set; }
        private int SelectedId { get; set; }
        public async Task<IEnumerable<ContactDTO>> GetCompanyContacts(int CompanyId)
        {
            IEnumerable<CompanyContactLink> _companyContactLinks = CompanyContactLinks.Where(p => p.CompanyId == CompanyId);
            List<ContactDTO> contacts = new List<ContactDTO>();
            foreach (var companyContact in _companyContactLinks)
            {
                ContactDTO contactDTO = await mailFindServ.Map(companyContact.Contact);
                contacts.Add(contactDTO);
            }
            return contacts;
        }

        public int GetSelectedId()
        {
            return SelectedId;
        }

        public void SetId(int Id)
        {
            SelectedId = Id;
        }

        public async Task UpdateCompanies()
        {
            AllCompanies = await companyServ.GetCompanies();
            NewCompanies = await companyServ.GetNewCompanies();
            QualifiedCompanies = await companyServ.GetQualifiedCompanies();
            NotQualifiedCompanies = await companyServ.GetNotQualifiedCompanies();
        }

        public async Task UpdateLogs()
        {
            logs = await _logService.GetLogs();
        }
    }
}
