using AppBO.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.DbSet.AccessControl
{
    public class RoleMenu : TableOption
    {
        [Key]
        public long RoleMenuId { get; set; }
        public long RoleId { get; set; }
        public long MenuDetailId { get; set; }
    }
}
