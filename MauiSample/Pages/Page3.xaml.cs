using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Maui.Controls.PlatformConfiguration;

namespace MauiSample;

public partial class Page3 : ContentPage
{
    NetworkHelper networkHelper;
    HttpClient client;
    CancellationToken cts;
    CancellationTokenSource cs = new CancellationTokenSource();
    HttpResponseMessage response;
    ObservableCollection<PostsModel> posts = new ObservableCollection<PostsModel>();
    public Page3()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        networkHelper = new NetworkHelper();
        client = new HttpClient();
        cts = cs.Token;
        client.DefaultRequestHeaders.Accept.Clear();
        client.MaxResponseContentBufferSize = 256000;
        client.Timeout = TimeSpan.FromMinutes(1);
        postLV.ItemsSource = posts;
    }

    async protected override void OnAppearing()
    {
        base.OnAppearing();
        activityIndicator.IsRunning = true;
        //GET 
        if (networkHelper.HasInternet())
        {
            if (await networkHelper.IsHostReachable() == true)
            {
                var uri = new Uri(Constants.URL + Constants.POSTS);
                response = await client.GetAsync(uri, cts);
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    JObject jObject = new JObject();
                    try
                    {
                        jObject = JObject.Parse(result);
                        ReceivedResult(jObject);
                    }
                    catch (Exception e)
                    {
                        JArray jA = JArray.Parse(result);
                        jObject = JObject.Parse("{\"count\":"+jA.Count+",\"data\":" + JsonConvert.SerializeObject(jA) + "}");
                        ReceivedResult(jObject);
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

    private void ReceivedResult(JObject jsonData)
    {
        posts.Clear();
        for (int x = 0; x < Convert.ToInt32(jsonData["count"]); x++)
        {
            PostsModel i = JsonConvert.DeserializeObject<PostsModel>(jsonData["data"][x].ToString());
            posts.Add(i);
        }
        activityIndicator.IsRunning = false;
    }

    private void PostLV_OnItemSelected(object? sender, SelectedItemChangedEventArgs e)
    {
        ((ListView)sender).SelectedItem = null; //remove the highlight
    }

    async private void PostLV_OnItemTapped(object? sender, ItemTappedEventArgs e)
    {
        PostsModel item = (e.Item) as PostsModel;
        await Navigation.PushAsync(new Page4(item, posts));
    }
}