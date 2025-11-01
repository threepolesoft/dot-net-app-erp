using AppBO.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.DbSet.AccessControl
{
    public class RoleDevice: TableOption
    {
        [Key]
        public long RoleDeviceId { get; set; }
        public long RoleId { get; set; }
    }
}
