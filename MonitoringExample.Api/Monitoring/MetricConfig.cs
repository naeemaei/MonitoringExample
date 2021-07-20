public class MetricConfig
{
    private MetricConfig()
    {
    }
    public static MetricConfig Instance { get; protected set; } = new MetricConfig();

    public float ErrorPercent { get; set; }
    public int AverageResponseTime { get; set; }
}