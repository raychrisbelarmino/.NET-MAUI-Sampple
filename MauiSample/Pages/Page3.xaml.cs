using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using Xamarin.KotlinX.Coroutines;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MauiSample;

public partial class Page3 : ContentPage
{
    NetworkHelper networkHelper = NetworkHelper.GetInstance;
    HttpClient client;
    CancellationTokenSource cts;
    ObservableCollection<PostsModel> posts = new ObservableCollection<PostsModel>(); 
    public Page3()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.MaxResponseContentBufferSize = 256000;
        client.Timeout = TimeSpan.FromMinutes(1);
        
        posts.Add(new PostsModel() { userId = 1, id = 0, title = "NVIDIA", body = "RTX 5090" });
        posts.Add(new PostsModel() { userId = 1, id = 1, title = "NVIDIA", body = "GTX 1080 Ti" });
        posts.Add(new PostsModel() { userId = 1, id = 2, title = "NVIDIA", body = "GTX 1050 Ti" });
        postLV.ItemsSource = posts;
    }

    async protected override void OnAppearing()
    {
        base.OnAppearing();

        //GET 
        if (networkHelper.HasInternet())
        {
            if (await networkHelper.IsHostReachable() == true)
            {
                var uri = new Uri(Constants.URL + Constants.POSTS);
                HttpResponseMessage response = await client.GetAsync(uri, cts.Token);
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    //Console.WriteLine("Response: " + result);

                    JObject jObject = new JObject();
                    try{
                        jObject = JObject.Parse(result);
                        Console.WriteLine("JObject:  "+jObject);
                    }catch (Exception e){
                        JArray jA = JArray.Parse(result);
                        jObject = JObject.Parse("{\"data\":" + JsonConvert.SerializeObject(jA) + "}");

                        Console.WriteLine("JObject if JArray:  "+jObject);
                    }    
                }
                else
                {
                    await DisplayAlert("Error!", response.StatusCode.ToString(), "ok");
                }
            }
            else
            {
                await DisplayAlert("Host Unreachable!", "The URL host for <Your App> cannot be reached and seems to be unavailable. Please try again later!", "ok");
            }
        }
        else
        {
            await DisplayAlert("No Internet Connection!", "Please check your internet connection, and try again!", "ok");
        }
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
}