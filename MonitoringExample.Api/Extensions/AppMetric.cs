using App.Metrics;

namespace MonitoringExample.Api.Extensions
{
    public static partial class Extension
    {

        private static MetricTags GetMetricTag(string key, string value)
        {
            return new MetricTags(key, value);
        }

        public static MetricTags Add(this MetricTags tags, string key, object value)
        {
            if (value.GetType().IsValueType || value.GetType() == typeof(string))
                tags = MetricTags.Concat(tags, GetMetricTag(key, value.ToString()));
            return tags;
        }

        public static MetricTags Add(this MetricTags tags, MetricTags newTag)
        {
            tags = MetricTags.Concat(tags, newTag);
            return tags;
        }
    }
}
