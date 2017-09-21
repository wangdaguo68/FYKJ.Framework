using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using FYKJ.Framework.Contract;

namespace FYKJ.Framework.Web.Controls
{
    public static class PagerForSeo
    {
        private static string PrepearRouteUrl(HtmlHelper helper, string pageIndexParameterName, int pageIndex)
        {
            RouteValueDictionary routeValues = new RouteValueDictionary
            {
                ["action"] = helper.ViewContext.RequestContext.RouteData.Values["action"],
                ["controller"] = helper.ViewContext.RequestContext.RouteData.Values["controller"],
                [pageIndexParameterName] = pageIndex
            };
            UrlHelper helper2 = new UrlHelper(helper.ViewContext.RequestContext);
            return helper2.RouteUrl(routeValues);
        }

        public static MvcHtmlString SeoPager(this HtmlHelper helper, IPagedList pagedList, string pageIndexParameterName = "id", int sectionSize = 20)
        {
            Func<int, int> keySelector = null;
            Func<IGrouping<int, int>, bool> predicate = null;
            StringBuilder builder = new StringBuilder();
            int num = (pagedList.TotalItemCount / pagedList.PageSize) + (((pagedList.TotalItemCount % pagedList.PageSize) == 0) ? 0 : 1);
            if (num > 1)
            {
                List<int> source = new List<int>();
                for (int i = 1; i <= num; i++)
                {
                    source.Add(i);
                }
                keySelector = p => (p - 1) / sectionSize;
                IEnumerable<IGrouping<int, int>> enumerable = source.GroupBy(keySelector);
                predicate = s => s.Key == ((pagedList.CurrentPageIndex - 1) / sectionSize);
                IGrouping<int, int> grouping = enumerable.Single(predicate);
                foreach (int num3 in grouping)
                {
                    if (num3 == pagedList.CurrentPageIndex)
                    {
                        builder.AppendFormat("<span>{0}</span>", num3);
                    }
                    else
                    {
                        builder.AppendFormat("<a href=\"{1}\">{0}</a>", num3, PrepearRouteUrl(helper, pageIndexParameterName, num3));
                    }
                }
                if (enumerable.Count() > 1)
                {
                    builder.Append("<br/>");
                    foreach (IGrouping<int, int> grouping2 in enumerable)
                    {
                        if (grouping2.Key == grouping.Key)
                        {
                            builder.AppendFormat("<span>{0}-{1}</span>", grouping2.First(), grouping2.Last());
                        }
                        else
                        {
                            builder.AppendFormat("<a href=\"{2}\">{0}-{1}</a>", grouping2.First(), grouping2.Last(), PrepearRouteUrl(helper, pageIndexParameterName, grouping2.First()));
                        }
                    }
                }
            }
            return MvcHtmlString.Create(builder.ToString());
        }
    }
}

