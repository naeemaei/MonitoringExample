public record MetricConfig
{
    public const string Key = nameof(MetricConfig);
    public float ErrorPercent { get; set; }
    public bool Enabled { get; set; }
    public int AverageResponseTime { get; set; }
    public int MetricPerSecond { get; set; }
}