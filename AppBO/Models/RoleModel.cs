using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.Models
{
    public class RoleModel: TableOption
    {
        public long RoleId { get; set; }
        [Required]
        public string RoleTitle { get; set; }  
        public string RoleName { get; set; }
        public string Scope { get; set; }
        public bool Status { get; set; }
    }
}
