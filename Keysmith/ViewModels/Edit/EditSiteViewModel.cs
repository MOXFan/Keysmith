using Keysmith.Models;

namespace Keysmith.ViewModels
{
    class EditSiteViewModel : SingleSubmodelEditViewModel<SiteModel,CustomerModel>
    {
        public override string SelectSubmodelRoute
        { get { return "//select/customer"; } }

        protected override string SelectCurrentItemRoute
        { get { return "//select/site"; } }

        protected override void SetSubmodel(CustomerModel selection)
        { CurrentItem.CustomerID = selection.ID; }
    }
}
