using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.BLAZOR.Models
{
    public class CompanyModel
    {
        public int Id { get; set; }
        public string CompanyLegalName { get; set; }
        public string TradingName { get; set; }
        public string HGBasedInCountryName { get; set; }
        public string LeadOwnerFullName { get; set; }
        public string QualificationName { get; set; }
        public string Website { get; set; }
        public DateTime QualifiedDate { get; set; }
        public string CompanyLinkedinFullLink { get; set; }
    }
}
