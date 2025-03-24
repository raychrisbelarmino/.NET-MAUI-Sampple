using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiSample
{
    public interface IRestService
    {
        Task PostRequestAsync(string url, string dictionary, CancellationToken ct);
        Task GetRequest(string url, CancellationToken ct);
        Task PutRequestAsync(string url, string dictionary, CancellationToken ct);
        Task DeleteRequestAsync(string url, CancellationToken ct);
    }
}
