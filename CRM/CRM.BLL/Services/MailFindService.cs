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
    public class MailFindService : IMailFindService
    {
        ApiContext db;
        public MailFindService(ApiContext context)
        {
            db = context;
        }
        public async Task<IEnumerable<ContactDTO>> FindContactsForCompany(int CompanyId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ContactDTO>> FindContactsForId(params int[] ContactId)
        {
            List<ContactDTO> contacts = new List<ContactDTO>();
            foreach (int id in ContactId)
            {
                Contact contact = await db.Contacts.FindAsync(id);
                ContactDTO contactDTO = await Map(contact);
                contacts.Add(contactDTO);
            }
            return contacts;
        }

        public async Task<IEnumerable<ContactDTO>> GetAllContacts()
        {
            return await MapRange(await db.Contacts.ToListAsync());
        }

        public async Task<IEnumerable<CompanyContactLink>> GetCompanyContactLinks()
        {
            return await db.CompanyContactLinks
                .Include(p => p.Contact).ThenInclude(p => p.Linkedin)
                .Include(p=>p.Company).ToListAsync();
        }

        public async Task<IEnumerable<ContactDTO>> GetCompanyContacts(int CompanyId)
        {
            IEnumerable<CompanyContactLink> companyContactLinks = await db.CompanyContactLinks.Where(p => p.CompanyId == CompanyId)
                .Include(p => p.Contact).ThenInclude(p => p.Linkedin).ToListAsync();
            List<ContactDTO> contacts = new List<ContactDTO>();
            foreach (var companyContact in companyContactLinks)
            {
                ContactDTO contactDTO = await Map(companyContact.Contact);
                contacts.Add(contactDTO);
            }
            return contacts;
        }

        public async Task<ContactDTO> Map(Contact contact)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Contact, ContactDTO>()).CreateMapper();
            ContactDTO ContactDTO = mapper.Map<ContactDTO>(contact);
            return ContactDTO;
        }
        public async Task<IEnumerable<ContactDTO>> MapRange(IEnumerable<Contact> contacts)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Contact, ContactDTO>()).CreateMapper();
            IEnumerable<ContactDTO> ContactDTO = mapper.Map<IEnumerable<ContactDTO>>(contacts);
            return ContactDTO;
        }
        public async Task<IEnumerable<Email>> SendToLemmlist(params int[] ContactId)
        {
            throw new NotImplementedException();
        }
    }
}
