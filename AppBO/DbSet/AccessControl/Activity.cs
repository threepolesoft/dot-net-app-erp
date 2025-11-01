using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.DbSet.AccessControl
{
    public class Activity
    {
        [Key]
        public long ActivityId { get; set; }
        public string TableName { get; set; }
        public long RowId { get; set; }
        public string Operation { get; set; }
        public string? Comments { get; set; }
        public long User { get; set; }
        public DateTime DateTime { get; set; }
    }
}
