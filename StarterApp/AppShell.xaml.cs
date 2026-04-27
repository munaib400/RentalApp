using StarterApp.ViewModels;
using StarterApp.Views;

namespace StarterApp;

public partial class AppShell : Shell
{
    public AppShell(AppShellViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();

        // Register routes
        Routing.RegisterRoute("ItemListPage", typeof(ItemListPage));
        Routing.RegisterRoute("CreateItemPage", typeof(CreateItemPage));
        Routing.RegisterRoute("RentalListPage", typeof(RentalListPage));
        Routing.RegisterRoute("RequestRentalPage", typeof(RequestRentalPage));
        Routing.RegisterRoute("ItemDetailPage", typeof(ItemDetailPage));
    }
}