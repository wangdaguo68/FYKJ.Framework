using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FYKJ.Framework.Web.Controls
{
    public static class HtmlPrefixScope
    {
        private const string idsToReuseKey = "__htmlPrefixScopeExtensions_IdsToReuse_";

        public static IDisposable BeginCollectionItem(this HtmlHelper html, string collectionName)
        {
            Queue<string> idsToReuse = GetIdsToReuse(html.ViewContext.HttpContext, collectionName);
            string str = (idsToReuse.Count > 0) ? idsToReuse.Dequeue() : Guid.NewGuid().ToString();
            html.ViewContext.Writer.WriteLine(string.Format("<input type=\"hidden\" name=\"{0}.index\" autocomplete=\"off\" value=\"{1}\" />", collectionName, html.Encode(str)));
            return html.BeginHtmlFieldPrefixScope(string.Format("{0}[{1}]", collectionName, str));
        }

        public static IDisposable BeginHtmlFieldPrefixScope(this HtmlHelper html, string htmlFieldPrefix)
        {
            return new HtmlFieldPrefixScope(html.ViewData.TemplateInfo, htmlFieldPrefix);
        }

        private static Queue<string> GetIdsToReuse(System.Web.HttpContextBase httpContext, string collectionName)
        {
            string str = "__htmlPrefixScopeExtensions_IdsToReuse_" + collectionName;
            Queue<string> queue = (Queue<string>) httpContext.Items[str];
            if (queue == null)
            {
                httpContext.Items[str] = queue = new Queue<string>();
                string str2 = httpContext.Request[collectionName + ".index"];
                if (string.IsNullOrEmpty(str2))
                {
                    return queue;
                }
                foreach (string str3 in str2.Split(','))
                {
                    queue.Enqueue(str3);
                }
            }
            return queue;
        }

        private class HtmlFieldPrefixScope : IDisposable
        {
            private readonly string previousHtmlFieldPrefix;
            private readonly TemplateInfo templateInfo;

            public HtmlFieldPrefixScope(TemplateInfo templateInfo, string htmlFieldPrefix)
            {
                this.templateInfo = templateInfo;
                previousHtmlFieldPrefix = templateInfo.HtmlFieldPrefix;
                templateInfo.HtmlFieldPrefix = htmlFieldPrefix;
            }

            public void Dispose()
            {
                templateInfo.HtmlFieldPrefix = previousHtmlFieldPrefix;
            }
        }
    }
}

