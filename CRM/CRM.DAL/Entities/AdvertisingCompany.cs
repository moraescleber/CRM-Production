using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace CRM.DAL.Entities
{
    public class AdvertisingCompany
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonPropertyName("_id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
