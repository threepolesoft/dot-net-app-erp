using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.Models
{
    public class MenuLeftModel
    {
        public long MenuDetailId { get; set; }
        public long RoleId { get; set; }
        public long MenuId { get; set; }
        public int Parent { get; set; }
        public int Serial { get; set; }
        public string MenuTitle { get; set; }
        public string MenuIcon { get; set; } = "";
        public string MenuUrl { get; set; } = "";
        public bool IsView { get; set; }
        public bool Status { get; set; }
        public List<MenuLeftModel> SubMenus { get; set; }
    }
}
