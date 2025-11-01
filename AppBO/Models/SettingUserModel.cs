using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.Models
{
    public class SettingUserModel: TableOption
    {
        public long SettingUserId { get; set; }
        public long UserId { get; set; }
        public long SettingId { get; set; }
        public string SettingTitle { get; set; }
        public string Value { get; set; }
    }
}
