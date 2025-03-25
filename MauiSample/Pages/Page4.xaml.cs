using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Text;

namespace MauiSample;

public partial class Page4 : ContentPage
{
    ObservableCollection<PostsModel> posts = new ObservableCollection<PostsModel>();
    PostsModel selectedItem = new PostsModel();
    NetworkHelper networkHelper;
    HttpClient client;
    CancellationToken cts;
    CancellationTokenSource cs = new CancellationTokenSource();
    HttpResponseMessage response;
    public Page4(PostsModel i, ObservableCollection<PostsModel> postsParam)
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        NavigationPage.SetHasBackButton(this, false);
        selectedItem = i;
        posts = postsParam;
        networkHelper = new NetworkHelper();
        client = new HttpClient();
        cts = cs.Token;
        client.DefaultRequestHeaders.Accept.Clear();
        client.MaxResponseContentBufferSize = 256000;
        client.Timeout = TimeSpan.FromMinutes(1);
    }

    async protected override void OnAppearing()
    {
        base.OnAppearing();

        //GET SINGLE POST
        if (networkHelper.HasInternet())
        {
            if (await networkHelper.IsHostReachable() == true)
            {
                var uri = new Uri(Constants.URL + Constants.POSTS + "/" + selectedItem.id);
                response = await client.GetAsync(uri, cts);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    JObject jObject = new JObject();
                    try
                    {
                        jObject = JObject.Parse(result);
                        ReceivedResult(jObject, 0);//0 - Get single post
                    }
                    catch (Exception e)
                    {
                        JArray jA = JArray.Parse(result);
                        jObject = JObject.Parse("{\"count\":" + jA.Count + ",\"data\":" + JsonConvert.SerializeObject(jA) + "}");
                        ReceivedResult(jObject, 0);//0 - Get single post
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

    private void ReceivedResult(JObject jsonData, int serviceType)
    {
        selectedItem = JsonConvert.DeserializeObject<PostsModel>(jsonData.ToString());
        uIDLbl.Text = selectedItem.userId.ToString();
        idLbl.Text = selectedItem.id.ToString();
        titleLbl.Text = selectedItem.title;
        bodyLbl.Text = selectedItem.body;
        
        foreach (var post in posts)
        {
            if (post.id == selectedItem.id)
            {
                post.title = selectedItem.title;
                post.body = selectedItem.body;
            }
        }
        
        switch (serviceType)
        {
            case 0:
                break;
            case 1:
                Navigation.PopAsync();
                break;
        }
    }

    async private void Update_OnClicked(object? sender, EventArgs e)
    {
        selectedItem.title = titleLbl.Text;
        selectedItem.body = bodyLbl.Text;
        
        if (networkHelper.HasInternet())
        {
            if (await networkHelper.IsHostReachable() == true)
            {
                var uri = new Uri(Constants.URL + Constants.POSTS + "/" + selectedItem.id);
                string json = JsonConvert.SerializeObject(selectedItem);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                response = await client.PutAsync(uri, content, cts);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    
                    JObject jObject = new JObject();
                    try
                    {
                        jObject = JObject.Parse(result);
                        ReceivedResult(jObject, 1);//1 - PUT, update post
                    }
                    catch (Exception ex)
                    {
                        JArray jA = JArray.Parse(result);
                        jObject = JObject.Parse("{\"count\":" + jA.Count + ",\"data\":" + JsonConvert.SerializeObject(jA) + "}");
                        ReceivedResult(jObject, 1);//1 - PUT, update post
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
}