using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.Models
{
    public class DeviceModel
    {
        public int LId { get; set; }

        public string Username { get; set; } = null!;

        public string? MobDeviceId { get; set; }

        public string? DeviceInformation { get; set; }

        public string? AppVersion { get; set; }

        public int? AppVersionCode { get; set; }

        public List<RoleModel> Roles { get; set; }
    }
}
