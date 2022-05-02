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
        private EskomApiWrapperConfiguration _configuration;

        public EskomApiWrapper() { }

        public EskomApiWrapper(EskomApiWrapperConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GetStatusAsync()
        {
            WebRequest request = WebRequest.Create(new Uri(_baseUrl + "/GetStatus"));
            request.Method = "GET";
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.127 Safari/537.36");
            request.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9");

            if (_configuration != null &&
                !string.IsNullOrEmpty(_configuration.ProxyAddress) &&
                _configuration.ProxyEnabled &&
                _configuration.ProxyPort >= 0 &&
                _configuration.ProxyPort <= 65535)
            {
                WebProxy webProxy = new WebProxy(_configuration.ProxyAddress, _configuration.ProxyPort);
                webProxy.BypassProxyOnLocal = true;
                request.Proxy = webProxy;
            }

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
