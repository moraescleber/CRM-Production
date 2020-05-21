using CRM.BLL.DTO;
using CRM.BLL.Interfaces;
using CRM.DAL.EF;
using CRM.DAL.Entities;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BLL.Services
{
    public class CsvService : ICsvService
    {
        const string CSV_PATH = "wwwroot/files/file.csv";
        ICompanyService _companyService;
        ApiContext db;
        public CsvService(ICompanyService companyService, ApiContext context)
        {
            _companyService = companyService;
            db = context;
        }
        public async Task ExportCSV(IEnumerable<CompanyDTO> companies)
        {
            throw new NotImplementedException();
        }

        public async Task ImportCSV()
        {
            List<CompanyCsvModel> Companies;
            using (StreamReader streamReader = new StreamReader(CSV_PATH))
            {
                using (CsvReader csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    // указываем используемый разделитель
                    csvReader.Configuration.Delimiter = ",";
                    csvReader.Configuration.HeaderValidated = null;
                    csvReader.Configuration.MissingFieldFound = null;
                    // получаем строки
                    Companies = csvReader.GetRecords<CompanyCsvModel>().ToList();
                }
            }
            foreach (var company in Companies ?? Enumerable.Empty<CompanyCsvModel>())
            {
                List<Region> regions = await db.Regions.Where(p => p.Name == company.RegionName).ToListAsync();
                Region region;
                if (regions.Count==0)
                {
                    region = new Region { Name = company.RegionName };
                    await db.Regions.AddAsync(region);
                    await db.SaveChangesAsync();
                }
                else
                {
                    region = regions.First();
                }
                Country country = await db.Countries.Where(p => p.RegionId == region.Id&&p.Name==company.HGBasedInCountry).FirstOrDefaultAsync();
                if(country==null)
                {
                    country = new Country { Name = company.HGBasedInCountry, RegionId = region.Id };
                    await db.Countries.AddAsync(country);
                    await db.SaveChangesAsync();
                }

                CompanyRegistrationDTO newCompany = new CompanyRegistrationDTO
                {
                    CompanyLegalName = company.CompanyLegalName,
                    TradingName = company.TradingName,
                    Website = company.Website,
                    CompanyLinkedinLink = company.CompanyLinkedinLink,
                    HGBasedInCountryId = country.Id
                };
                try
                {
                    await _companyService.CreateCompany(newCompany);
                }
                catch(Exception ex)
                {

                }
            }
        }
    }
}
