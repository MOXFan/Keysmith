using Keysmith.Models;

namespace Keysmith.ViewModels
{
    class EditKeywayViewModel : SingleSubmodelEditViewModel<KeywayModel,ManufacturerModel>
    {
        public override string SelectSubmodelRoute
        { get { return "//select/manufacturer"; } }

        protected override string SelectCurrentItemRoute
        { get { return "//select/keyway"; } }

        protected override void SetSubmodel(ManufacturerModel selection)
        { CurrentItem.ManufacturerID = selection.ID; }
    }
}
