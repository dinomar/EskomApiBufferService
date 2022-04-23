using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EskomApiBufferService
{
    public interface IEskomApiWrapper
    {
        Task<string> GetStatusAsync();
    }
}
