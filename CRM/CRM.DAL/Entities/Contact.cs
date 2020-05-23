using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.DAL.Entities
{
    public class Contact
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Surname { get; set; }
        public int? LinkedinId { get; set; }
        public Linkedin Linkedin { get; set; }

        public virtual List<Email> Emails { get; set; }
        public virtual List<CompanyContactLink> CompanyContactLinks { get; set; }
    }
}
