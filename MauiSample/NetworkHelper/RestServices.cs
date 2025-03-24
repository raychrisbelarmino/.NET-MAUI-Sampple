using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MauiSample
{
    public class RestServices
    {
        NetworkHelper networkHelper = NetworkHelper.GetInstance;
        HttpClient client;

        static RestServices _restService;
        WeakReference<IRestConnector> _webServiceDelegate;
        public IRestConnector WebServiceDelegate
        {
            get
            {
                IRestConnector webServiceDelegate;
                return _webServiceDelegate.TryGetTarget(out webServiceDelegate) ? webServiceDelegate : null;
            }

            set
            {
                _webServiceDelegate = new WeakReference<IRestConnector>(value);
            }
        }

        public static RestServices GetInstance
        {
            get { if (_restService == null) _restService = new RestServices(); return _restService; }
        }

        public RestServices()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.MaxResponseContentBufferSize = 256000;
            client.Timeout = TimeSpan.FromMinutes(1);
        }

        public async Task GetRequest(string url, int serviceType, CancellationToken ct)
        {
            //Console.WriteLine("Request URL: " + url);
            if (networkHelper.HasInternet())
            {
                if (await networkHelper.IsHostReachable() == true)
                {
                    var uri = new Uri(url);

                    HttpResponseMessage response = await client.GetAsync(uri, ct);
                    await RequestAsync(response, serviceType, ct);
                }
                else
                {
                    //Console.WriteLine("Host Unreachable!" + " - " + "The URL host for <Your App> cannot be reached and seems to be unavailable. Please try again later!");
                    WebServiceDelegate?.ReceiveTimeoutError("Host Unreachable!", "The URL host for <Your App> cannot be reached and seems to be unavailable. Please try again later!", serviceType);
                }
            }
            else
            {
                //Console.WriteLine("No Internet Connection!" + " - " + "Please check your internet connection, and try again!");
                WebServiceDelegate?.ReceiveTimeoutError("No Internet Connection!", "Please check your internet connection, and try again!", serviceType);
            }
        }

        public async Task PostRequestAsync(string url, string dictionary, int serviceType, CancellationToken ct)
        {
            //Console.WriteLine("URL: " + url);
            //Console.WriteLine("Contents: " + dictionary);
            //Console.WriteLine("Cancellation Token: " + ct.ToString());

            if (networkHelper.HasInternet())
            {
                if (await networkHelper.IsHostReachable() == true)
                {
                    var uri = new Uri(url);
                    var content = new StringContent(dictionary, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(uri, content, ct);

                    await RequestAsync(response, serviceType, ct);
                }
                else
                {
                    WebServiceDelegate?.ReceiveTimeoutError("Host Unreachable!", "The URL host for <Your App> cannot be reached and seems to be unavailable. Please try again later!", serviceType);
                }
            }
            else
            {
                WebServiceDelegate?.ReceiveTimeoutError("No Internet Connection!", "Please check your internet connection, and try again!", serviceType);
            }
        }

        async Task RequestAsync(HttpResponseMessage response, int serviceType, CancellationToken ct)
        {
            //Console.WriteLine("Request Success? : " + response.IsSuccessStatusCode);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
               //Console.WriteLine("Response: " + result);

                
                try{
                    WebServiceDelegate?.ReceiveJSONData(JObject.Parse(result), serviceType, ct);
                }catch (Exception e){
                    JArray jA = JArray.Parse(result);
                    string output = "{\"data\":"+JsonConvert.SerializeObject(jA)+"}";

                    //Console.WriteLine("serialized array: "+output);
                    WebServiceDelegate?.ReceiveJSONData(JObject.Parse(output), serviceType, ct);
                }    
            }
            else
            {
                //var errorResult = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                WebServiceDelegate?.ReceiveTimeoutError("Error!", response.StatusCode.ToString(), serviceType);
                //if (!string.IsNullOrEmpty(errorResult) && !errorResult.Contains("<html"))
                //{
                //    var json = JObject.Parse(errorResult);

                //    if (json["status"] != null)
                //    {
                //        WebServiceDelegate?.ReceiveJSONData(json, serviceType, ct);
                //    }
                //    else if (json["error"] != null)
                //    {
                //        WebServiceDelegate?.ReceiveTimeoutError("Error!", json["error"].ToString(), serviceType);
                //    }
                //    else
                //    {
                //        WebServiceDelegate?.ReceiveTimeoutError("Error!", json["errors"][0].ToString(), serviceType);
                //    }
                //}
                //else
                //{
                //    WebServiceDelegate?.ReceiveTimeoutError("Error!", response.StatusCode.ToString(), serviceType);
                //}
            }
        }

        public async Task PutRequestAsync(string url, string dictionary, int serviceType, CancellationToken ct)
        {
            //Console.WriteLine("URL: " + url);
            //Console.WriteLine("Contents: " + dictionary);
            if (networkHelper.HasInternet())
            {
                if (await networkHelper.IsHostReachable() == true)
                {
                    var uri = new Uri(url);
                    var content = new StringContent(dictionary, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync(uri, content, ct);

                    await RequestAsync(response, serviceType, ct);
                }
                else
                {
                    WebServiceDelegate?.ReceiveTimeoutError("Host Unreachable!", "The URL host for <Your App> cannot be reached and seems to be unavailable. Please try again later!", serviceType);
                }
            }
            else
            {
                WebServiceDelegate?.ReceiveTimeoutError("No Internet Connection!", "Please check your internet connection, and try again!", serviceType);
            }

        }

        public async Task DeleteRequestAsync(string url, int serviceType, CancellationToken ct)
        {
            //Console.WriteLine("URL: " + url);
            if (networkHelper.HasInternet())
            {
                if (await networkHelper.IsHostReachable() == true)
                {
                    var uri = new Uri(url);

                    HttpResponseMessage response = await client.DeleteAsync(uri, ct);

                    await RequestAsync(response, serviceType, ct);
                }
                else
                {
                    WebServiceDelegate?.ReceiveTimeoutError("Host Unreachable!", "The URL host for <Your App> cannot be reached and seems to be unavailable. Please try again later!", serviceType);
                }
            }
            else
            {
                WebServiceDelegate?.ReceiveTimeoutError("No Internet Connection!", "Please check your internet connection, and try again!", serviceType);
            }
        }
    }
}
