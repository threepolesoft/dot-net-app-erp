using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.Models
{
    public class RoleMenuModel: TableOption
    {
        public long RoleMenuId { get; set; }
        public long RoleId { get; set; }
        public long MenuDetailId { get; set; }
        public bool Status { get; set; }
    }
}
