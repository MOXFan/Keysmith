using Keysmith.Models;
using System.Windows.Input;
using Xamarin.Forms;

namespace Keysmith.ViewModels
{
    abstract class DualSubmodelEditViewModel<TModel, TSub, TSub2> : SingleSubmodelEditViewModel<TModel, TSub>
        where TModel : ModelBase, new()
        where TSub : ModelBase, new()
        where TSub2 : ModelBase, new()
    {
        #region Constructors
        public DualSubmodelEditViewModel() : base()
        {
            SelectSubmodel2Command = new Command(SelectSubmodel2Async);
            ClearSubmodel2Command = new Command(ClearSubmodel2);
        }
        #endregion
        #region Properties
        public ICommand SelectSubmodel2Command { get; private set; }
        public ICommand ClearSubmodel2Command { get; private set; }
        public abstract string SelectSubmodel2Route { get; }
        #endregion
        #region Methods
        private void ClearSubmodel2()
        { SetSubmodel2(new TSub2()); }
        private async void SelectSubmodel2Async()
        {
            Page selectionPage = await NavigateToRoute(SelectSubmodel2Route);
            RegisterSelectionHandler<TSub2>(selectionPage, SelectSubmodel2ViewModel_ItemSelected);
        }
        private void SelectSubmodel2ViewModel_ItemSelected(object sender, TSub2 selection)
        { SetSubmodel2(selection); }
        protected abstract void SetSubmodel2(TSub2 selection);
        #endregion
    }
}
