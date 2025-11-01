using AppBO.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.ModelsApi
{
    public class SaveRoleModelReq
    {
        [Required(ErrorMessage = "Role Id is required")]
        [Range(0, long.MaxValue, ErrorMessage = "Role Id must be greater than or equal to 0")]
        public long RoleId { get; set; }

        [Required(ErrorMessage = "RoleName is required")]
        public string RoleTitle { get; set; }

        [Required(ErrorMessage ="Scope is required")]
        public string Scope { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public bool IsActive { get; set; } = true;
        public bool IsDelete { get; set; } = false;
    }

    public class UserRoleAllRes
    {
        public long ApplicationUserId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public List<RoleModel> Roles { get; set; } = new List<RoleModel>();
    }
}
