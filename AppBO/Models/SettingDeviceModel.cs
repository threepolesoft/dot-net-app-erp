using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.Models
{
    public class SettingDeviceModel: TableOption
    {
        public long SettingDeviceId { get; set; }
        public string DeviceId { get; set; }
        public long UserId { get; set; }
        public long SettingId { get; set; }
        public string SettingName { get; set; }
        public string SettingTitle { get; set; }
        public string Value { get; set; }
        public string? Scope { get; set; }

    }
}
