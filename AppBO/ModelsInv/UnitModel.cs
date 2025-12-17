using AppBO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.ModelsInv
{
    public class UnitModel: TableOption
    {
        public long UnitId { get; set; }
        public string UnitName { get; set; }
    }
}
