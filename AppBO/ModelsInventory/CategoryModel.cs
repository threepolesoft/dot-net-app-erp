using AppBO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.ModelsInventory
{
    public class CategoryModel: TableOption
    {
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
