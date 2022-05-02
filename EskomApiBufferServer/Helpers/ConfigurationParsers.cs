using EskomApiBufferService;
using Microsoft.Extensions.Configuration;
using System;

namespace EskomApiBufferServer.Helpers
{
    public static class ConfigurationParsers
    {
        public static EskomBufferServiceConfiguration ParseEskomBufferServiceConfiguration(IConfiguration configuration, EskomApiWrapper eskomApiWrapper = null)
        {
            EskomApiWrapperConfiguration eskomApiWrapperConfiguration = new EskomApiWrapperConfiguration();
            if (configuration.GetSection("EskomApiWrapper").Exists() && configuration.GetSection("EskomApiWrapper:Proxy").Exists())
            {
                bool proxyEnabled;
                int proxyPort;

                eskomApiWrapperConfiguration.ProxyEnabled = (Boolean.TryParse(configuration["EskomApiWrapper:Proxy:Enabled"]?.ToString(), out proxyEnabled) ? proxyEnabled : false);

                if (eskomApiWrapperConfiguration.ProxyEnabled)
                {
                    eskomApiWrapperConfiguration.ProxyAddress = (configuration["EskomApiWrapper:Proxy:Address"]?.ToString() ?? String.Empty);
                    eskomApiWrapperConfiguration.ProxyPort = (Int32.TryParse(configuration["EskomApiWrapper:Proxy:Port"]?.ToString(), out proxyPort) ? proxyPort : 0);
                }
            }


            int EskomBufferServiceRetries;
            int EskomBufferServiceStatusMinRange;
            int EskomBufferServiceStatusMaxRange;
            int EskomBufferServiceTimeInMinutes;
            int EskomBufferServiceMaxLogs;

            return new EskomBufferServiceConfiguration()
            {
                EskomApiWrapper = (eskomApiWrapper != null) ? eskomApiWrapper : new EskomApiWrapper(eskomApiWrapperConfiguration),
                Retries = (Int32.TryParse(configuration["EskomBufferService:Retries"]?.ToString(), out EskomBufferServiceRetries) ? EskomBufferServiceRetries : 3),
                StatusMinRange = (Int32.TryParse(configuration["EskomBufferService:StatusMinRange"]?.ToString(), out EskomBufferServiceStatusMinRange) ? EskomBufferServiceStatusMinRange : 0),
                StatusMaxRange = (Int32.TryParse(configuration["EskomBufferService:StatusMaxRange"]?.ToString(), out EskomBufferServiceStatusMaxRange) ? EskomBufferServiceStatusMaxRange : 10),
                DelayInMinutes = (Int32.TryParse(configuration["EskomBufferService:DelayInMinutes"]?.ToString(), out EskomBufferServiceTimeInMinutes) ? EskomBufferServiceTimeInMinutes : 60),
                MaxLogs = (Int32.TryParse(configuration["EskomBufferService:EskomBufferServiceMaxLogs"]?.ToString(), out EskomBufferServiceMaxLogs) ? EskomBufferServiceMaxLogs : 1000)
            };
        } 
    }
}
