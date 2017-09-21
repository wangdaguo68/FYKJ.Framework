using System;
using System.Collections.Generic;
using System.Web;

namespace FYKJ.Framework.Utility
{
    public class Fetch
    {
        public static string Get(string name)
        {
            string str = HttpContext.Current.Request.QueryString[name];
            if (str != null)
            {
                return str.Trim();
            }
            return "";
        }

        public static int[] GetIds(string name)
        {
            string str = Post(name);
            List<int> list = new List<int>();
            int result = 0;
            foreach (string str2 in str.Split(','))
            {
                if (int.TryParse(str2.Trim(), out result))
                {
                    list.Add(result);
                }
            }
            return list.ToArray();
        }

        public static int GetQueryId(string name)
        {
            int result = 0;
            int.TryParse(Get(name), out result);
            return result;
        }

        public static int[] GetQueryIds(string name)
        {
            string str = Get(name);
            List<int> list = new List<int>();
            int result = 0;
            foreach (string str2 in str.Split(','))
            {
                if (int.TryParse(str2.Trim(), out result))
                {
                    list.Add(result);
                }
            }
            return list.ToArray();
        }

        public static string Post(string name)
        {
            string str = HttpContext.Current.Request.Form[name];
            if (str != null)
            {
                return str.Trim();
            }
            return "";
        }

        public static string CurrentUrl
        {
            get
            {
                return HttpContext.Current.Request.Url.ToString();
            }
        }

        public static string ServerDomain
        {
            get
            {
                var s = HttpContext.Current.Request.Url.Host.ToLower();
                if ((s.Split('.').Length >= 3) && !RegExp.IsIp(s))
                {
                    var str2 = s.Remove(0, s.IndexOf(".", StringComparison.Ordinal) + 1);
                    if ((!str2.StartsWith("com.") && !str2.StartsWith("net.")) && (!str2.StartsWith("org.") && !str2.StartsWith("gov.")))
                    {
                        return str2;
                    }
                }
                return s;
            }
        }

        public static string UserIp
        {
            get
            {
                string str2;
                string s = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (((str2 = s) == null) || (str2 == ""))
                {
                    s = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                if (!RegExp.IsIp(s))
                {
                    return "Unknown";
                }
                return s;
            }
        }
    }
}

