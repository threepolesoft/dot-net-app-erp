using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.Models
{
    public class RoleAccessModel
    {
        public long RoleId { get; set; }
        public string RoleTitle { get; set; }
        public string[] CheckedKeys { get; set; }
        public List<MenuLeftModel> Menus { get; set; } = new List<MenuLeftModel>();
        public List<MenuElement> Elements { get; set; } = new List<MenuElement>();
    }
}
