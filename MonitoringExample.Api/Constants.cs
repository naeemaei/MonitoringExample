using System.Collections.Generic;

namespace MonitoringExample.Api
{
    public static class Constants
    {
        public static int MaxNum { get; } = 100;
        public static int MinNum { get; } = 0;
        public static int MidNum { get; } = 50;
        public static Dictionary<int, float> ResponseTimeDistribution = new()
        { 
            { 5, 0.04f},
            { 17, 0.1f},
            { 35, 0.37f },
            { 50, 0.75f },
            { 65, 1 },
            { 85, 1.5f },
            { 95, 2 },
            { 100, 3 }
        };
    }
}
