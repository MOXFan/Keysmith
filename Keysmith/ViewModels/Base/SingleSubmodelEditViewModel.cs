using Keysmith.Models;
using System.Windows.Input;
using Xamarin.Forms;

namespace Keysmith.ViewModels
{
    abstract class SingleSubmodelEditViewModel<TModel, TSub> : EditViewModel<TModel>
        where TModel : ModelBase, new()
        where TSub : ModelBase, new()
    {
        #region Constructors
        public SingleSubmodelEditViewModel() : base()
        {
            SelectSubmodelCommand = new Command(SelectSubmodel);
            ClearSubmodelCommand = new Command(ClearSubmodel);
        }
        #endregion
        #region Properties
        public ICommand SelectSubmodelCommand { get; private set; }
        public ICommand ClearSubmodelCommand { get; private set; }
        public abstract string SelectSubmodelRoute { get; }
        #endregion
        #region Methods
        private void ClearSubmodel()
        { SetSubmodel(new TSub()); }
        private void SelectSubmodel()
        { OpenSelectPageAsync<TSub>(SelectSubmodelRoute, SelectSubmodelViewModel_ItemSelected); }
        private void SelectSubmodelViewModel_ItemSelected(object sender, TSub selection)
        { SetSubmodel(selection); }
        protected abstract void SetSubmodel(TSub selection);
        #endregion
    }
}
