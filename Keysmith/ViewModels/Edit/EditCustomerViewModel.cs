using Keysmith.Models;

namespace Keysmith.ViewModels
{
    class EditCustomerViewModel : EditViewModel<CustomerModel>
    {
        protected override string SelectCurrentItemRoute
        { get { return "//select/customer"; } }
    }
}
