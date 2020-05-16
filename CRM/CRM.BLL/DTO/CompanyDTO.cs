using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.BLL.DTO
{
    public class CompanyDTO
    {
        public int Id { get; set; }
        public string CompanyLegalName { get; set; }
        public string TradingName { get; set; }
        public int HGBasedInCountryId { get; set; }
        public string LeadOwnerId { get; set; }
        public int QualificationId { get; set; }
        public string Website { get; set; }
        public DateTime QualifiedDate { get; set; }
        public int CompanyLinkedinId { get; set; }
    }
}
