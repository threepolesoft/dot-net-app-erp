using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.Models
{
    public class RoleUserModel: TableOption
    {
        public long RoleUserId { get; set; }
        public long ApplicationUserId { get; set; }
        public long RoleId { get; set; }
    }
}
