namespace FYKJ.Framework.Web
{
    using System.Collections.Specialized;
    using System.Web.Routing;

    public class WebExceptionContext
    {
        public string CurrentUrl { get; set; }

        public NameValueCollection FormData { get; set; }

        public string IP { get; set; }

        public bool IsAjaxRequest { get; set; }

        public NameValueCollection QueryData { get; set; }

        public string RefUrl { get; set; }

        public RouteValueDictionary RouteData { get; set; }
    }
}

