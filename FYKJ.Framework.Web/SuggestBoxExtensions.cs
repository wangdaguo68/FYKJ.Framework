namespace FYKJ.Framework.Web.Controls
{
    using System.Collections.Generic;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    public static class SuggestBoxExtensions
    {
        public static MvcHtmlString SuggestBox(this HtmlHelper htmlHelper, string name, object value, string controller, string action, string fieldName, string callBack, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.SuggestBox(name, value, controller, action, "", fieldName, fieldName, "", "", "", callBack, htmlAttributes);
        }

        public static MvcHtmlString SuggestBox(this HtmlHelper htmlHelper, string name, object value, string controller, string action, string headerText, string displayFields, string valueField, string callBack, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.SuggestBox(name, value, controller, action, headerText, displayFields, valueField, "", "", "", callBack, htmlAttributes);
        }

        public static MvcHtmlString SuggestBox(this HtmlHelper htmlHelper, string name, object value, string controller, string action, string headerText, string displayFields, string valueField, string keyField, string keyTextBoxName, string keyTextBoxValue, string callBack, IDictionary<string, object> htmlAttributes)
        {
            StringBuilder builder = new StringBuilder();
            if (htmlAttributes == null)
            {
                htmlAttributes = new Dictionary<string, object>();
            }
            string str = "";
            if (htmlAttributes.ContainsKey("style"))
            {
                str = htmlAttributes["style"].ToString();
            }
            string str2 = name.ToUpper() + "_SUGBOX";
            if (str.Length > 0)
            {
                builder.Append(htmlHelper.TextBox(name, value, new { style = str, autocomplete = "off" }));
            }
            else
            {
                builder.Append(htmlHelper.TextBox(name, value, new { autocomplete = "off" }));
            }
            builder.Append("<script type=\"text/javascript\">");
            builder.AppendFormat("$('{0}').suggest({{boxId:'{1}',controller:'{2}',action:'{3}',headerText:'{4}',displayFields:'{5}',valueField:'{6}',keyField:'{7}',keyTextBoxName:'{8}',callBack:'{9}'}})", "#" + name, str2, controller, action, headerText, displayFields, valueField, keyField, keyTextBoxName, callBack);
            builder.Append("</script>");
            if (keyTextBoxName != "")
            {
                builder.Append(htmlHelper.Hidden(keyTextBoxName, keyTextBoxValue));
            }
            return new MvcHtmlString(builder.ToString());
        }
    }
}

