using AppBO.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.DbSet.Inventory
{
    public class Size: TableOption
    {
        [Key]
        public long SizeId { get; set; }
        public string SizeName { get; set; }
    }
}
