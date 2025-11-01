using AppBO.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.DbSet.AccessControl
{
    public class RoleUser: TableOption
    {
        [Key]
        public long RoleUserId { get; set; }
        public long ApplicationUserId { get; set; }
        public long RoleId { get; set; }
    }
}
