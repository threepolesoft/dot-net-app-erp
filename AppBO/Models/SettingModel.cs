using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.Models
{
    public class SettingModel: TableOption
    {
        public long SettingId { get; set; }
        public string SettingName { get; set; }

        [Required(ErrorMessage = "Option title required")]
        public string SettingTitle { get; set; }
        public long UserId { get; set; }
        public string DeviceId { get; set; }
        public string Scope { get; set; }
        public string Value { get; set; }
    }
}
