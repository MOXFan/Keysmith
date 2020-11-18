using Keysmith.Models;

namespace Keysmith.ViewModels
{
    class SelectSiteViewModel : ModelFilteredSelectViewModel<SiteModel, CustomerModel>
    {
        protected override string CurrentRoute
        {
            get
            { return "//select/site"; }
        }
        protected override string SelectModelFilterRoute
        {
            get
            { return "//select/customer"; }
        }
        protected override int GetFilteredID(SiteModel input)
        {
            return input.CustomerID;
        }
    }
}
