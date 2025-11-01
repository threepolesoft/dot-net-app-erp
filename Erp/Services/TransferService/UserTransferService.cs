using AppBO.Models;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace Erp.Services.TransferService
{
    public class UserTransferService : INotifyPropertyChanged
    {
        public event Action? OnChange;
        public event PropertyChangedEventHandler? PropertyChanged;

        private string _DeviceId = string.Empty;
        public string DeviceId
        {
            get => _DeviceId;
            set
            {
                _DeviceId = value;
                OnPropertyChanged(nameof(DeviceId));
            }
        }

        private ConcurrentDictionary<string, List<string>> _ActiveUser = new ConcurrentDictionary<string, List<string>>();
        public ConcurrentDictionary<string, List<string>> ActiveUser
        {
            get => _ActiveUser;
            set
            {
                _ActiveUser = value;
                OnPropertyChanged(nameof(ActiveUser));
            }
        }

        private ApplicationUserModel _User = new ApplicationUserModel();
        public ApplicationUserModel User
        {
            get => _User;
            set
            {
                _User = value;
                OnPropertyChanged(nameof(User));
            }
        }

        private List<ApplicationUserModel> _Users = new List<ApplicationUserModel>();
        public List<ApplicationUserModel> Users
        {
            get => _Users;
            set
            {
                _Users = value;
                OnPropertyChanged(nameof(Users));
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