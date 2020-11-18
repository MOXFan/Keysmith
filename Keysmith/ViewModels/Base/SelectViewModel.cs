using Keysmith.Models;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Keysmith.ViewModels
{
    class SelectViewModel<TModel> : ListViewModel<TModel>
        where TModel : ModelBase, new()
    {
        #region Private Members
        private TModel _selectedItem = null;
        #endregion
        #region Constructors
        public SelectViewModel() : base()
        {
            SelectionChangedCommand = new Command(SelectItem);
        }
        #endregion
        #region Properties
        public TModel SelectedItem
        {
            get
            { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }
        public ICommand SelectionChangedCommand { get; private set; }
        #endregion
        #region Methods
        private void SelectItem(TModel selection)
        {
            if (SelectedItem != null)
            {
                ItemSelected?.Invoke(this, selection);
                SelectedItem = null;
                Shell.Current.Navigation.PopAsync();
            }
        }
        private void SelectItem()
        { SelectItem(SelectedItem); }
        #endregion
        #region Events
        public event EventHandler<TModel> ItemSelected;
        #endregion
    }
}
