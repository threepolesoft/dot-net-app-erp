using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.Models
{
    public class TableOption
    {
        public int OrganizationId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDelete { get; set; } = false;
        public long EntryBy { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime EntryAt { get; set; }
    }
}
