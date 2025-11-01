using AppBO.Models;
using System.ComponentModel;

namespace Erp.Services.TransferService
{
    public class MenuLeftTransferService : INotifyPropertyChanged
    {
        public event Action? OnChange;
        public event PropertyChangedEventHandler? PropertyChanged;

        private MenuLeftModel _Menu = new MenuLeftModel();
        public MenuLeftModel Menu
        {
            get => _Menu;
            set
            {
                _Menu = value;
                OnPropertyChanged(nameof(Menu));
            }
        }

        private List<MenuLeftModel> _Menus = new List<MenuLeftModel>();
        public List<MenuLeftModel> Menus
        {
            get => _Menus;
            set
            {
                _Menus = value;
                OnPropertyChanged(nameof(Menus));
            }
        }

        private MenuElement _Selected = new MenuElement(new MenuLeftModel());
        public MenuElement Selected
        {
            get => _Selected;
            set
            {
                _Selected = value;
                OnPropertyChanged(nameof(Selected));
            }
        }

        private List<MenuElement> menuElements = new List<MenuElement>();
        public List<MenuElement> MenuElements
        {
            get => menuElements;
            set
            {
                menuElements = value;
                OnPropertyChanged(nameof(MenuElements));
            }
        }

        private List<MenuLeftModel> _UserMenu = new List<MenuLeftModel>();
        public List<MenuLeftModel> UserMenu
        {
            get => _UserMenu;
            set
            {
                _UserMenu = value;
                OnPropertyChanged(nameof(UserMenu));
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
