using Keysmith.Models;

namespace Keysmith.ViewModels
{
    class SelectKeyViewModel : DualModelFilteredSelectViewModel<KeyModel, KeywayModel, SiteModel>
    {
        protected override string CurrentRoute
        {
            get
            { return "//select/key"; }
        }
        protected override string SelectModelFilterRoute
        {
            get
            { return "//select/keyway"; }
        }
        protected override string SelectModelFilter2Route
        {
            get
            { return "//select/site"; }
        }
        protected override int GetFilteredID(KeyModel input)
        {
            return input.KeywayID;
        }
        protected override int GetFilteredID2(KeyModel input)
        {
            return input.SiteID;
        }
    }
}