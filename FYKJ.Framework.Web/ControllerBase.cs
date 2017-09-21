using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FYKJ.Framework.Contract;
using FYKJ.Framework.Utility;
using Newtonsoft.Json;

namespace FYKJ.Framework.Web
{
    public class ControllerBase : Controller
    {
        public ContentResult Back(string notice)
        {
            StringBuilder builder = new StringBuilder("<script>");
            if (!string.IsNullOrEmpty(notice))
            {
                builder.AppendFormat("alert('{0}');", notice);
            }
            builder.Append("history.go(-1)</script>");
            return Content(builder.ToString());
        }

        public virtual void ClearOperater()
        {
        }

        public ContentResult CloseThickbox()
        {
            return Content("<script>top.tb_remove()</script>");
        }

        protected ContentResult JsonP(string callback, object data)
        {
            string str = JsonConvert.SerializeObject(data);
            return Content(string.Format("{0}({1})", callback, str));
        }

        protected virtual void LogException(Exception exception, WebExceptionContext exceptionContext = null)
        {
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            if (!filterContext.RequestContext.HttpContext.Request.IsAjaxRequest() && !filterContext.IsChildAction)
            {
                RenderViewData();
            }
            ClearOperater();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            UpdateOperater(filterContext);
            base.OnActionExecuting(filterContext);
            (from v in filterContext.ActionParameters.Values
                where v is Request
                select v).ToList().ForEach(v => ((Request) v).PageSize = PageSize);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            Exception exception = filterContext.Exception;
            LogException(exception, WebExceptionContext);
        }

        public ContentResult PageReturn(string msg, string url = null)
        {
            StringBuilder builder = new StringBuilder("<script type='text/javascript'>");
            if (!string.IsNullOrEmpty(msg))
            {
                builder.AppendFormat("alert('{0}');", msg);
            }
            if (string.IsNullOrWhiteSpace(url))
            {
                url = Request.Url.ToString();
            }
            builder.Append("window.location.href='" + url + "'</script>");
            return Content(builder.ToString());
        }

        public ContentResult RefreshParent(string alert = null)
        {
            string content = string.Format("<script>{0}; parent.location.reload(1)</script>", string.IsNullOrEmpty(alert) ? string.Empty : ("alert('" + alert + "')"));
            return Content(content);
        }

        public ContentResult RefreshParentTab(string alert = null)
        {
            string content = string.Format("<script>{0}; if (window.opener != null) {{ window.opener.location.reload(); window.opener = null;window.open('', '_self', '');  window.close()}} else {{parent.location.reload(1)}}</script>", string.IsNullOrEmpty(alert) ? string.Empty : ("alert('" + alert + "')"));
            return Content(content);
        }

        protected virtual void RenderViewData()
        {
        }

        public ContentResult Stop(string notice, string redirect, bool isAlert = false)
        {
            string content = "<meta http-equiv='refresh' content='1;url=" + redirect + "' /><body style='margin-top:0px;color:red;font-size:24px;'>" + notice + "</body>";
            if (isAlert)
            {
                content = string.Format("<script>alert('{0}'); window.location.href='{1}'</script>", notice, redirect);
            }
            return Content(content);
        }

        public virtual void UpdateOperater(ActionExecutingContext filterContext)
        {
            if (Operater != null)
            {
                WCFContext.Current.Operater = Operater;
            }
        }

        public virtual Operater Operater
        {
            get
            {
                return null;
            }
        }

        public virtual int PageSize => 10;

        public WebExceptionContext WebExceptionContext
        {
            get
            {
                return new WebExceptionContext { IP = Fetch.UserIp, CurrentUrl = Fetch.CurrentUrl, RefUrl = ((Request == null) || (Request.UrlReferrer == null)) ? string.Empty : Request.UrlReferrer.AbsoluteUri, IsAjaxRequest = (Request != null) && Request.IsAjaxRequest(), FormData = (Request == null) ? null : Request.Form, QueryData = (Request == null) ? null : Request.QueryString, RouteData = (((Request == null) || (Request.RequestContext == null)) || (Request.RequestContext.RouteData == null)) ? null : Request.RequestContext.RouteData.Values };
            }
        }
    }
}

