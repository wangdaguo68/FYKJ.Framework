namespace FYKJ.Framework.Utility
{
    using System;
    using System.Web;

    public class Cookie
    {
        public static HttpCookie Get(string name)
        {
            return HttpContext.Current.Request.Cookies[name];
        }

        public static string GetValue(string name)
        {
            var cookie = Get(name);
            return cookie != null ? cookie.Value : string.Empty;
        }

        public static void Remove(string name)
        {
            Remove(Get(name));
        }

        public static void Remove(HttpCookie cookie)
        {
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now;
                Save(cookie);
            }
        }

        public static void Save(HttpCookie cookie, int expiresHours = 0)
        {
            var serverDomain = Fetch.ServerDomain;
            var str2 = HttpContext.Current.Request.Url.Host.ToLower();
            if (serverDomain != str2)
            {
                cookie.Domain = serverDomain;
            }
            if (expiresHours > 0)
            {
                cookie.Expires = DateTime.Now.AddHours(expiresHours);
            }
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static void Save(string name, string value, int expiresHours = 0)
        {
            var cookie = Get(name) ?? Set(name);
            cookie.Value = value;
            Save(cookie, expiresHours);
        }

        public static HttpCookie Set(string name)
        {
            return new HttpCookie(name);
        }
    }
}

