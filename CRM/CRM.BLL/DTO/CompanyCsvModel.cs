using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.BLL.DTO
{
    public class CompanyCsvModel
    {
        public int Id { get; set; }
        [Name("Company Legal Name")]
        public string CompanyLegalName { get; set; }
        [Name("Trading Name")]
        public string TradingName { get; set; }
        [Name("Region")]
        public string RegionName { get; set; }
        [Name("HQ based in country")]
        public string HGBasedInCountry { get; set; }
        [Name("Valid website link OR leave blank")]
        public string Website { get; set; }
        [Name("Generic email")]
        public string CompanyLinkedinLink { get; set; }
    }
}
