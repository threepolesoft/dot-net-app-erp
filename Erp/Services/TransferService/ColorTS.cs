using AppBO.ModelsInventory;
using System.ComponentModel;

namespace Erp.Services.TransferService
{
    public class ColorTS : INotifyPropertyChanged
    {
        public event Action? OnChange;
        public event PropertyChangedEventHandler? PropertyChanged;

        // This method will be called to trigger the OnChange event

        private ColorModel _Color = new ColorModel();
        public ColorModel Color
        {
            get => _Color;
            set
            {
                _Color = value;
                OnPropertyChanged(nameof(Color));
            }
        }

        private List<ColorModel> _Colors = new List<ColorModel>();
        public List<ColorModel> Colors
        {
            get => _Colors;
            set
            {
                _Colors = value;
                OnPropertyChanged(nameof(Colors));
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
