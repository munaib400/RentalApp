using StarterApp.ViewModels;

namespace StarterApp.Views;

public partial class RentalListPage : ContentPage
{
    private readonly RentalListViewModel _viewModel;

    public RentalListPage(RentalListViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadRentalsCommand.Execute(null);
    }
}
