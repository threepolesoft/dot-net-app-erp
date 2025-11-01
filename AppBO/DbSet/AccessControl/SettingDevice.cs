using AppBO.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.DbSet.AccessControl
{
    public class SettingDevice: TableOption
    {
        [Key]
        public long SettingDeviceId { get; set; }
        public string DeviceId { get; set; }
        public long UserId { get; set; }
        public long SettingId { get; set; }
        public string Value { get; set; }
    }
}
