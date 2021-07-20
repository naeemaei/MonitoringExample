using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MonitoringExample.Api.Dtos
{
    public class Labels
    {
        [JsonProperty("alertname")]
        public string Alertname { get; set; }

        [JsonProperty("instance")]
        public string Instance { get; set; }

        [JsonProperty("job")]
        public string Job { get; set; }

        [JsonProperty("monitor")]
        public string Monitor { get; set; }

        [JsonProperty("severity")]
        public string Severity { get; set; }
    }

    public class Annotations
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }
    }

    public class Alert
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("labels")]
        public Labels Labels { get; set; }

        [JsonProperty("annotations")]
        public Annotations Annotations { get; set; }

        [JsonProperty("startsAt")]
        public DateTime StartsAt { get; set; }

        [JsonProperty("endsAt")]
        public DateTime EndsAt { get; set; }

        [JsonProperty("generatorURL")]
        public string GeneratorURL { get; set; }

        [JsonProperty("fingerprint")]
        public string Fingerprint { get; set; }
    }

    public class GroupLabels
    {
    }

    public class CommonLabels
    {
        [JsonProperty("alertname")]
        public string Alertname { get; set; }

        [JsonProperty("job")]
        public string Job { get; set; }

        [JsonProperty("monitor")]
        public string Monitor { get; set; }

        [JsonProperty("severity")]
        public string Severity { get; set; }
    }

    public class CommonAnnotations
    {
    }

    public class MonitoringAlertDto
    {
        [JsonProperty("receiver")]
        public string Receiver { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("alerts")]
        public List<Alert> Alerts { get; set; }

        [JsonProperty("groupLabels")]
        public GroupLabels GroupLabels { get; set; }

        [JsonProperty("commonLabels")]
        public CommonLabels CommonLabels { get; set; }

        [JsonProperty("commonAnnotations")]
        public CommonAnnotations CommonAnnotations { get; set; }

        [JsonProperty("externalURL")]
        public string ExternalURL { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("groupKey")]
        public string GroupKey { get; set; }

        [JsonProperty("truncatedAlerts")]
        public int TruncatedAlerts { get; set; }
    }

}