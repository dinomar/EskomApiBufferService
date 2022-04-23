using System;
using System.Collections.Generic;
using System.Text;

namespace EskomApiBufferService
{
    public class Status
    {
        public string Level { get; private set; }
        public DateTime Updated { get; private set; }

        public Status(string level)
        {
            Level = level;
            Updated = DateTime.Now;
        }
    }
}
