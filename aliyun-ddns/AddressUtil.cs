using Serilog;
using System.Net;

namespace aliyun_ddns
{
    public class AddressUtil
    {
        static readonly HttpClient _client;
        static AddressUtil()
        {
            _client = new HttpClient();
        }
        public static async Task<IPAddress?> GetIPv4AddressAsync(string url)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(url)) url = "https://ipv4.ip.mir6.com";
                var response = await _client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _ = IPAddress.TryParse(content, out var ip);
                    return ip;
                }
            }
            catch (Exception ex) { Log.Error(ex, "Unhandled exception"); }


            return null;
        }
        public static async Task<IPAddress?> GetIPv6AddressAsync(string url)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(url)) url = "https://ipv6.ip.mir6.com";
                var response = await _client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _ = IPAddress.TryParse(content, out var ip);
                    return ip;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unhandled exception");
            }

            return null;
        }
    }
}
