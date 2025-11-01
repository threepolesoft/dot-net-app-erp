using AppBO.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.DbSet.Inventory
{
    public class Color : TableOption
    {
        [Key]
        public long ColorId { get; set; }
        public string ColorName { get; set; }
    }
}
