using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.BLL.DTO
{
    public class GetUserDTO
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
