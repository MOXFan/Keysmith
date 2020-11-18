using Keysmith.Models;

namespace Keysmith.ViewModels
{
    class SelectKeywayViewModel : ModelFilteredSelectViewModel<KeywayModel, ManufacturerModel>
    {
        protected override string CurrentRoute
        {
            get
            { return "//select/keyway"; }
        }
        protected override string SelectModelFilterRoute
        {
            get
            { return "//select/manufacturer"; }
        }
        protected override int GetFilteredID(KeywayModel input)
        {
            return input.ManufacturerID;
        }
    }
}
