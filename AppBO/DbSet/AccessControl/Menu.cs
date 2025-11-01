using AppBO.Models;
using System.ComponentModel.DataAnnotations;

namespace AppBO.DbSet.AccessControl
{
    public class Menu : TableOption
    {
        [Key]
        public long MenuId { get; set; }
        public string MenuUrl { get; set; }
        public string MenuName { get; set; }
        public string MenuTitle { get; set; }
        public string MenuIcon { get; set; }
        public string Scope { get; set; }
    }
}
