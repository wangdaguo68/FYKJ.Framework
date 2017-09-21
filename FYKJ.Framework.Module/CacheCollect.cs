namespace FYKJ.Module
{
    using DevTrends.MvcDonutCaching;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Web;
    using Cache;

    public class CacheCollect : ContextCollectHandler
    {
        public override void ProcessRequest(HttpRequest req, HttpResponse res)
        {
            if (req.QueryString.Count == 0)
            {
                res.Write("<p><h1>网站当前缓存列表：</h1><p>");
                var values = new List<string>();
                var item = "<a href='?cacheclear=true' target='_blank'>！点击清除所有缓存</a>";
                values.Add(item);
                var enumerator = HttpRuntime.Cache.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var str2 = enumerator.Key.ToString();
                    item = string.Format("<b>{0}</b>：{1}（<a href='?key={0}' target='_blank'>查看数据</a>）", str2, enumerator.Value.GetType());
                    values.Add(item);
                }
                values.Sort();
                res.Write(string.Join("<hr>", values));
            }
            else if (req["cacheclear"] != null)
            {
                CacheHelper.Clear();
                new OutputCacheManager().RemoveItems();
                res.Write("清除缓存成功！");
            }
            else if (req["key"] != null)
            {
                var obj2 = CacheHelper.Get(req["key"]);
                if (obj2 != null)
                {
                    res.Write(JsonConvert.SerializeObject(obj2));
                }
            }
        }
    }
}

