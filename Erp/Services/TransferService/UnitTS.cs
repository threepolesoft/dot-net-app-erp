using AppBO.ModelsInv;
using System.ComponentModel;

namespace Erp.Services.TransferService
{
    public class UnitTS : INotifyPropertyChanged
    {
        public event Action? OnChange;
        public event PropertyChangedEventHandler? PropertyChanged;


        private UnitModel _Unit = new UnitModel();
        public UnitModel Unit
        {
            get => _Unit;
            set
            {
                _Unit = value;
                OnPropertyChanged(nameof(Unit));
            }
        }

        private List<UnitModel> _Units = new List<UnitModel>();
        public List<UnitModel> Units
        {
            get => _Units;
            set
            {
                _Units = value;
                OnPropertyChanged(nameof(Units));
            }
        }

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
