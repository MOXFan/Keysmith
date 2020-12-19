using Keysmith.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Keysmith.ViewModels
{
    class StandardPinningViewModel : ListViewModel<KeyModel>
    {
        #region Private Members
        private PinningModel _pinning = new PinningModel();
        private ObservableCollection<KeyModel> _selectedKeys = new ObservableCollection<KeyModel>();
        private bool _isRefreshingCurrentKeys = false;
        #endregion
        #region Constructors
        public StandardPinningViewModel()
        {
            AddKeyCommand = new Command(OpenAddKeySelector);
            ClearKeysCommand = new Command(ClearKeys);
            RefreshSelectedKeysCommand = new Command(RefreshSelectedKeysAsync);
        }
        #endregion
        #region Properties
        protected string SelectAddKeyRoute { get { return "//select/key"; } }
        public PinningModel Pinning
        {
            get { return _pinning; }
            set
            {
                _pinning = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<KeyModel> SelectedKeys
        {
            get { return _selectedKeys; }
            set
            {
                _selectedKeys = value;
                OnPropertyChanged();
            }
        }
        public ICommand AddKeyCommand { get; private set; }
        public ICommand ClearKeysCommand { get; private set; }
        public ICommand RefreshSelectedKeysCommand { get; private set; }
        public bool IsRefreshingSelectedKeys
        {
            get { return _isRefreshingCurrentKeys; }
            set
            {
                _isRefreshingCurrentKeys = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Methods
        protected void AddSelectedKey(KeyModel selection)
        {
            if (SelectedKeys.Contains(selection) == false)
            {
                SelectedKeys.Add(selection);
                OnPropertyChanged("SelectedKeys");
                UpdatePinning();
            }
        }
        protected void UpdatePinning()
        { Pinning = new PinningModel(SelectedKeys); }
        public void OpenAddKeySelector()
        { OpenSelectPageAsync<KeyModel>(SelectAddKeyRoute,AddKeySelectorViewModel_ItemSelected); }
        public void ClearKeys()
        {
            SelectedKeys = new ObservableCollection<KeyModel>();
            UpdatePinning();
        }
        public async void RefreshSelectedKeysAsync()
        {
            ObservableCollection<KeyModel> buffer = new ObservableCollection<KeyModel>();
            for (int index = 0; index < SelectedKeys.Count; index++)
            {
                buffer.Add(await Repo.GetItemAsync(SelectedKeys[index].ID));
            }
            SelectedKeys = buffer;
            IsRefreshingSelectedKeys = false;
            UpdatePinning();
        }
        protected void AddKeySelectorViewModel_ItemSelected(object sender, KeyModel selection)
        { AddSelectedKey(selection); }
        #endregion
    }
}
