using Keysmith.Models;
using System.Windows.Input;
using Xamarin.Forms;

namespace Keysmith.ViewModels
{
    abstract class ModelFilteredSelectViewModel <TModel, TFilter> : SelectViewModel<TModel>
        where TModel: ModelBase, new()
        where TFilter: ModelBase, new()
    {
        #region Private Members
        private TFilter _modelFilter = new TFilter();
        #endregion
        #region Constructors
        public ModelFilteredSelectViewModel() : base()
        {
            SelectModelFilterCommand = new Command(SelectModelFilter);
            ClearModelFilterCommand = new Command(ClearModelFilter);
        }
        #endregion
        #region Properties
        protected override abstract string CurrentRoute { get; }
        protected abstract string SelectModelFilterRoute { get; }
        public ICommand SelectModelFilterCommand { get; private set; }
        public ICommand ClearModelFilterCommand { get; private set; }
        public TFilter ModelFilter
        {
            get
            { return _modelFilter; }
            set
            {
                _modelFilter = value;
                OnPropertyChanged();
                OnPropertyChanged("FilteredItems");
            }
        }
        #endregion
        #region Methods
        private void ClearModelFilter()
        { ModelFilter = new TFilter(); }
        private void SelectModelFilter()
        { OpenSelectPageAsync<TFilter>(SelectModelFilterRoute, SelectModelFilterViewModel_InstanceItemSelected); }
        private void SelectModelFilterViewModel_InstanceItemSelected(object sender, TFilter selection)
        { ModelFilter = selection; }
        protected override bool FilterPredicate(TModel input)
        {
            if (base.FilterPredicate(input) == false)
            { return false; }
            else if (ModelFilter.ID <= 0 || GetFilteredID(input) == ModelFilter.ID)
            { return true; }
            else
            { return false; }
        }
        protected override bool IsFilterApplied()
        {
            if (base.IsFilterApplied())
            { return true; }
            else if (ModelFilter?.ID > 0)
            { return true; }
            else
            { return false; }
        }
        protected abstract int GetFilteredID(TModel input);
        #endregion
    }
}
