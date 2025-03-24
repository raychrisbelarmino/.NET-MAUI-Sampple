using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

namespace MauiSample;

public partial class Page4 : ContentPage
{
	int selectedID;
    ObservableCollection<PostsModel> posts = new ObservableCollection<PostsModel>();
    PostsModel selectedItem = new PostsModel();
    NetworkHelper networkHelper;
    HttpClient client;
    CancellationToken cts;
    CancellationTokenSource cs = new CancellationTokenSource();
    HttpResponseMessage response;
    public Page4(int id, ObservableCollection<PostsModel> postsParam)
	{
		InitializeComponent();
		selectedID = id;
        posts = postsParam;
        NavigationPage.SetHasNavigationBar(this, false);
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

        //GET 
        if (networkHelper.HasInternet())
        {
            if (await networkHelper.IsHostReachable() == true)
            {
                var uri = new Uri(Constants.URL + Constants.POSTS + "/" + selectedID);
                response = await client.GetAsync(uri, cts);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    Console.WriteLine("Response: " + result);

                    JObject jObject = new JObject();
                    try
                    {
                        jObject = JObject.Parse(result);
                        Console.WriteLine("JObject:  " + jObject);
                        ReceivedResult(jObject);
                    }
                    catch (Exception e)
                    {
                        JArray jA = JArray.Parse(result);

                        jObject = JObject.Parse("{\"count\":" + jA.Count + ",\"data\":" + JsonConvert.SerializeObject(jA) + "}");

                        Console.WriteLine("JObject if JArray:  " + jObject);
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

    public void ReceivedResult(JObject jsonData)
    {
        selectedItem = JsonConvert.DeserializeObject<PostsModel>(jsonData.ToString());
        uIDLbl.Text = "User ID:" + selectedItem.userId.ToString();
        idLbl.Text = "ID:" + selectedItem.id.ToString();
        titleLbl.Text = "Title:" + selectedItem.title;
        bodyLbl.Text = "Body:" + selectedItem.body;
    }
}