namespace FYKJ.Framework.Web.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Text;
    using System.Web.Mvc;

    public static class CheckBoxListHelper
    {
        public static MvcHtmlString CheckBoxList(this HtmlHelper helper, string name, bool isHorizon = true)
        {
            return helper.CheckBoxList(name, (helper.ViewData[name] as IEnumerable<SelectListItem>), new object(), isHorizon);
        }

        public static MvcHtmlString CheckBoxList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> selectList, bool isHorizon = true)
        {
            return helper.CheckBoxList(name, selectList, new object(), isHorizon);
        }

        public static MvcHtmlString CheckBoxList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> selectList, object htmlAttributes, bool isHorizon = true)
        {
            return helper.CheckBoxList(name, name, selectList, htmlAttributes, isHorizon);
        }

        public static MvcHtmlString CheckBoxList(this HtmlHelper helper, string id, string name, IEnumerable<SelectListItem> selectList, object htmlAttributes, bool isHorizon = true)
        {
            IDictionary<string, object> ht = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            HashSet<string> set = new HashSet<string>();
            List<SelectListItem> list = new List<SelectListItem>();
            string str = ((selectList as SelectList).SelectedValue == null) ? string.Empty : Convert.ToString((selectList as SelectList).SelectedValue);
            if (!string.IsNullOrEmpty(str))
            {
                if (str.Contains(","))
                {
                    string[] strArray = str.Split(new char[] { ',' });
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        set.Add(strArray[i].Trim());
                    }
                }
                else
                {
                    set.Add(str);
                }
            }
            foreach (SelectListItem item in selectList)
            {
                item.Selected = (item.Value != null) ? set.Contains(item.Value) : set.Contains(item.Text);
                list.Add(item);
            }
            selectList = list;
            ht.Add("type", "checkbox");
            ht.Add("id", id);
            ht.Add("name", name);
            ht.Add("style", "border:none;");
            StringBuilder builder = new StringBuilder();
            foreach (SelectListItem item2 in selectList)
            {
                IDictionary<string, object> attributes = ht.DeepCopy();
                attributes.Add("value", item2.Value);
                if (item2.Selected)
                {
                    attributes.Add("checked", "checked");
                }
                TagBuilder builder2 = new TagBuilder("input");
                builder2.MergeAttributes(attributes);
                string str2 = builder2.ToString(TagRenderMode.SelfClosing);
                string format = isHorizon ? "<label> {0}  {1}</label>" : "<p><label> {0}  {1}</label></p>";
                builder.AppendFormat(format, str2, item2.Text);
            }
            return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper helper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, bool isHorizon = true)
        {
            string[] strArray = expression.ToString().Split(".".ToCharArray());
            string id = string.Join("_", strArray, 1, strArray.Length - 1);
            string name = string.Join(".", strArray, 1, strArray.Length - 1);
            return helper.CheckBoxList(id, name, selectList, new object(), isHorizon);
        }

        private static IDictionary<string, object> DeepCopy(this IDictionary<string, object> ht)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (KeyValuePair<string, object> pair in ht)
            {
                dictionary.Add(pair.Key, pair.Value);
            }
            return dictionary;
        }
    }
}

