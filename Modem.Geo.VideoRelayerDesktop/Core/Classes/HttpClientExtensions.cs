using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Modem.Geo.VideoRelayerDesktop.Core.Classes
{
    public static class HttpClientExtesnions
    {
        public static async Task LoginAsync(this HttpClient client, string uri, object obj)
        {
            await client.PostAsync(
                uri,
                new StringContent(
                    JsonSerializer.Serialize(obj),
                    Encoding.UTF8,
                    "application/json"));
        }

        public static async Task LogoutAsync(this HttpClient client, string uri)
        {
            await client.PostAsync(
                uri,
                new StringContent(
                    "",
                    Encoding.UTF8,
                    "application/json"));
        }
    }

}
