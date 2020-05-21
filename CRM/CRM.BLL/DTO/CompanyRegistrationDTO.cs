using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CRM.BLL.DTO
{
    public class CompanyRegistrationDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Введите Company Legal Name")]
        public string CompanyLegalName { get; set; }
        [Required(ErrorMessage = "Введите Trading Name")]
        public string TradingName { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Выберите страну")]
        public int HGBasedInCountryId { get; set; }
        public string Website { get; set; }
        public string CompanyLinkedinLink { get; set; }
    }
}
