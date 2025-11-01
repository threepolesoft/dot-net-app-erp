using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.Models
{
    public class MenuElement
    {
        public MenuLeftModel Parent { get; set; }
        public List<MenuElement> Child { get; set; } = new List<MenuElement>();

        // Constructor for easier initialization
        public MenuElement(MenuLeftModel leftMenu)
        {
            Parent = leftMenu;
        }
    }
}
