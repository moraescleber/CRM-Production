using CRM.BLL.DTO;
using CRM.BLL.Interfaces;
using CRM.DAL.EF;
using CRM.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CRM.BLL.Services
{
    public class HunterIntegrationService : IHunterIntegrationService
    {
        public const string API_SECRET_KEY = "63f602242cad091f3e415c03408f309c450ffd4d";
        ApiContext db;
        ITempService _tempService;
        public HunterIntegrationService(ApiContext context, ITempService tempService)
        {
            db = context;
            _tempService = tempService;
        }
        public async Task<IEnumerable<Contact>> FindDomainContacts(string DomainName)
        {
            Company company = await db.Companies.FindAsync(_tempService.GetSelectedId());
            List<Contact> contacts = new List<Contact>();
            List<ContactDTO> AllContacts = _tempService.Contacts.ToList();
            using HttpClient Http = new HttpClient();
            var response = await Http.GetAsync("https://api.hunter.io/v2/domain-search?domain="+ DomainName + "&api_key="+API_SECRET_KEY);
            HunterResponseDTO FoundContacts = new HunterResponseDTO();
            if(response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                FoundContacts = JsonSerializer.Deserialize<HunterResponseDTO>(result);
            }
            foreach(var contact in FoundContacts.data.emails)
            {
                Linkedin linkedin;
                if(contact.linkedin!=null&& contact.linkedin!="null")
                {
                    linkedin = new Linkedin
                    {
                        FullLink = contact.linkedin
                    };
                    await db.Linkedins.AddAsync(linkedin);
                    Contact newContact = new Contact
                    {
                        Email = contact.value,
                        FirstName = contact.first_name,
                        Linkedin = linkedin,
                        Surname = contact.last_name,
                        Position = contact.position
                    };
                    if(AllContacts.Where(p=>p.Email== contact.value)==null)
                    {
                        await db.Contacts.AddAsync(newContact);
                        CompanyContactLink companyContactLink = new CompanyContactLink
                        {
                            Company = company,
                            Contact = newContact
                        };
                        await db.CompanyContactLinks.AddAsync(companyContactLink);
                        await db.SaveChangesAsync();
                        contacts.Add(newContact);
                    }
                }
                else
                {
                    Contact newContact = new Contact
                    {
                        Email = contact.value,
                        FirstName = contact.first_name,
                        Surname = contact.last_name,
                        Position = contact.position
                    };
                    if (AllContacts.Where(p => p.Email == contact.value).FirstOrDefault() == null)
                    {
                        await db.Contacts.AddAsync(newContact); 
                        CompanyContactLink companyContactLink = new CompanyContactLink
                        {
                            Company = company,
                            Contact = newContact
                        };
                        await db.CompanyContactLinks.AddAsync(companyContactLink);
                        await db.SaveChangesAsync();
                        contacts.Add(newContact);
                    }
                }
            }
            return contacts;
        }
    }
}
