using Keysmith.Models;
using Keysmith.PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Keysmith.ViewModels
{
    class ListViewModel<TModel> : PropertyChangedBase where TModel : ModelBase, new()
    {
        #region Private Members
        private bool _isRefreshingItems = false;
        private string _searchFilter = "";
        #endregion
        #region Constructors
        public ListViewModel()
        {
            RefreshItemsCommand = new Command(RefreshItemsAsync);
            Repo.PropertyChanged += Repo_ItemsPropertyChanged;
        }
        #endregion
        #region Properties
        protected Repository<TModel> Repo { get; set; } = Repository<TModel>.Instance;
        protected Repository<CustomerModel> CustomerRepo { get; set; } = Repository<CustomerModel>.Instance;
        protected Repository<SiteModel> SiteRepo { get; set; } = Repository<SiteModel>.Instance;
        protected Repository<ManufacturerModel> ManufacturerRepo { get; set; } = Repository<ManufacturerModel>.Instance;
        protected Repository<KeywayModel> KeywayRepo { get; set; } = Repository<KeywayModel>.Instance;
        protected virtual string CurrentRoute
        {
            get
            {
                var flyout = Shell.Current.CurrentItem;
                var tab = flyout.CurrentItem;
                var page = tab.CurrentItem;
                if (page.Title == null)
                { return $"//{flyout.Route}/{tab.Route}"; }
                else
                { return $"//{flyout.Route}/{tab.Route}/{page.Route}"; }
            }
        }
        public List<TModel> Items { get { return Repo.Items; } }
        public string SearchFilter
        {
            get
            { return _searchFilter; }
            set
            {
                _searchFilter = value;
                OnPropertyChanged();
                OnPropertyChanged("FilteredItems");
            }
        }
        public List<TModel> FilteredItems
        {
            get
            { return FilterItems(); }
        }
        public ICommand RefreshItemsCommand { get; private set; }
        public bool IsRefreshingItems
        {
            get { return _isRefreshingItems; }
            set
            {
                _isRefreshingItems = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Methods
        #region Navigation Methods
        protected async void OpenSelectPageAsync<TMethodModel>(string destinationRoute, EventHandler<TMethodModel> selectionEventHandler)
            where TMethodModel : ModelBase, new()
        {
            List<Page> savedNavStack = SaveNavStack();
            Page selectionPage = await NavigateToRoute(destinationRoute);
            RegisterSelectionHandler(selectionPage, selectionEventHandler);
            RestoreNavigationStack(savedNavStack);
        }
        protected List<Page> SaveNavStack()
        {
            IReadOnlyList<Page> navStack = GetNavStack();
            List<Page> output = new List<Page>();

            for (int index = 1; index < navStack.Count; index++)
            { output.Add(navStack[index]); }

            return output;
        }
        private void RestoreNavigationStack(IReadOnlyList<Page> previousNavStack)
        {
            Page topPage = GetTopNavStackPage();

            foreach (Page currentPage in previousNavStack)
            { Shell.Current.Navigation.InsertPageBefore(currentPage, topPage); }
        }
        protected async Task<Page> NavigateToRoute(string newRoute)
        {
            await Shell.Current.GoToAsync(newRoute);
            return GetTopNavStackPage();
        }
        protected Page GetTopNavStackPage()
        {
            IReadOnlyList<Page> navStack = GetNavStack();
            int lastStackIndex = navStack.Count - 1;
            return navStack[lastStackIndex];
        }
        protected IReadOnlyList<Page> GetNavStack()
        { return Shell.Current.Navigation.NavigationStack; }
        protected void RegisterSelectionHandler<TMethodModel>(Page selectionPage, EventHandler<TMethodModel> selectionHandler)
        where TMethodModel : ModelBase, new()
        {
            SelectViewModel<TMethodModel> selectViewModel = selectionPage.BindingContext as SelectViewModel<TMethodModel>;
            selectViewModel.ItemSelected += selectionHandler;
        }
        #endregion
        protected List<TModel> FilterItems()
        {
            if (IsFilterApplied())
            { return Repo.Items.FindAll(FilterPredicate); }
            else
            { return Repo.Items; }
        }
        protected virtual bool FilterPredicate(TModel input)
        { return input.MatchFilter(SearchFilter); }
        protected virtual bool IsFilterApplied()
        {
            if (string.IsNullOrEmpty(SearchFilter))
            { return false; }
            else
            { return true; }
        }
        protected virtual async void RefreshItemsAsync()
        { await Repo.RefreshItemsAsync(); }
        private void Repo_ItemsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Items")
            { 
                OnPropertyChanged("Items");
                OnPropertyChanged("FilteredItems");
                IsRefreshingItems = false;
            }
        }
        #endregion
    }
}
