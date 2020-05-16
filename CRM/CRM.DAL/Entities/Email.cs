using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.DAL.Entities
{
    public class Email
    {
        public int Id { get; set; }
        public string LeadOwnerSenderId { get; set; }
        public User LeadOwnerSender { get; set; }
        public string ClientAddresseeId { get; set; }
        public Contact ClientAddressee { get; set; }
        public bool IsSended { get; set; }
        public int MessageId { get; set; }
        public MailMessage Message { get; set; }
        public string MessageResponse { get; set; }
        public DateTime SendDate { get; set; } = DateTime.Now;
    }
}
