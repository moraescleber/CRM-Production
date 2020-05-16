using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.BLL.DTO
{
    public class ContactDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public int LinkedinId { get; set; }
    }
}
