using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.Models
{
    public class MenuDetailModel: TableOption
    {
        public long MenuDetailId { get; set; }
        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "Select a menu")]
        public long MenuId { get; set; }
        public int Parent { get; set; }
        public int Serial { get; set; }
        public string MenuName { get; set; }
        public string MenuUrl { get; set; } = "";
        public string MenuTitle { get; set; }
        public string MenuIcon { get; set; } = "";
        public string Scope { get; set; }
        public bool IsView { get; set; }
    }
}
