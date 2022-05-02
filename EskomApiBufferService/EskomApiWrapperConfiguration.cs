using System;
using System.Collections.Generic;
using System.Text;

namespace EskomApiBufferService
{
    public class EskomApiWrapperConfiguration
    {
        public bool ProxyEnabled { get; set; }
        public string ProxyAddress { get; set; }
        public int ProxyPort { get; set; }
    }
}
