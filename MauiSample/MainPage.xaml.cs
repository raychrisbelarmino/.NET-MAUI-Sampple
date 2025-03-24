namespace MauiSample;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		NavigationPage.SetHasNavigationBar(this, false);
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new NavigationPage(new AppShell()));
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
		Navigation.PushModalAsync(new Page2());
    }

    private void SetPreference_Clicked(object sender, EventArgs e)
    {
        // Set a string, number or boolean value:
        Preferences.Default.Set("fname", "Chiz");
    }
    
    private void GetPreference_Clicked(object? sender, EventArgs e)
    {
        string firstName = Preferences.Default.Get("fname", "Default value if preference does not exist");
        Console.WriteLine("Value of first name: " + firstName);
    }

    private void Animate_Clicked(object sender, EventArgs e)
    {
        //Animation
        logoImage.RotateTo(90, 5000);
        logoImage.ScaleTo(0.5, 5000);
        logoImage.TranslateTo(20, 20, 5000);
        logoImage.FadeTo(0.5, 5000);
    }
}

