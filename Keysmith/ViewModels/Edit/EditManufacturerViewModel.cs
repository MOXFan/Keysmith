using Keysmith.Models;

namespace Keysmith.ViewModels
{
    class EditManufacturerViewModel : EditViewModel<ManufacturerModel>
    {
        protected override string SelectCurrentItemRoute
        { get { return "//select/manufacturer"; } }
    }
}
