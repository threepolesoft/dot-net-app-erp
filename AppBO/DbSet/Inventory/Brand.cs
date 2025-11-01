using AppBO.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.DbSet.Inventory
{
    public class Brand: TableOption
    {
        [Key]
        public long BrandId { get; set; }
        public string BrandName { get; set; }
    }
}
