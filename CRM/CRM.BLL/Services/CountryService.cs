using AutoMapper;
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
    public class CountryService : ICountryService
    {
        readonly ApiContext db;
        public CountryService(ApiContext context)
        {
            db = context;
        }
        public async Task<CountryDTO> CreateCountry(CountryDTO CountryRegistrationDTO)
        {
            Region region = await db.Regions.FindAsync(CountryRegistrationDTO.RegionId);
            Country country = new Country
            {
                Capital = CountryRegistrationDTO.Capital,
                Name = CountryRegistrationDTO.Name,
                Region = region
            };
            await db.Countries.AddAsync(country);
            await db.SaveChangesAsync();
            return CountryRegistrationDTO;
        }

        public async Task<bool> DeleteCountry(int CountryId)
        {
            Country country = await db.Countries.FindAsync(CountryId);
            if (country == null)
            {
                return false;
            }
            db.Countries.Remove(country);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<CountryDTO> EditCountry(CountryDTO countryDTO)
        {
            Country country = await db.Countries.FindAsync(countryDTO.Id);
            Region region = await db.Regions.FindAsync(countryDTO.RegionId);
            country.Name = countryDTO.Name;
            country.Capital = countryDTO.Capital;
            country.Region = region;
            db.Countries.Update(country);
            await db.SaveChangesAsync();
            return countryDTO;
        }

        public async Task<IEnumerable<CountryDTO>> GetCountries()
        {
            IEnumerable<Country> countries = await db.Countries.ToListAsync();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Country, CountryDTO>()).CreateMapper();
            IEnumerable<CountryDTO> countryLangDTOs = mapper.Map<IEnumerable<Country>, IEnumerable<CountryDTO>>(countries);
            return countryLangDTOs;
        }

        public async Task<CountryDTO> GetCountry(int CountryId)
        {
            try
            {

                Country country = await db.Countries.FindAsync(CountryId);

                CountryDTO countryDTO = new CountryDTO
                {
                    Capital = country.Capital,
                    Id = CountryId,
                    Name = country.Name,
                    RegionId = country.RegionId
                };
                return countryDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<CountryDTO>> GetRegionCountries(int RegionId)
        {
            IEnumerable<CountryDTO> countries = await GetCountries();
            return countries.Where(p => p.RegionId == RegionId);
        }
    }
}
