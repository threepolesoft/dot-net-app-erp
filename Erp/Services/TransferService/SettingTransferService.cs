using AppBO.Models;
using AppBO.ModelsApi;
using System.ComponentModel;

namespace Erp.Services.TransferService
{
    public class SettingTransferService : INotifyPropertyChanged
    {
        public event Action? OnChange;
        public event PropertyChangedEventHandler? PropertyChanged;

        private SettingModel _Setting = new SettingModel();
        public SettingModel Setting
        {
            get => _Setting;
            set
            {
                _Setting = value;
                OnPropertyChanged(nameof(Setting));
            }
        }

        private List<SettingModel> _Settings = new List<SettingModel>();
        public List<SettingModel> Settings
        {
            get => _Settings;
            set
            {
                _Settings = value;
                OnPropertyChanged(nameof(Settings));
            }
        }

        private List<UserSettingAllRes> _UserSettingAll = new List<UserSettingAllRes>();
        public List<UserSettingAllRes> UserSettingAll
        {
            get => _UserSettingAll;
            set
            {
                _UserSettingAll = value;
                OnPropertyChanged(nameof(UserSettingAll));
            }
        }

        private DeviceSettingAllRes _DeviceSettingAll = new DeviceSettingAllRes();
        public DeviceSettingAllRes DeviceSettingAll
        {
            get => _DeviceSettingAll;
            set
            {
                _DeviceSettingAll = value;
                OnPropertyChanged(nameof(DeviceSettingAll));
            }
        }

        // This method will be called to trigger the OnChange event
        public void NotifyStateChanged()
        {
            OnChange?.Invoke();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}