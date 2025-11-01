using AppBO.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.DbSet.AccessControl
{
    public class Role: TableOption
    {
        [Key]
        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleTitle { get; set; }
        public string Scope { get; set; }

    }
}
