using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.ModelsApi
{
    public class SaveSettingModelReq
    {
        [Required(ErrorMessage = "SettingId is required")]
        [Range(0, long.MaxValue, ErrorMessage = "SettingId must be greater than or equal to 0")]
        public long? SettingId { get; set; }

        [Required(ErrorMessage = "SettingTitle is required")]
        public string? SettingTitle { get; set; }

        [Required(ErrorMessage = "Scope is required")]
        public string Scope { get; set; }

        [Required(ErrorMessage = "IsActive is required")]
        public bool? IsActive { get; set; }
        public bool IsDelete { get; set; } = false;
    }
}
