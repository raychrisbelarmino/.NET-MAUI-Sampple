using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MauiSample;

public partial class Page3 : ContentPage, IRestConnector
{
    CancellationTokenSource cts;
    RestServices webService = new RestServices();
    ObservableCollection<PostsModel> posts = new ObservableCollection<PostsModel>(); 
    public Page3()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

        webService.WebServiceDelegate = this;

        posts.Add(new PostsModel() { userId = 1, id = 0, title = "NVIDIA", body = "RTX 5090" });
        posts.Add(new PostsModel() { userId = 1, id = 1, title = "NVIDIA", body = "GTX 1080 Ti" });
        posts.Add(new PostsModel() { userId = 1, id = 2, title = "NVIDIA", body = "GTX 1050 Ti" });
        postLV.ItemsSource = posts;
    }

    async protected override void OnAppearing()
    {
        base.OnAppearing();

        cts = new CancellationTokenSource();
        try
        {
            string url = Constants.URL + Constants.POSTS;

            await webService.GetRequest(url, 0, cts.Token);
        }
        catch (OperationCanceledException oce)
        {
            await DisplayAlert("Error!!", oce.Message, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error!!!", ex.Message, "OK");
        }
        cts = null;
    }

    private void PostLV_OnItemSelected(object? sender, SelectedItemChangedEventArgs e)
    {
        ((ListView)sender).SelectedItem = null; //remove the highlight
    }

    private void PostLV_OnItemTapped(object? sender, ItemTappedEventArgs e)
    {
        PostsModel item = (e.Item) as PostsModel;
        Console.WriteLine("Item tapped: ", item);
        posts.Remove(posts.Where(i => i.id == item.id).Single());
    }

    async public void ReceiveJSONData(JObject jsonData, int serviceType, CancellationToken ct)
    {
        switch (serviceType)
        {
            case 0://GET LIST
                posts.Clear();
                posts = JsonConvert.DeserializeObject<ObservableCollection<PostsModel>>(jsonData["data"].ToString());
                await DisplayAlert("Success", "Data loaded successfully", "OK");
                break;
        }
       
    }

    async public void ReceiveTimeoutError(string title, string error, int serviceType)
    {
        await DisplayAlert(title, error, "OK");
    }
}