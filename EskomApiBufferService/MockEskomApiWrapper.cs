using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EskomApiBufferService
{
    public class MockEskomApiWrapper : IEskomApiWrapper
    {
        public string GetStatusResponse { get; set; } = "0";
        public int Delay { get; set; } = 1000;
        public bool ThrowWebException { get; set; } = false;
        public bool ThrowContinuousWebExceptions { get; set; } = false;

        public MockEskomApiWrapper() { }

        public MockEskomApiWrapper(MockEskomApiWrapperConfiguration configuration)
        {
            GetStatusResponse = configuration.GetStatusResponse;
            Delay = configuration.Delay;
            ThrowWebException = configuration.ThrowWebException;
            ThrowContinuousWebExceptions = configuration.ThrowContinuousWebExceptions;
        }

        public async Task<string> GetStatusAsync()
        {
            return await Task.Run(async () =>
            {
                await Task.Delay(Delay);

                if (ThrowWebException)
                {
                    ThrowWebException = false;
                    throw new System.Net.WebException();
                }
                else if (ThrowContinuousWebExceptions)
                {
                    throw new System.Net.WebException();
                }

                return GetStatusResponse;
            });
        }
    }
}
