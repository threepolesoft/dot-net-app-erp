using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.ModelsApi
{
    public class SaveUsersDeviceModelReq
    {
        [Required(ErrorMessage = "User Name is required")]
        [Range(1, long.MaxValue, ErrorMessage = "User Id must be greater than or equal to 1")]
        public long UserId { get; set; }


        [Required(ErrorMessage = "Device Id is required")]
        public string DeviceId { get; set; }


        [Required(ErrorMessage = "Device Name is required")]
        public string DeviceName { get; set; }


        [Required(ErrorMessage = "Scope is required")]
        public string Scope { get; set; }
    }
}
