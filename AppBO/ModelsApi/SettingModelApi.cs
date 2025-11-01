using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.ModelsApi
{
    internal class SettingModelApi
    {
    }

    public class SaveUserSettingReq
    {
        [Required(ErrorMessage = "UserId is required")]
        [Range(1, long.MaxValue, ErrorMessage = "UserId must be greater than or equal to 1")]
        public long? UserId { get; set; }

        [Required(ErrorMessage = "SettingId is required")]
        [Range(1, long.MaxValue, ErrorMessage = "SettingId must be greater than or equal to 1")]
        public long? SettingId { get; set; }   
        
        [Required(ErrorMessage = "Value is required")]
        public string? Value { get; set; }
    }
    
    public class SaveDeviceSettingReq
    {
        [Required(ErrorMessage = "UserId is required")]
        [Range(1, long.MaxValue, ErrorMessage = "UserId must be greater than or equal to 1")]
        public long? UserId { get; set; }

        [Required(ErrorMessage = "DeviceId is required")]
        public string? DeviceId { get; set; }

        [Required(ErrorMessage = "SettingId is required")]
        [Range(1, long.MaxValue, ErrorMessage = "SettingId must be greater than or equal to 1")]
        public long? SettingId { get; set; }   
        
        [Required(ErrorMessage = "Value is required")]
        public string? Value { get; set; }
    }

    public class UpdateDeviceSettingReq
    {
        [Required(ErrorMessage = "DeviceId is required")]
        public string? DeviceId { get; set; }

        [Required(ErrorMessage = "SettingName is required")]
        public string? SettingName { get; set; }

        [Required(ErrorMessage = "Scope is required")]
        public string? Scope { get; set; }

        [Required(ErrorMessage = "Value is required")]
        public string? Value { get; set; }
    }

    public class DeviceSettingAllRes
    {
        public UserInfo UserInfo { get; set; }
        public List<Setting> UserSettings { get; set; } = new List<Setting>();
        public List<DeviceSetting> DeviceSettings { get; set; } = new List<DeviceSetting>();
    }

    public class UserSettingAllRes
    {
        public long ApplicationUserId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public List<Setting> Settings { get; set; } = new List<Setting>();
    }


    public class DeviceSetting
    {
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public List<Setting> Settings { get; set; }
    }

    public class Setting
    {
        public int SettingId { get; set; }
        public string SettingName { get; set; }
        public string SettingTitle { get; set; }
        public string Value { get; set; }
    }

    public class UserInfo
    {
        public string LawyerName { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
    }
}
