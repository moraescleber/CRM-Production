using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.DAL.Entities
{
    public class CompanyContactLink
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public int ContactId { get; set; }
        public Contact Contact { get; set; }
    }
}
