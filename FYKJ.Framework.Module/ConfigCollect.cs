using System.Linq;

namespace FYKJ.Module
{
    using System.Text;
    using System.Web;
    using Config;
    using Framework.Utility;

    public class ConfigCollect : ContextCollectHandler
    {
        public override void ProcessRequest(HttpRequest req, HttpResponse res)
        {
            var type = typeof(CachedConfigContext);
            var current = CachedConfigContext.Current;
            if (string.IsNullOrEmpty(req["config"]))
            {
                res.Write("<p><h1>网站当前配置列表：</h1><p>");
                foreach (var info in type.GetProperties().Where(info => info.Name != "ConfigService"))
                {
                    res.Write(string.Format("<p><a href='?config={0}' target='_blank'>{0} [点击查看]</a></p>", info.Name));
                }
            }
            else
            {
                foreach (var info2 in type.GetProperties())
                {
                    if ((info2.Name == req["config"]) && (info2.Name != "DaoConfig"))
                    {
                        var obj2 = info2.GetValue(current, null);
                        if (obj2 != null)
                        {
                            res.ContentType = "text/xml";
                            res.ContentEncoding = Encoding.UTF8;
                            res.Write(SerializationHelper.XmlSerialize(obj2));
                            return;
                        }
                    }
                }
            }
        }
    }
}

