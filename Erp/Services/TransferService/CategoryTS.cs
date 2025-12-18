using AppBO.ModelsInventory;
using System.ComponentModel;

namespace Erp.Services.TransferService
{
    public class CategoryTS : INotifyPropertyChanged
    {
        public event Action? OnChange;
        public event PropertyChangedEventHandler? PropertyChanged;

        // This method will be called to trigger the OnChange event

        private CategoryModel _Category = new CategoryModel();
        public CategoryModel Category
        {
            get => _Category;
            set
            {
                _Category = value;
                OnPropertyChanged(nameof(Category));
            }
        }

        private List<CategoryModel> _Categorys = new List<CategoryModel>();
        public List<CategoryModel> Categorys
        {
            get => _Categorys;
            set
            {
                _Categorys = value;
                OnPropertyChanged(nameof(Categorys));
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
