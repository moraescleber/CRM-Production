using CRM.BLL.DTO;
using CRM.BLL.Interfaces;
using CRM.DAL.EF;
using CRM.DAL.Entities;
using Microsoft.EntityFrameworkCore;
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
        ApiContext db;
        ICountryService countryServ;
        IUserRegistrationService userRegistrationServ;
        public TempService(IMailFindService mailFindService, ICompanyService companyService,
            ILogService logService, ApiContext context, IUserRegistrationService userRegistrationService, ICountryService countryService)
        {
            mailFindServ = mailFindService;
            companyServ = companyService;
            _logService = logService;
            db = context;
            countryServ = countryService;
            userRegistrationServ = userRegistrationService;
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
        public IEnumerable<CompanyModel> CompanyModels { get; set; }
        public IEnumerable<CountryDTO> Countries { get; set; }
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
            Linkedins = await db.Linkedins.ToListAsync();
            CompanyContactLinks = await mailFindServ.GetCompanyContactLinks();
            Countries = await countryServ.GetCountries();
            Contacts = await mailFindServ.GetAllContacts();
            List<CompanyModel> companies = new List<CompanyModel>();
            await Task.Run(async () =>
            {
                foreach (var company in await companyServ.GetCompanies())
                {
                    var Qualifications = await db.CompanyQualifications.ToListAsync();
                    var QualificationName = Qualifications.Where(p => p.Id == company.QualificationId).FirstOrDefault().QualificationName;
                    var lead = await userRegistrationServ.GetUserFullName(company.LeadOwnerId);
                    var country = await countryServ.GetCountry(company.HGBasedInCountryId);

                    string linkedinLink = null;
                    if (company.CompanyLinkedinId != 0)
                    {
                        var linkedin = await db.Linkedins.FindAsync(company.CompanyLinkedinId);
                        linkedinLink = linkedin.FullLink;

                    }
                    CompanyModel companyModel = new CompanyModel
                    {
                        CompanyLegalName = company.CompanyLegalName,
                        HGBasedInCountryName = country.Name,
                        Id = company.Id,
                        LeadOwnerFullName = lead,
                        QualificationName = QualificationName,
                        QualifiedDate = company.QualifiedDate,
                        TradingName = company.TradingName,
                        Website = company.Website,
                        CompanyLinkedinFullLink = linkedinLink
                    };
                    companies.Add(companyModel);
                }
            }
            );
            CompanyModels = companies;
            await UpdateLogs();
            /*IEnumerable<CompanyDTO> companies = await companyServ.GetCompanies();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CompanyDTO, CompanyModel>()
            .ForMember(p=>p.HGBasedInCountryName, p=>p.MapFrom(s=>countryServ.GetCountry(s.HGBasedInCountryId).Result.Name))
            .ForMember(p => p.LeadOwnerFullName, p => p.MapFrom(s =>  userRegistrationServ.GetUserFullName(s.LeadOwnerId)))
            .ForMember(p => p.QualificationName, p => p.MapFrom(s => qualificationServ.GetQualifications().Result.Where(p=>p.Id==s.QualificationId).FirstOrDefault().QualificationName))
            ).CreateMapper();
            companyModels  = mapper.Map<IEnumerable<CompanyDTO>, IEnumerable<CompanyModel>>(companies);*/
        }

        public async Task UpdateLogs()
        {
            logs = await _logService.GetLogs();
        }
    }
}
