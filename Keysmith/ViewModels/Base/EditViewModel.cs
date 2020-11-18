using Keysmith.Models;
using System.Windows.Input;
using Xamarin.Forms;

namespace Keysmith.ViewModels
{
    abstract class EditViewModel<TModel> : ListViewModel<TModel> where TModel : ModelBase, new()
    {
        #region Private Members
        private TModel _currentItem = new TModel();
        private bool _isRefreshingCurrentItem = false;
        #endregion
        #region Constructors
        public EditViewModel() : base()
        {
            RefreshCurrentItemCommand = new Command(RefreshCurrentItem);
            NewCurrentItemCommand = new Command(NewCurrentItem);
            SaveCurrentItemCommand = new Command(SaveCurrentItemAsync);
            DeleteCurrentItemCommand = new Command(DeleteCurrentItemAsync);
            SelectCurrentItemCommand = new Command(SelectCurrentItem);
        }
        #endregion
        #region Properties
        public ICommand RefreshCurrentItemCommand { get; private set; }
        public ICommand NewCurrentItemCommand { get; private set; }
        public ICommand SaveCurrentItemCommand { get; private set; }
        public ICommand DeleteCurrentItemCommand { get; private set; }
        public ICommand SelectCurrentItemCommand { get; private set; }

        public bool IsRefreshingCurrentItem
        {
            get
            { return _isRefreshingCurrentItem; }
            set
            {
                _isRefreshingCurrentItem = value;
                OnPropertyChanged();
            }
        }
        public TModel CurrentItem
        {
            get
            { return _currentItem; }
            set
            {
                _currentItem = value;
                OnPropertyChanged();
            }
        }

        protected abstract string SelectCurrentItemRoute { get; }
        #endregion
        #region Methods
        public void RefreshCurrentItem()
        {
            LoadCurrentItemAsync(CurrentItem.ID);

            IsRefreshingCurrentItem = false;
        }
        public async void LoadCurrentItemAsync(int id)
        {
            if (Repo.IsValidID(id))
            {
                CurrentItem = await Repo.GetItemAsync(id);
            }
        }
        public virtual void NewCurrentItem()
        { CurrentItem = new TModel(); }
        public async void SaveCurrentItemAsync()
        {
            int id = await Repo.SaveItemAsync(CurrentItem);
            await Repo.RefreshItemsAsync();
            LoadCurrentItemAsync(id);
        }
        public async void DeleteCurrentItemAsync()
        {
            await Repo.DeleteItemAsync(CurrentItem);
            NewCurrentItem();
        }
        private void SelectCurrentItem()
        { OpenSelectPageAsync<TModel>(SelectCurrentItemRoute, SelectCurrentItemViewModel_ItemSelected); }
        private void SelectCurrentItemViewModel_ItemSelected(object sender, TModel selection)
        { LoadCurrentItemAsync(selection.ID); }
        #endregion
    }
}
