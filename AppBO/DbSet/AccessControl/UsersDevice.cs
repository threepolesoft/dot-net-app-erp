using AppBO.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.DbSet.AccessControl
{
    public class UsersDevice : TableOption
    {
        [Key]
        public long UsersDeviceId { get; set; }
        public long UserId { get; set; }

        [MaxLength(32)]
        public string DeviceId { get; set; }

        [MaxLength(50)]
        public string DeviceName { get; set; }

        [MaxLength(3)]
        public string Scope { get; set; }

        public string? Platform { get; set; } = "App";

        public DateTime? LoginAt { get; set; }

    }
}
