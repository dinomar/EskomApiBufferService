using System;
using System.Collections.Generic;
using System.Text;

namespace EskomApiBufferService
{
    public class Status
    {
        public int Level { get; private set; }
        public DateTime Updated { get; private set; }

        public Status(string level)
        {
            int i;
            if (!Int32.TryParse(level, out i))
            {
                throw new FormatException("level could not be parsed to a int value.");
            }

            Level = i;
            Updated = DateTime.Now;
        }
    }
}
