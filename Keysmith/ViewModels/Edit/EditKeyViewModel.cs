using Keysmith.Models;

namespace Keysmith.ViewModels
{
    class EditKeyViewModel : DualSubmodelEditViewModel<KeyModel, KeywayModel, SiteModel>
    {
        protected override string SelectCurrentItemRoute
        { get { return "//select/key"; } }
        public override string SelectSubmodelRoute
        { get { return "//select/keyway"; } }
        public override string SelectSubmodel2Route
        { get { return "//select/site"; } }
        protected override void SetSubmodel(KeywayModel selection)
        { CurrentItem.KeywayID = selection.ID; }
        protected override void SetSubmodel2(SiteModel selection)
        { CurrentItem.SiteID = selection.ID; }

        public override void NewCurrentItem()
        {
            KeyModel newKey = new KeyModel
            {
                Keyway = CurrentItem.Keyway,
                Site = CurrentItem.Site
            };

            CurrentItem = newKey;
        }
    }
}
