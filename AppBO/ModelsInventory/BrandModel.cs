using AppBO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.ModelsInventory
{
    public class BrandModel: TableOption
    {
        public long BrandId { get; set; }
        public string BrandName { get; set; }
    }
}
