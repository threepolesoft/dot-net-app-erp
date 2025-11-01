using AppBO.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.DbSet.AccessControl
{
    public class SettingUser: TableOption
    {
        [Key]
        public long SettingUserId { get; set; }
        public long ApplicationUserId { get; set; }
        public long SettingId { get; set; }
        public string Value { get; set; }
    }
}
