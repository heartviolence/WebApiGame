using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Assets.Scripts
{
    internal static class GameApiClient
    {
        public static HttpClient Client { get; set; }
    }
}
