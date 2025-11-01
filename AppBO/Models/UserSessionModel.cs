using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.Models
{
    public class UserSessionModel
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }

        public List<string> Roles { get; set; }
    }
}
