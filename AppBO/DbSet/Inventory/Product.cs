using AppBO.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.DbSet.Inventory
{
    public class Product : TableOption
    {
        [Key]
        public long ProductId { get; set; }
        public long CategoryId { get; set; }
        public long SupplierId { get; set; }
        public long UnitId { get; set; }
        public long BrandId { get; set; }
        public long PriceId { get; set; }
        public long ColorId { get; set; }
        public long SizeId { get; set; }
        public string SKU { get; set; }
        public string ProductName { get; set; }
    }
}
