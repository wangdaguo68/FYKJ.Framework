using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace FYKJ.Framework.Web.Controls
{
    public static class RadioBoxListHelper
    {
        private static IDictionary<string, object> DeepCopy(this IDictionary<string, object> ht)
        {
            return ht.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public static MvcHtmlString RadioBoxList(this HtmlHelper helper, string name)
        {
            return helper.RadioBoxList(name, (helper.ViewData[name] as IEnumerable<SelectListItem>), new object());
        }

        public static MvcHtmlString RadioBoxList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> selectList)
        {
            return helper.RadioBoxList(name, selectList, new object());
        }

        public static MvcHtmlString RadioBoxList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> selectList, object htmlAttributes)
        {
            IDictionary<string, object> ht = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            ht.Add("type", "radio");
            ht.Add("name", name);
            StringBuilder builder = new StringBuilder();
            int num = 0;
            int num2 = 0;
            foreach (SelectListItem item in selectList)
            {
                string str = string.Format("{0}{1}", name, num2++);
                IDictionary<string, object> attributes = ht.DeepCopy();
                attributes.Add("value", item.Value);
                attributes.Add("id", str);
                object selectedValue = (selectList as SelectList).SelectedValue;
                if (selectedValue == null)
                {
                    if (num++ == 0)
                    {
                        attributes.Add("checked", null);
                    }
                }
                else if (item.Value == selectedValue.ToString())
                {
                    attributes.Add("checked", null);
                }
                TagBuilder builder2 = new TagBuilder("input");
                builder2.MergeAttributes(attributes);
                string str2 = builder2.ToString(TagRenderMode.SelfClosing);
                builder.AppendFormat(" {0}  <label for='{2}'>{1}</label>", str2, item.Text, str);
            }
            return MvcHtmlString.Create(builder.ToString());
        }
    }
}

