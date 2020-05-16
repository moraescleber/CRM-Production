using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.BLL.DTO
{
    public class CompanyRegistrationDTO
    {
        public int Id { get; set; }
        public string CompanyLegalName { get; set; }
        public string TradingName { get; set; }
        public int HGBasedInCountryId { get; set; }
        public string Website { get; set; }
        public string CompanyLinkedinLink { get; set; }
    }
}
