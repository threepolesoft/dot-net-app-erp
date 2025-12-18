using AppBO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.ModelsInventory
{
    public class ColorModel: TableOption
    {
        public long ColorId { get; set; }
        public string ColorName { get; set; }
    }
}
