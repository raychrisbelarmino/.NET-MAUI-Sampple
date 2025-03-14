﻿namespace MauiSample;

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
}

