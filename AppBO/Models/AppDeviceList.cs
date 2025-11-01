using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.Models
{
    public class AppDeviceList
    {
        public string? MobDeviceId { get; set; }

        public string? DeviceInformation { get; set; }

        public List<SettingModel> Settings { get; set; }
    }
}
