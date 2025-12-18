using AppBO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.ModelsInventory
{
    public class SizeModel: TableOption
    {
        public long SizeId { get; set; }
        public string SizeName { get; set; }
    }
}
