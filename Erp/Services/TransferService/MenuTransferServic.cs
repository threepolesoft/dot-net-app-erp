using AppBO.Models;
using AppBO.ModelsApi;
using System.ComponentModel;

namespace Erp.Services.TransferService
{
    public class MenuTransferService : INotifyPropertyChanged
    {
        public event Action? OnChange;
        public event PropertyChangedEventHandler? PropertyChanged;

        private MenuModel _Menu = new MenuModel();
        public MenuModel Menu
        {
            get => _Menu;
            set
            {
                _Menu = value;
                OnPropertyChanged(nameof(Menu));
            }
        }

        private List<MenuModel> _Menus = new List<MenuModel>();
        public List<MenuModel> Menus
        {
            get => _Menus;
            set
            {
                _Menus = value;
                OnPropertyChanged(nameof(Menus));
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