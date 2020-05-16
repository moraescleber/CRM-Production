using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.DAL.Entities
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Capital { get; set; }
        public int RegionId { get; set; }
        public Region Region { get; set; }
    }
}
