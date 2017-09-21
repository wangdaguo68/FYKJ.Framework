namespace FYKJ.Module
{
    using System;
    using System.Text;
    using System.Web;
    using System.Web.Routing;
    using Framework.Utility;

    public abstract class ContextCollectHandler : IHttpHandler, IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return this;
        }

        public void ProcessRequest(HttpContext context)
        {
            var res = context.Response;
            var req = context.Request;
            res.Clear();
            res.ContentEncoding = Encoding.Default;
            if (!string.Equals(Fetch.ServerDomain, "localhost", StringComparison.OrdinalIgnoreCase))
            {
                res.Write("<h1>非法访问收集数据！</h1>");
            }
            else
            {
                ProcessRequest(req, res);
            }
            res.Flush();
            res.End();
            res.Close();
        }

        public abstract void ProcessRequest(HttpRequest req, HttpResponse res);

        public bool IsReusable => false;
    }
}

