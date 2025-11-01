using AppBO.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.DbSet.AccessControl
{
    public class MenuUser: TableOption
    {
        [Key]
        public long UserMenuId { get; set; }
        public long ApplicationUserId { get; set; }
        public long MenuId { get; set; }
    }
}
