using System;
using System.ComponentModel;

namespace MonitoringExample.Api.Extensions
{
    public static partial class Extension
    {
        public static string GetDescription(this Enum value)
        {
            if (value == null)
                return null;

            var fi = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes != null && attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}
