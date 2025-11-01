using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.Models
{
    public class UsersDeviceModel: TableOption
    {
        public long UsersDeviceId { get; set; }
        public long UserId { get; set; }

        public string DeviceId { get; set; }

        public string DeviceName { get; set; }

        public string Scope { get; set; }

        public string? Platform { get; set; } = "App";

        public DateTime? LoginAt { get; set; }
    }
}
