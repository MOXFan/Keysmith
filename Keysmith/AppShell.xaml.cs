
using Keysmith.Views;
using Xamarin.Forms;

namespace Keysmith
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute("//select/customer", typeof(SelectCustomerPage));
            Routing.RegisterRoute("//select/manufacturer", typeof(SelectManufacturerPage));
            Routing.RegisterRoute("//select/keyway", typeof(SelectKeywayPage));
            Routing.RegisterRoute("//select/site", typeof(SelectSitePage));
            Routing.RegisterRoute("//select/key", typeof(SelectKeyPage));
        }
    }
}