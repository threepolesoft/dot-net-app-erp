using AppBO.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.DbSet.Inventory
{
    public class Category: TableOption
    {
        [Key]
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
