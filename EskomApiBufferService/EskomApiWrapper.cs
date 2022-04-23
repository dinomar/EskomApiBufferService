using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EskomApiBufferService
{
    public class EskomApiWrapper : IEskomApiWrapper
    {
        private static readonly string _baseUrl = "https://loadshedding.eskom.co.za/LoadShedding";

        public async Task<string> GetStatusAsync()
        {
            WebRequest request = WebRequest.Create(new Uri(_baseUrl + "/GetStatus"));
            request.Method = "GET";
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.127 Safari/537.36");
            request.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9");

            using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
            {
                using (Stream resposeStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(resposeStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }
    }
}
