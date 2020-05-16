using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.DAL.Entities
{
    public class Linkedin
    {
        public int Id { get; set; }
        public string FullLink { get; set; }

        public virtual List<Company> Companies { get; set; }
        public virtual List<Contact> Contacts { get; set; }
    }
}
