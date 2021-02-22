using System.ComponentModel;
using System.Threading.Tasks;

namespace Keysmith.Models
{
    interface IRepository<TModel> : INotifyPropertyChanged
        where TModel : ModelBase, new()
    {
        Task RefreshItemsAsync();
        Task<TModel> GetItemAsync(int id);
        Task<int> SaveItemAsync(TModel item);
        Task<bool> DeleteItemAsync(TModel item);
        bool IsValidID(int id);        
    }
}
