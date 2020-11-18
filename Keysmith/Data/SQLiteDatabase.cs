using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Keysmith.TaskExtensions;
using Keysmith.Models;

namespace Keysmith.Data
{
    public class SQLiteDatabase : INotifyPropertyChanged
    {
        #region Private Members
        static readonly Lazy<SQLiteAsyncConnection> _lazyConnection = new Lazy<SQLiteAsyncConnection>(() =>
        { return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags); });
        static readonly Lazy<SQLiteDatabase> _lazyInstance = new Lazy<SQLiteDatabase>(() =>
        { return new SQLiteDatabase(); });
        static bool initialized = false;
        #endregion
        #region Constructors
        public SQLiteDatabase()
        {
            InitializeAsync().FireAndForget(false);
            StaticDatabaseChanged += PassStaticDatabaseChangedToInstance;
        }
        #endregion
        #region Properties
        private static SQLiteAsyncConnection Connection => _lazyConnection.Value;
        public static SQLiteDatabase Instance => _lazyInstance.Value;
        #endregion
        #region Methods
        private async Task InitializeAsync()
        {
            if(!initialized)
            {
                Task KeyTableTask = InitializeTableAsync(typeof(KeyModel));
                Task KeywayTableTask = InitializeTableAsync(typeof(KeywayModel));
                Task ManufacturerTableTask = InitializeTableAsync(typeof(ManufacturerModel));
                Task SiteTableTask = InitializeTableAsync(typeof(SiteModel));
                Task CustomerTableTask = InitializeTableAsync(typeof(CustomerModel));

                await Task.WhenAll(KeyTableTask, KeywayTableTask, ManufacturerTableTask, SiteTableTask, CustomerTableTask);
                initialized = true;
            }
        }

        private async Task InitializeTableAsync(Type tableType)
        {
            if(!initialized)
            {
                if(!Connection.TableMappings.Any(currentMapping => currentMapping.MappedType.Name == tableType.Name))
                {
                    await Connection.CreateTablesAsync(CreateFlags.None, tableType).ConfigureAwait(false);
                }
            }
        }

        public Task<List<T>> GetItemsAsync<T>() where T : ModelBase, new() 
        {
            return Connection.Table<T>().ToListAsync();
        }

        public Task<T> GetItemAsync<T>(int selectedID) where T : ModelBase, new()
        {
            return Connection.Table<T>().Where(currentItem => currentItem.ID == selectedID).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync<T>(T item) where T: ModelBase, new()
        {
            if (item.ID != 0)
            { await Connection.UpdateAsync(item); }
            else
            { await Connection.InsertAsync(item); }

            OnStaticDatabaseChanged(typeof(T).ToString());

            return item.ID;
        }

        public async Task<bool> DeleteItemAsync<T>(T item) where T : ModelBase, new()
        { return await DeleteItemAsync<T>(item.ID); }
        public async Task<bool> DeleteItemAsync<T>(int id) where T: ModelBase, new()
        {
            int returnCode = await Connection.DeleteAsync<T>(id);

            if(returnCode == 1)
            {
                OnStaticDatabaseChanged(typeof(T).ToString());
                return true; 
            }
            else
            { return false; }
        }
        #endregion
        #region INotifyPropertyChanged Implementation
        private static event PropertyChangedEventHandler StaticDatabaseChanged;
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnStaticDatabaseChanged([CallerMemberName]string property = "")
        {
            StaticDatabaseChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        private void OnPropertyChanged([CallerMemberName]string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        private void PassStaticDatabaseChangedToInstance(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }
        #endregion
    }
}
