using AppBO.Models;
using System.ComponentModel.DataAnnotations;

namespace AppBO.DbSet.AccessControl
{
    public class MenuDetail : TableOption
    {
        [Key]
        public long MenuDetailId { get; set; }
        public long MenuId { get; set; }
        public int Parent { get; set; }
        public int Serial { get; set; }
        public bool IsView { get; set; }
    }
}
