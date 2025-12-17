using AppBO.ModelsInv;
using System.ComponentModel;

namespace Erp.Services.TransferService
{
    public class SizeTS : INotifyPropertyChanged
    {
        public event Action? OnChange;
        public event PropertyChangedEventHandler? PropertyChanged;


        private SizeModel _Size = new SizeModel();
        public SizeModel Size
        {
            get => _Size;
            set
            {
                _Size = value;
                OnPropertyChanged(nameof(Size));
            }
        }

        private List<SizeModel> _Sizes = new List<SizeModel>();
        public List<SizeModel> Sizes
        {
            get => _Sizes;
            set
            {
                _Sizes = value;
                OnPropertyChanged(nameof(Sizes));
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
