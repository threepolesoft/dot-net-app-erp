using AppBO.Models;
using AppBO.ModelsInv;
using System.ComponentModel;

namespace Erp.Services.TransferService
{
    public class BrandTransferServic : INotifyPropertyChanged
    {
        public event Action? OnChange;
        public event PropertyChangedEventHandler? PropertyChanged;

        // This method will be called to trigger the OnChange event

        private BrandModel _Brand = new BrandModel();
        public BrandModel Brand
        {
            get => _Brand;
            set
            {
                _Brand = value;
                OnPropertyChanged(nameof(Brand));
            }
        }

        private List<BrandModel> _Brands = new List<BrandModel>();
        public List<BrandModel> Brands
        {
            get => _Brands;
            set
            {
                _Brands = value;
                OnPropertyChanged(nameof(Brands));
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
