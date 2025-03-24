using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiSample
{
    public interface iNetworkHelper
    {
        bool HasInternet();
        Task<bool> IsHostReachable();
    }
}
