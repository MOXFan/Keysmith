using Keysmith.Models;
using System.Windows.Input;
using Xamarin.Forms;

namespace Keysmith.ViewModels
{
    abstract class DualModelFilteredSelectViewModel<TModel, TFilter, TFilter2> : ModelFilteredSelectViewModel<TModel, TFilter>
        where TModel : ModelBase, new()
        where TFilter : ModelBase, new()
        where TFilter2 : ModelBase, new()
    {
        #region Private Members
        private TFilter2 _modelFilter2 = new TFilter2();
        #endregion
        #region Constructors
        public DualModelFilteredSelectViewModel() : base()
        {
            SelectModelFilter2Command = new Command(SelectModelFilter2);
            ClearModelFilter2Command = new Command(ClearModelFilter2);
        }
        #endregion
        #region Properties
        protected abstract string SelectModelFilter2Route { get; }
        public ICommand SelectModelFilter2Command { get; private set; }
        public ICommand ClearModelFilter2Command { get; private set; }
        public TFilter2 ModelFilter2
        {
            get
            { return _modelFilter2; }
            set
            {
                _modelFilter2 = value;
                OnPropertyChanged();
                OnPropertyChanged("FilteredItems");
            }
        }
        #endregion
        #region Methods
        private void ClearModelFilter2()
        { ModelFilter2 = new TFilter2(); }
        private void SelectModelFilter2()
        { OpenSelectPageAsync<TFilter2>(SelectModelFilter2Route, SelectModelFilter2ViewModel_InstanceItemSelected); }
        private void SelectModelFilter2ViewModel_InstanceItemSelected(object sender, TFilter2 selection)
        { ModelFilter2 = selection; }
        protected override bool FilterPredicate(TModel input)
        {
            if (base.FilterPredicate(input) == false)
            { return false; }
            else if (ModelFilter2.ID <= 0 || GetFilteredID2(input) == ModelFilter2.ID)
            { return true; }
            else
            { return false; }
        }
        protected override bool IsFilterApplied()
        {
            if (base.IsFilterApplied())
            { return true; }
            else if (ModelFilter2?.ID > 0)
            { return true; }
            else
            { return false; }
        }
        protected abstract int GetFilteredID2(TModel input);
        #endregion
    }
}
