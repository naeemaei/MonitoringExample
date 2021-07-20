using System.ComponentModel;

namespace MonitoringExample.Api.Enums
{
    public enum Endpoints
    {
        [Description("search")]
        Search = 1,

        [Description("product-detail-page")]
        ProductDetailPage = 2,

        [Description("register-order")]
        Ordering = 3,

        [Description("register-payment")]
        Payment = 4,
    }
}
