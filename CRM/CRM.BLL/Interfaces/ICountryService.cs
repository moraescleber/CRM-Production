using CRM.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BLL.Interfaces
{
    public interface ICountryService
    {
        Task<CountryDTO> CreateCountry(CountryDTO CountryRegistrationDTO);
        Task<CountryDTO> GetCountry(int CountryId);
        Task<IEnumerable<CountryDTO>> GetCountries();
        Task<bool> DeleteCountry(int CountryId);
        Task<CountryDTO> EditCountry(CountryDTO countryDTO);
        Task<IEnumerable<CountryDTO>> GetRegionCountries(int RegionId);
    }
}
