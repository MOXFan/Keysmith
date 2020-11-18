using Keysmith.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Keysmith.TaskExtensions;

namespace Keysmith.Models
{
    class Repository<TModel> : INotifyPropertyChanged
        where TModel : ModelBase, new()
    {
        #region Private Members
        private static readonly Lazy<Repository<TModel>> _lazyInstance = new Lazy<Repository<TModel>>(() => new Repository<TModel>());
        private List<TModel> _items = new List<TModel>();
        #endregion
        #region Constructors
        public Repository() 
        {
            RefreshItemsAsync().FireAndForget();
            DB.PropertyChanged += DB_PropertyChanged;
        }
        #endregion
        #region Properties
        public static Repository<TModel> Instance => _lazyInstance.Value;
        private static SQLiteDatabase DB { get; set; } = SQLiteDatabase.Instance;
        public List<TModel> Items 
        { 
            get
            { return _items; }
            private set
            {
                _items = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Methods
        public async Task RefreshItemsAsync()
        { Items = await DB.GetItemsAsync<TModel>(); }
        private void DB_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == typeof(TModel).ToString())
            { RefreshItemsAsync().FireAndForget(); }
        }
        public async Task<TModel> GetItemAsync(int id)
        { return await DB.GetItemAsync<TModel>(id); }
        public async Task<TModel> GetItemAsync(TModel item)
        { return await GetItemAsync(item.ID); }
        public async Task<int> SaveItemAsync(TModel item)
        { return await DB.SaveItemAsync<TModel>(item); }
        public async Task<bool> DeleteItemAsync(TModel item)
        { return await DB.DeleteItemAsync<TModel>(item); }
        public bool IsValidID(int id)
        {
            if (id > 0)
            { return true; }
            else
            { return false; }
        }
        #endregion
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }
}
