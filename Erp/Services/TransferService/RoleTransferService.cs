using AppBO.Models;
using AppBO.ModelsApi;
using System.ComponentModel;

namespace Erp.Services.TransferService
{
    public class RoleTransferService : INotifyPropertyChanged
    {
        public event Action? OnChange;
        public event PropertyChangedEventHandler? PropertyChanged;

        private RoleModel _Role = new RoleModel();
        public RoleModel Role
        {
            get => _Role;
            set
            {
                _Role = value;
                OnPropertyChanged(nameof(Role));
            }
        }

        private List<RoleModel> _Roles = new List<RoleModel>();
        public List<RoleModel> Roles
        {
            get => _Roles;
            set
            {
                _Roles = value;
                OnPropertyChanged(nameof(Roles));
            }
        }

        private List<UserRoleAllRes> _UserRolegAll = new List<UserRoleAllRes>();
        public List<UserRoleAllRes> UserRoleAll
        {
            get => _UserRolegAll;
            set
            {
                _UserRolegAll = value;
                OnPropertyChanged(nameof(UserRoleAll));
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