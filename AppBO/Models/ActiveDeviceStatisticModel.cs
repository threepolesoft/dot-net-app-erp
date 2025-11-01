using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.Models
{
    public class ActiveDeviceStatisticModel
    {
        public string Platform { get; set; }

        public ConcurrentDictionary<string, string> Devices = new();

        public int Total => Devices.Count;
    }
}
