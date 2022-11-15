namespace MauiApp13;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}