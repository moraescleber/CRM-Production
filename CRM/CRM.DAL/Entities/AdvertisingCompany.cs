using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CRM.DAL.Entities
{
    public class AdvertisingCompany
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
