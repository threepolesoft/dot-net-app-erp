using AppBO.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.DbSet.Inventory
{
    public class Unit: TableOption
    {
        [Key]
        public long UnitId { get; set; }
        public string UnitName { get; set; }
    }
}
