using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.BLL.DTO
{
    public class CountryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Capital { get; set; }
        public int RegionId { get; set; }
    }
}
