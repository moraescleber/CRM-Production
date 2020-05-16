using AutoMapper;
using CRM.BLL.DTO;
using CRM.BLL.Interfaces;
using CRM.DAL.EF;
using CRM.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BLL.Services
{
    public class CompanyService : ICompanyService
    {
        readonly ApiContext db;
        readonly ICountryService countryServ;
        readonly UserManager<User> _userManager;
        public CompanyService(ApiContext context, ICountryService countryService, UserManager<User> userManager)
        {
            db = context;
            countryServ = countryService;
            _userManager = userManager;
        }
        public async Task<CompanyDTO> AppointLead(AppointCompanyLeadDTO companyLeadDTO)
        {
            Company company = await db.Companies.FindAsync(companyLeadDTO.CompanyId);
            User user = await _userManager.FindByIdAsync(companyLeadDTO.LeadId);
            if (company == null)
            {
                throw new Exception("Id компании не правильная");
            }
            if (user == null)
            {
                throw new Exception("Не верный LeadId");
            }
            if (company.LeadOwnerId == user.Id)
            {
                throw new Exception("Lead уже назначен");
            }
            company.LeadOwnerId = user.Id;
            db.Companies.Update(company);
            await db.SaveChangesAsync();
            return await GetCompany(company.Id);
        }

        public async Task<CompanyRegistrationDTO> CreateCompany(CompanyRegistrationDTO CompanyRegistrationDTO)
        {
            CompanyQualification NewCompany = await db.CompanyQualifications.Where(p => p.QualificationName == "NewCompany").FirstOrDefaultAsync();
            var country = countryServ.GetCountry(CompanyRegistrationDTO.HGBasedInCountryId);
            if (country == null)
            {
                throw new Exception("Id Страны не правильная");
            }
            Company company;
            if (CompanyRegistrationDTO.CompanyLinkedinLink != null)
            {
                Linkedin linkedin = new Linkedin { FullLink = CompanyRegistrationDTO.CompanyLinkedinLink };
                await db.Linkedins.AddAsync(linkedin);
                company = new Company
                {
                    CompanyLegalName = CompanyRegistrationDTO.CompanyLegalName,
                    HGBasedInCountryId = CompanyRegistrationDTO.HGBasedInCountryId,
                    Qualification = NewCompany,
                    TradingName = CompanyRegistrationDTO.TradingName,
                    Website = CompanyRegistrationDTO.Website,
                    CompanyLinkedin = linkedin
                };
            }
            else
            {
                company = new Company
                {
                    CompanyLegalName = CompanyRegistrationDTO.CompanyLegalName,
                    HGBasedInCountryId = CompanyRegistrationDTO.HGBasedInCountryId,
                    Qualification = NewCompany,
                    TradingName = CompanyRegistrationDTO.TradingName,
                    Website = CompanyRegistrationDTO.Website
                };
            }
            await db.Companies.AddAsync(company);
            await db.SaveChangesAsync();
            return CompanyRegistrationDTO;
        }

        public async Task<bool> DeleteCompany(int CompanyId)
        {
            Company company = await db.Companies.FindAsync(CompanyId);
            if (company == null) return false;
            db.Companies.Remove(company);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<CompanyRegistrationDTO> EditCompany(CompanyRegistrationDTO CompanyDTO)
        {
            Company company = await db.Companies.FindAsync(CompanyDTO.Id);

            company.CompanyLegalName = CompanyDTO.CompanyLegalName;
            var country = countryServ.GetCountry(CompanyDTO.HGBasedInCountryId);
            if (country == null)
            {
                throw new Exception("Id Страны не правильная");
            }
            company.HGBasedInCountryId = CompanyDTO.HGBasedInCountryId;
            company.TradingName = CompanyDTO.TradingName;
            company.Website = CompanyDTO.Website;
            db.Companies.Update(company);
            await db.SaveChangesAsync();
            return CompanyDTO;
        }

        public async Task<IEnumerable<CompanyDTO>> GetCompanies()
        {
            IEnumerable<Company> companies = await db.Companies.ToListAsync();
            IEnumerable<CompanyDTO> countryLangDTOs = GetMapper().Map<IEnumerable<Company>, IEnumerable<CompanyDTO>>(companies);
            return countryLangDTOs;
        }

        public async Task<CompanyDTO> GetCompany(int CompanyId)
        {
            Company company = await db.Companies.FindAsync(CompanyId);
            return GetMapper().Map<CompanyDTO>(company);
        }

        public async Task<IEnumerable<CompanyDTO>> GetCountryCompanies(int CountryId)
        {
            IEnumerable<CompanyDTO> companies = await GetCompanies();
            return companies.Where(p => p.HGBasedInCountryId == CountryId);
        }

        public async Task<IEnumerable<CompanyDTO>> GetNewCompanies()
        {
            CompanyQualification companyQualification = await db.CompanyQualifications.Where(p => p.QualificationName == "NewCompany").FirstOrDefaultAsync();
            IEnumerable<CompanyDTO> NewCompanies = await GetCompanies();
            return NewCompanies.Where(p => p.QualificationId == companyQualification.Id);
        }

        public async Task<IEnumerable<CompanyDTO>> GetNotQualifiedCompanies()
        {
            CompanyQualification companyQualification = await db.CompanyQualifications.Where(p => p.QualificationName == "NotQualified").FirstOrDefaultAsync();
            IEnumerable<CompanyDTO> NotQualifiedCompanies = await GetCompanies();
            return NotQualifiedCompanies.Where(p => p.QualificationId == companyQualification.Id);
        }

        public async Task<IEnumerable<CompanyDTO>> GetQualifiedCompanies()
        {
            CompanyQualification companyQualification = await db.CompanyQualifications.Where(p => p.QualificationName == "Qualified").FirstOrDefaultAsync();
            IEnumerable<CompanyDTO> QualifiedCompanies = await GetCompanies();
            return QualifiedCompanies.Where(p => p.QualificationId == companyQualification.Id);
        }

        public async Task<IEnumerable<CompanyDTO>> GetRegionCompanies(int RegionId)
        {
            IEnumerable<CountryDTO> countries = await countryServ.GetRegionCountries(RegionId);
            List<CompanyDTO> companies = new List<CompanyDTO>();
            foreach (var country in countries)
            {
                var companyDTOs = await GetCountryCompanies(country.Id);
                foreach (var companyDTO in companyDTOs)
                {
                    if (!companies.Contains(companyDTO))
                    {
                        companies.Add(companyDTO);
                    }
                }
            }
            return companies;
        }

        public async Task<CompanyDTO> QualifyCompany(QualifyCompanyDTO QualifyCompanyDTO)
        {
            Company company = await db.Companies.FindAsync(QualifyCompanyDTO.CompanyId);
            CompanyQualification companyQualification = await db.CompanyQualifications.FindAsync(QualifyCompanyDTO.CompanyQualificationId);
            if (company == null)
            {
                throw new Exception("Id компании не правильная");
            }
            if (companyQualification == null)
            {
                throw new Exception("Id кваливикации не правильная");
            }
            if (company.QualificationId == QualifyCompanyDTO.CompanyQualificationId)
            {
                throw new Exception("Компания " + company.TradingName + " уже " + companyQualification.QualificationName);
            }
            company.QualificationId = companyQualification.Id;
            company.QualifiedDate = DateTime.Now;
            db.Companies.Update(company);
            await db.SaveChangesAsync();
            return await GetCompany(company.Id);
        }

        public async Task<bool> SetNotQualified(int CompanyId)
        {
            CompanyQualification companyQualification = await db.CompanyQualifications.Where(p => p.QualificationName == "NotQualified").FirstOrDefaultAsync();
            QualifyCompanyDTO qualifyCompanyDTO = new QualifyCompanyDTO { CompanyId = CompanyId, CompanyQualificationId = companyQualification.Id };
            try
            {
                await QualifyCompany(qualifyCompanyDTO);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<bool> SetQualified(int CompanyId)
        {
            CompanyQualification companyQualification = await db.CompanyQualifications.Where(p => p.QualificationName == "Qualified").FirstOrDefaultAsync();
            QualifyCompanyDTO qualifyCompanyDTO = new QualifyCompanyDTO { CompanyId = CompanyId, CompanyQualificationId = companyQualification.Id };
            try
            {
                await QualifyCompany(qualifyCompanyDTO);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private IMapper GetMapper()
        {
            return new MapperConfiguration(cfg => cfg.CreateMap<Company, CompanyDTO>()).CreateMapper();
        }
    }
}
