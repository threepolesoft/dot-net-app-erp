using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.Models
{
    public class MenuModel : TableOption
    {
        public long MenuId { get; set; }
        public string MenuName { get; set; }
        public string MenuUrl { get; set; } = "";

        [Required]
        public string MenuTitle { get; set; }
        public string MenuIcon { get; set; } = "";
        public string Scope { get; set; }
    }
}
