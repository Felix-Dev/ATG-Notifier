using System;
using System.Collections.Generic;
using System.Text;

namespace ATG_Notifier.Data.Utilities
{
    public class UIdGenerator
    {
        private static long prevId;

        private static readonly object idLock = new object();

        private static readonly DateTime DateSeed = DateTime.Parse("2019/01/01");

        static public long Next(int prefix = 1)
        {
            lock (idLock)
            {
                var id = (long)(DateTime.UtcNow - DateSeed).TotalMilliseconds + prefix * 100000000000;
                if (id == prevId)
                {
                    id += 1;
                }

                prevId = id;
                return id;
            }
        }
    }
}
