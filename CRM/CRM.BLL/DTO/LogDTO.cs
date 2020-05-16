using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.BLL.DTO
{
    public class LogDTO
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
