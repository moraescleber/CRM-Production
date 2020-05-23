using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.BLL.DTO
{
    public class HunterResponseDTO
    {
        public data data { get; set; }
    }
    public class data
    {
        public string domain { get; set; }
        public IEnumerable<emails> emails { get; set; }
    }
    public class emails
    {
        public string value { get; set; }
        public string position { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string linkedin { get; set; }
    }
}
