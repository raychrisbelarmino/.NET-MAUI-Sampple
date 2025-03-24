using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;


namespace MauiSample
{
    public interface IRestConnector
    {
        void ReceiveJSONData(JObject jsonData, int serviceType, CancellationToken ct);
        void ReceiveTimeoutError(string title, string error, int serviceType);
    }
}
