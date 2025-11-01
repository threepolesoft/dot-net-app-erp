using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.Models
{
    public class ApplicationUserModel: TableOption
    {
        public long ApplicationUserId { get; set; }

        [Required(ErrorMessage = "Role id required")]
        [Range(1, long.MaxValue, ErrorMessage = "Role id required")]
        public long RoleId { get; set; }

        [Required(ErrorMessage = "Full name required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "user name required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password required")]
        public string Password { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
