using System;
using System.Collections.Generic;
using System.Text;

namespace EskomApiBufferService
{
    public class EskomBufferServiceConfiguration
    {
        public IEskomApiWrapper EskomApiWrapper { get; set; }

        public int Retries { get; set; } = 3;
        public int StatusMinRange { get; set; } = 0;
        public int StatusMaxRange { get; set; } = 10;
        public int DelayInMinutes { get; set; } = 1;
        public int MaxLogs { get; set; } = 1000;
    }
}
