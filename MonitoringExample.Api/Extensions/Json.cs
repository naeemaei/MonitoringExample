using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MonitoringExample.Api.Extensions
{
    public static partial class Extension
    {
        public static string ToJson(this object value, JsonSerializerSettings settings = null)
        {
            if (settings == null)
            {
                settings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
                settings.Converters.Add(new StringEnumConverter { AllowIntegerValues = false });
            }

            return JsonConvert.SerializeObject(value, settings);
        }
    }
}
