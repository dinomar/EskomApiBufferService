using System;
using System.Collections.Generic;
using System.Text;

namespace EskomApiBufferService
{
    public class MockEskomApiWrapperConfiguration
    {
        public string GetStatusResponse { get; set; } = "0";
        public int Delay { get; set; } = 1000;
        public bool ThrowWebException { get; set; } = false;
        public bool ThrowContinuousWebExceptions { get; set; } = false;
    }
}
