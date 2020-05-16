using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.DAL.Entities
{
    public class MailMessage
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
