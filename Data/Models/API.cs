using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SendingDataByEmail.Data.Models
{
    public class API
    {
        public API(int apiId, string name, string url, string city)
        {
            ApiId = apiId;
            Name = name;
            Url = url;
            City = city;
        }

        public int ApiId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string City { get; set; }

    }
}