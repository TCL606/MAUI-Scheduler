using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiApp1;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
        this.BindingContext = BindingContext as EventViewModel;
	}
}
