using System;

namespace MonitoringExample.Api.Extensions
{
    public static partial class Extension
    {
        private static object lockObj = new();
        private static Random random = new();


        public static int RandomGenerator(int min, int max)
        {
            lock (lockObj)
            {
                return Convert.ToInt32(random.NextDouble() * (max - min) + min);
            }
        }
    }
}
