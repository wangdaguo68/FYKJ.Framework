using System;
using System.Web.Mvc;

namespace FYKJ.Framework.Web
{
    public class AuthorizeFilterAttribute : ActionFilterAttribute
    {
        public AuthorizeFilterAttribute(string name)
        {
            Name = name;
        }

        protected virtual bool Authorize(ActionExecutingContext filterContext, string permissionName)
        {
            if (filterContext.HttpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }
            return true;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Authorize(filterContext, Name))
            {
                ContentResult result = new ContentResult {
                    Content = "<script>alert('抱歉,你不具有当前操作的权限！');history.go(-1)</script>"
                };
                filterContext.Result = result;
            }
        }

        public string Name { get; set; }
    }
}

