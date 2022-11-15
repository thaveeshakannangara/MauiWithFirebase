namespace MauiApp13;

public partial class BranchView : ContentPage
{
	public BranchView(BranchViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}