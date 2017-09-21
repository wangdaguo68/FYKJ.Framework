using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace FYKJ.Framework.Web.Controls
{
    internal class PagerBuilder
    {
        private readonly string _actionName;
        private readonly AjaxHelper _ajax;
        private readonly AjaxOptions _ajaxOptions;
        private readonly string _controllerName;
        private readonly int _endPageIndex;
        private readonly HtmlHelper _html;
        private IDictionary<string, object> _htmlAttributes;
        private readonly bool _msAjaxPaging;
        private readonly int _pageIndex;
        private readonly PagerOptions _pagerOptions;
        private readonly string _routeName;
        private readonly RouteValueDictionary _routeValues;
        private readonly int _startPageIndex;
        private readonly int _totalItemCount;
        private readonly int _totalPageCount;
        private const string CopyrightText = "\r\n<!--MvcPager 1.5 for ASP.NET MVC 3.0 \x00a9 2009-2011 Webdiyer (http://www.webdiyer.com)-->\r\n";
        private const string GoToPageScript = "function _MvcPager_GoToPage(_pib,_mp){var pageIndex;if(_pib.tagName==\"SELECT\"){pageIndex=_pib.options[_pib.selectedIndex].value;}else{pageIndex=_pib.value;var r=new RegExp(\"^\\\\s*(\\\\d+)\\\\s*$\");if(!r.test(pageIndex)){alert(\"%InvalidPageIndexErrorMessage%\");return;}else if(RegExp.$1<1||RegExp.$1>_mp){alert(\"%PageIndexOutOfRangeErrorMessage%\");return;}}var _hl=document.getElementById(_pib.id+'link').childNodes[0];var _lh=_hl.href;_hl.href=_lh.replace('*_MvcPager_PageIndex_*',pageIndex);if(_hl.click){_hl.click();}else{var evt=document.createEvent('MouseEvents');evt.initEvent('click',true,true);_hl.dispatchEvent(evt);}_hl.href=_lh;}";
        private const string KeyDownScript = "function _MvcPager_Keydown(e){var _kc,_pib;if(window.event){_kc=e.keyCode;_pib=e.srcElement;}else if(e.which){_kc=e.which;_pib=e.target;}var validKey=(_kc==8||_kc==46||_kc==37||_kc==39||(_kc>=48&&_kc<=57)||(_kc>=96&&_kc<=105));if(!validKey){if(_kc==13){ _MvcPager_GoToPage(_pib,%TotalPageCount%);}if(e.preventDefault){e.preventDefault();}else{event.returnValue=false;}}}";
        private const string ScriptPageIndexName = "*_MvcPager_PageIndex_*";

        internal PagerBuilder(HtmlHelper html, AjaxHelper ajax, PagerOptions pagerOptions, IDictionary<string, object> htmlAttributes)
        {
            _totalPageCount = 1;
            _startPageIndex = 1;
            _endPageIndex = 1;
            if (pagerOptions == null)
            {
                pagerOptions = new PagerOptions();
            }
            _html = html;
            _ajax = ajax;
            _pagerOptions = pagerOptions;
            _htmlAttributes = htmlAttributes;
        }

        internal PagerBuilder(HtmlHelper helper, string actionName, string controllerName, int totalPageCount, int pageIndex, int totalItemCount, PagerOptions pagerOptions, string routeName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes) : this(helper, null, actionName, controllerName, totalPageCount, pageIndex, totalItemCount, pagerOptions, routeName, routeValues, null, htmlAttributes)
        {
        }

        internal PagerBuilder(AjaxHelper helper, string actionName, string controllerName, int totalPageCount, int pageIndex, int totalItemCount, PagerOptions pagerOptions, string routeName, RouteValueDictionary routeValues, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes) : this(null, helper, actionName, controllerName, totalPageCount, pageIndex, totalItemCount, pagerOptions, routeName, routeValues, ajaxOptions, htmlAttributes)
        {
        }

        internal PagerBuilder(HtmlHelper helper, string actionName, string controllerName, int totalPageCount, int pageIndex, int totalItemCount, PagerOptions pagerOptions, string routeName, RouteValueDictionary routeValues, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes) : this(helper, null, actionName, controllerName, totalPageCount, pageIndex, totalItemCount, pagerOptions, routeName, routeValues, ajaxOptions, htmlAttributes)
        {
        }

        internal PagerBuilder(HtmlHelper html, AjaxHelper ajax, string actionName, string controllerName, int totalPageCount, int pageIndex, int totalItemCount, PagerOptions pagerOptions, string routeName, RouteValueDictionary routeValues, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
        {
            _totalPageCount = 1;
            _startPageIndex = 1;
            _endPageIndex = 1;
            _msAjaxPaging = ajax != null;
            if (string.IsNullOrEmpty(actionName))
            {
                if (ajax != null)
                {
                    actionName = (string) ajax.ViewContext.RouteData.Values["action"];
                }
                else
                {
                    actionName = (string) html.ViewContext.RouteData.Values["action"];
                }
            }
            if (string.IsNullOrEmpty(controllerName))
            {
                if (ajax != null)
                {
                    controllerName = (string) ajax.ViewContext.RouteData.Values["controller"];
                }
                else
                {
                    controllerName = (string) html.ViewContext.RouteData.Values["controller"];
                }
            }
            if (pagerOptions == null)
            {
                pagerOptions = new PagerOptions();
            }
            _html = html;
            _ajax = ajax;
            _actionName = actionName;
            _controllerName = controllerName;
            if ((pagerOptions.MaxPageIndex == 0) || (pagerOptions.MaxPageIndex > totalPageCount))
            {
                _totalPageCount = totalPageCount;
            }
            else
            {
                _totalPageCount = pagerOptions.MaxPageIndex;
            }
            _pageIndex = pageIndex;
            _totalItemCount = totalItemCount;
            _pagerOptions = pagerOptions;
            _routeName = routeName;
            _routeValues = routeValues;
            _ajaxOptions = ajaxOptions;
            _htmlAttributes = htmlAttributes;
            _startPageIndex = pageIndex - (pagerOptions.NumericPagerItemCount / 2);
            if ((_startPageIndex + pagerOptions.NumericPagerItemCount) > _totalPageCount)
            {
                _startPageIndex = (_totalPageCount + 1) - pagerOptions.NumericPagerItemCount;
            }
            if (_startPageIndex < 1)
            {
                _startPageIndex = 1;
            }
            _endPageIndex = (_startPageIndex + _pagerOptions.NumericPagerItemCount) - 1;
            if (_endPageIndex > _totalPageCount)
            {
                _endPageIndex = _totalPageCount;
            }
        }

        private void AddFirst(ICollection<PagerItem> results)
        {
            PagerItem item = new PagerItem(_pagerOptions.FirstPageText, 1, _pageIndex == 1, PagerItemType.FirstPage);
            if (!item.Disabled || (item.Disabled && _pagerOptions.ShowDisabledPagerItems))
            {
                results.Add(item);
            }
        }

        private void AddLast(ICollection<PagerItem> results)
        {
            PagerItem item = new PagerItem(_pagerOptions.LastPageText, _totalPageCount, _pageIndex >= _totalPageCount, PagerItemType.LastPage);
            if (!item.Disabled || (item.Disabled && _pagerOptions.ShowDisabledPagerItems))
            {
                results.Add(item);
            }
        }

        private void AddMoreAfter(ICollection<PagerItem> results)
        {
            if (_endPageIndex < _totalPageCount)
            {
                int pageIndex = _startPageIndex + _pagerOptions.NumericPagerItemCount;
                if (pageIndex > _totalPageCount)
                {
                    pageIndex = _totalPageCount;
                }
                PagerItem item = new PagerItem(_pagerOptions.MorePageText, pageIndex, false, PagerItemType.MorePage);
                results.Add(item);
            }
        }

        private void AddMoreBefore(ICollection<PagerItem> results)
        {
            if ((_startPageIndex > 1) && _pagerOptions.ShowMorePagerItems)
            {
                int pageIndex = _startPageIndex - 1;
                if (pageIndex < 1)
                {
                    pageIndex = 1;
                }
                PagerItem item = new PagerItem(_pagerOptions.MorePageText, pageIndex, false, PagerItemType.MorePage);
                results.Add(item);
            }
        }

        private void AddNext(ICollection<PagerItem> results)
        {
            PagerItem item = new PagerItem(_pagerOptions.NextPageText, _pageIndex + 1, _pageIndex >= _totalPageCount, PagerItemType.NextPage);
            if (!item.Disabled || (item.Disabled && _pagerOptions.ShowDisabledPagerItems))
            {
                results.Add(item);
            }
        }

        private void AddPageNumbers(ICollection<PagerItem> results)
        {
            for (int i = _startPageIndex; i <= _endPageIndex; i++)
            {
                string str = i.ToString();
                if ((i == _pageIndex) && !string.IsNullOrEmpty(_pagerOptions.CurrentPageNumberFormatString))
                {
                    str = string.Format(_pagerOptions.CurrentPageNumberFormatString, str);
                }
                else if (!string.IsNullOrEmpty(_pagerOptions.PageNumberFormatString))
                {
                    str = string.Format(_pagerOptions.PageNumberFormatString, str);
                }
                PagerItem item = new PagerItem(str, i, false, PagerItemType.NumericPage);
                results.Add(item);
            }
        }

        private void AddPrevious(ICollection<PagerItem> results)
        {
            PagerItem item = new PagerItem(_pagerOptions.PrevPageText, _pageIndex - 1, _pageIndex == 1, PagerItemType.PrevPage);
            if (!item.Disabled || (item.Disabled && _pagerOptions.ShowDisabledPagerItems))
            {
                results.Add(item);
            }
        }

        private string BuildGoToPageSection(ref string pagerScript)
        {
            int num;
            ViewContext context = _msAjaxPaging ? _ajax.ViewContext : _html.ViewContext;
            if (int.TryParse((string) context.HttpContext.Items["_MvcPager_ControlIndex"], out num))
            {
                num++;
            }
            context.HttpContext.Items["_MvcPager_ControlIndex"] = num.ToString();
            string str = "_MvcPager_Ctrl" + num;
            string str2 = GenerateAnchor(new PagerItem("0", 0, false, PagerItemType.NumericPage));
            if (num == 0)
            {
                pagerScript = pagerScript + "function _MvcPager_Keydown(e){var _kc,_pib;if(window.event){_kc=e.keyCode;_pib=e.srcElement;}else if(e.which){_kc=e.which;_pib=e.target;}var validKey=(_kc==8||_kc==46||_kc==37||_kc==39||(_kc>=48&&_kc<=57)||(_kc>=96&&_kc<=105));if(!validKey){if(_kc==13){ _MvcPager_GoToPage(_pib,%TotalPageCount%);}if(e.preventDefault){e.preventDefault();}else{event.returnValue=false;}}}".Replace("%TotalPageCount%", _totalPageCount.ToString()) + "function _MvcPager_GoToPage(_pib,_mp){var pageIndex;if(_pib.tagName==\"SELECT\"){pageIndex=_pib.options[_pib.selectedIndex].value;}else{pageIndex=_pib.value;var r=new RegExp(\"^\\\\s*(\\\\d+)\\\\s*$\");if(!r.test(pageIndex)){alert(\"%InvalidPageIndexErrorMessage%\");return;}else if(RegExp.$1<1||RegExp.$1>_mp){alert(\"%PageIndexOutOfRangeErrorMessage%\");return;}}var _hl=document.getElementById(_pib.id+'link').childNodes[0];var _lh=_hl.href;_hl.href=_lh.replace('*_MvcPager_PageIndex_*',pageIndex);if(_hl.click){_hl.click();}else{var evt=document.createEvent('MouseEvents');evt.initEvent('click',true,true);_hl.dispatchEvent(evt);}_hl.href=_lh;}".Replace("%InvalidPageIndexErrorMessage%", _pagerOptions.InvalidPageIndexErrorMessage).Replace("%PageIndexOutOfRangeErrorMessage%", _pagerOptions.PageIndexOutOfRangeErrorMessage);
            }
            string str3 = null;
            if (!_pagerOptions.ShowGoButton)
            {
                str3 = " onchange=\"_MvcPager_GoToPage(this," + _totalPageCount + ")\"";
            }
            StringBuilder builder = new StringBuilder();
            if (_pagerOptions.PageIndexBoxType == PageIndexBoxType.DropDownList)
            {
                int num2 = _pageIndex - (_pagerOptions.MaximumPageIndexItems / 2);
                if ((num2 + _pagerOptions.MaximumPageIndexItems) > _totalPageCount)
                {
                    num2 = (_totalPageCount + 1) - _pagerOptions.MaximumPageIndexItems;
                }
                if (num2 < 1)
                {
                    num2 = 1;
                }
                int num3 = (num2 + _pagerOptions.MaximumPageIndexItems) - 1;
                if (num3 > _totalPageCount)
                {
                    num3 = _totalPageCount;
                }
                builder.AppendFormat("<select id=\"{0}\"{1}>", str + "_pib", str3);
                for (int i = num2; i <= num3; i++)
                {
                    builder.AppendFormat("<option value=\"{0}\"", i);
                    if (i == _pageIndex)
                    {
                        builder.Append(" selected=\"selected\"");
                    }
                    builder.AppendFormat(">{0}</option>", i);
                }
                builder.Append("</select>");
            }
            else
            {
                builder.AppendFormat("<input type=\"text\" id=\"{0}\" value=\"{1}\" onkeydown=\"_MvcPager_Keydown(event)\"{2}/>", str + "_pib", _pageIndex, str3);
            }
            if (!string.IsNullOrEmpty(_pagerOptions.PageIndexBoxWrapperFormatString))
            {
                builder = new StringBuilder(string.Format(_pagerOptions.PageIndexBoxWrapperFormatString, builder));
            }
            if (_pagerOptions.ShowGoButton)
            {
                builder.AppendFormat("<input type=\"button\" value=\"{0}\" onclick=\"_MvcPager_GoToPage(document.getElementById('{1}')," + _totalPageCount + ")\"/>", _pagerOptions.GoButtonText, str + "_pib");
            }
            builder.AppendFormat("<span id=\"{0}\" style=\"display:none;width:0px;height:0px\">{1}</span>", str + "_piblink", str2);
            if (!string.IsNullOrEmpty(_pagerOptions.GoToPageSectionWrapperFormatString) || !string.IsNullOrEmpty(_pagerOptions.PagerItemWrapperFormatString))
            {
                return string.Format(_pagerOptions.GoToPageSectionWrapperFormatString ?? _pagerOptions.PagerItemWrapperFormatString, builder);
            }
            return builder.ToString();
        }

        private string BuildTotalItemCountPageSection(ref string countScript)
        {
            countScript = string.Format("<span>总条数: {0}</span>", _totalItemCount);
            return countScript;
        }

        private MvcHtmlString CreateWrappedPagerElement(PagerItem item, string el)
        {
            string str = el;
            switch (item.Type)
            {
                case PagerItemType.FirstPage:
                case PagerItemType.NextPage:
                case PagerItemType.PrevPage:
                case PagerItemType.LastPage:
                    if (!string.IsNullOrEmpty(_pagerOptions.NavigationPagerItemWrapperFormatString) || !string.IsNullOrEmpty(_pagerOptions.PagerItemWrapperFormatString))
                    {
                        str = string.Format(_pagerOptions.NavigationPagerItemWrapperFormatString ?? _pagerOptions.PagerItemWrapperFormatString, el);
                    }
                    break;

                case PagerItemType.MorePage:
                    if (!string.IsNullOrEmpty(_pagerOptions.MorePagerItemWrapperFormatString) || !string.IsNullOrEmpty(_pagerOptions.PagerItemWrapperFormatString))
                    {
                        str = string.Format(_pagerOptions.MorePagerItemWrapperFormatString ?? _pagerOptions.PagerItemWrapperFormatString, el);
                    }
                    break;

                case PagerItemType.NumericPage:
                    if ((item.PageIndex != _pageIndex) || (string.IsNullOrEmpty(_pagerOptions.CurrentPagerItemWrapperFormatString) && string.IsNullOrEmpty(_pagerOptions.PagerItemWrapperFormatString)))
                    {
                        if (!string.IsNullOrEmpty(_pagerOptions.NumericPagerItemWrapperFormatString) || !string.IsNullOrEmpty(_pagerOptions.PagerItemWrapperFormatString))
                        {
                            str = string.Format(_pagerOptions.NumericPagerItemWrapperFormatString ?? _pagerOptions.PagerItemWrapperFormatString, el);
                        }
                        break;
                    }
                    str = string.Format(_pagerOptions.CurrentPagerItemWrapperFormatString ?? _pagerOptions.PagerItemWrapperFormatString, el);
                    break;
            }
            return MvcHtmlString.Create(str + _pagerOptions.SeparatorHtml);
        }

        private string GenerateAnchor(PagerItem item)
        {
            if (_msAjaxPaging)
            {
                RouteValueDictionary currentRouteValues = GetCurrentRouteValues(_ajax.ViewContext);
                if (item.PageIndex == 0)
                {
                    currentRouteValues[_pagerOptions.PageIndexParameterName] = "*_MvcPager_PageIndex_*";
                }
                else
                {
                    currentRouteValues[_pagerOptions.PageIndexParameterName] = item.PageIndex;
                }
                if (!string.IsNullOrEmpty(_routeName))
                {
                    return _ajax.RouteLink(item.Text, _routeName, currentRouteValues, _ajaxOptions).ToString();
                }
                return _ajax.RouteLink(item.Text, currentRouteValues, _ajaxOptions).ToString();
            }
            string str = GenerateUrl(item.PageIndex);
            if (!_pagerOptions.UseJqueryAjax)
            {
                return ("<a href=\"" + str + "\" onclick=\"window.open(this.attributes.getNamedItem('href').value,'_self')\"></a>");
            }
            if (_html.ViewContext.UnobtrusiveJavaScriptEnabled)
            {
                TagBuilder builder = new TagBuilder("a") {
                    InnerHtml = item.Text
                };
                builder.MergeAttribute("href", str);
                builder.MergeAttribute("p", item.PageIndex.ToString());
                builder.MergeAttributes(_ajaxOptions.ToUnobtrusiveHtmlAttributes());
                if (!string.IsNullOrEmpty(str))
                {
                    return builder.ToString(TagRenderMode.Normal);
                }
                return _html.Encode(item.Text);
            }
            StringBuilder builder3 = new StringBuilder();
            if ((!string.IsNullOrEmpty(_ajaxOptions.OnFailure) || !string.IsNullOrEmpty(_ajaxOptions.OnBegin)) || (!string.IsNullOrEmpty(_ajaxOptions.OnComplete) && (_ajaxOptions.HttpMethod.ToUpper() != "GET")))
            {
                builder3.Append("$.ajax({type:'").Append((_ajaxOptions.HttpMethod.ToUpper() == "GET") ? "get" : "post");
                builder3.Append("',url:$(this).attr('href'),success:function(data,status,xhr){$('#");
                builder3.Append(_ajaxOptions.UpdateTargetId).Append("').html(data);}");
                if (!string.IsNullOrEmpty(_ajaxOptions.OnFailure))
                {
                    builder3.Append(",error:").Append(HttpUtility.HtmlAttributeEncode(_ajaxOptions.OnFailure));
                }
                if (!string.IsNullOrEmpty(_ajaxOptions.OnBegin))
                {
                    builder3.Append(",beforeSend:").Append(HttpUtility.HtmlAttributeEncode(_ajaxOptions.OnBegin));
                }
                if (!string.IsNullOrEmpty(_ajaxOptions.OnComplete))
                {
                    builder3.Append(",complete:").Append(HttpUtility.HtmlAttributeEncode(_ajaxOptions.OnComplete));
                }
                builder3.Append("});return false;");
            }
            else if (_ajaxOptions.HttpMethod.ToUpper() == "GET")
            {
                builder3.Append("$('#").Append(_ajaxOptions.UpdateTargetId);
                builder3.Append("').load($(this).attr('href')");
                if (!string.IsNullOrEmpty(_ajaxOptions.OnComplete))
                {
                    builder3.Append(",").Append(HttpUtility.HtmlAttributeEncode(_ajaxOptions.OnComplete));
                }
                builder3.Append(");return false;");
            }
            else
            {
                builder3.Append("$.post($(this).attr('href'), function(data) {$('#");
                builder3.Append(_ajaxOptions.UpdateTargetId);
                builder3.Append("').html(data);});return false;");
            }
            if (!string.IsNullOrEmpty(str))
            {
                return string.Format(CultureInfo.InvariantCulture, "<a href=\"{0}\" onclick=\"{1}\">{2}</a>", str, builder3, item.Text);
            }
            return _html.Encode(item.Text);
        }

        private MvcHtmlString GenerateJqAjaxPagerElement(PagerItem item)
        {
            if (item.Disabled)
            {
                return CreateWrappedPagerElement(item, string.Format("<a disabled=\"disabled\">{0}</a>", item.Text));
            }
            return CreateWrappedPagerElement(item, GenerateAnchor(item));
        }

        private MvcHtmlString GenerateMsAjaxPagerElement(PagerItem item)
        {
            if ((item.PageIndex == _pageIndex) && !item.Disabled)
            {
                return CreateWrappedPagerElement(item, item.Text);
            }
            if (item.Disabled)
            {
                return CreateWrappedPagerElement(item, string.Format("<a disabled=\"disabled\">{0}</a>", item.Text));
            }
            if ((item.PageIndex >= 1) && (item.PageIndex <= _totalPageCount))
            {
                return CreateWrappedPagerElement(item, GenerateAnchor(item));
            }
            return null;
        }

        private MvcHtmlString GeneratePagerElement(PagerItem item)
        {
            string str = GenerateUrl(item.PageIndex);
            if (item.Disabled)
            {
                return CreateWrappedPagerElement(item, string.Format("<a disabled=\"disabled\">{0}</a>", item.Text));
            }
            return CreateWrappedPagerElement(item, string.IsNullOrEmpty(str) ? _html.Encode(item.Text) : string.Format("<a href='{0}'>{1}</a>", str, item.Text));
        }

        private string GenerateUrl(int pageIndex)
        {
            if ((pageIndex > _totalPageCount) || (pageIndex == _pageIndex))
            {
                return null;
            }
            RouteValueDictionary currentRouteValues = GetCurrentRouteValues(_html.ViewContext);
            if (pageIndex == 0)
            {
                currentRouteValues[_pagerOptions.PageIndexParameterName] = "*_MvcPager_PageIndex_*";
            }
            else if ((pageIndex == 1) && _pagerOptions.SEOForFirstPage)
            {
                currentRouteValues[_pagerOptions.PageIndexParameterName] = string.Empty;
            }
            else
            {
                currentRouteValues[_pagerOptions.PageIndexParameterName] = pageIndex;
            }
            UrlHelper helper = new UrlHelper(_html.ViewContext.RequestContext);
            if (!string.IsNullOrEmpty(_routeName))
            {
                return helper.RouteUrl(_routeName, currentRouteValues);
            }
            return helper.RouteUrl(currentRouteValues);
        }

        private RouteValueDictionary GetCurrentRouteValues(ViewContext viewContext)
        {
            RouteValueDictionary dictionary = _routeValues ?? new RouteValueDictionary();
            NameValueCollection queryString = viewContext.HttpContext.Request.QueryString;
            if ((queryString != null) && (queryString.Count > 0))
            {
                string[] array = { "x-requested-with", "xmlhttprequest", _pagerOptions.PageIndexParameterName.ToLower() };
                foreach (string str in queryString.Keys)
                {
                    if (!string.IsNullOrEmpty(str) && (Array.IndexOf(array, str.ToLower()) < 0))
                    {
                        dictionary[str] = queryString[str];
                    }
                }
            }
            dictionary["action"] = _actionName;
            dictionary["controller"] = _controllerName;
            return dictionary;
        }

        internal MvcHtmlString RenderPager()
        {
            if ((_totalPageCount <= 1) && _pagerOptions.AutoHide)
            {
                return MvcHtmlString.Create("\r\n<!--MvcPager 1.5 for ASP.NET MVC 3.0 \x00a9 2009-2011 Webdiyer (http://www.webdiyer.com)-->\r\n");
            }
            if (((_pageIndex > _totalPageCount) && (_totalPageCount > 0)) || (_pageIndex < 1))
            {
                return MvcHtmlString.Create(string.Format("<div style=\"color:red;font-weight:bold\">{1} <a href='{0}'>【点击】返回第一页</a></div>", GenerateUrl(1), _pagerOptions.PageIndexOutOfRangeErrorMessage));
            }
            List<PagerItem> results = new List<PagerItem>();
            if (_pagerOptions.ShowFirstLast)
            {
                AddFirst(results);
            }
            if (_pagerOptions.ShowPrevNext)
            {
                AddPrevious(results);
            }
            if (_pagerOptions.ShowNumericPagerItems)
            {
                if (_pagerOptions.AlwaysShowFirstLastPageNumber && (_startPageIndex > 1))
                {
                    results.Add(new PagerItem("1", 1, false, PagerItemType.NumericPage));
                }
                if (_pagerOptions.ShowMorePagerItems)
                {
                    AddMoreBefore(results);
                }
                AddPageNumbers(results);
                if (_pagerOptions.ShowMorePagerItems)
                {
                    AddMoreAfter(results);
                }
                if (_pagerOptions.AlwaysShowFirstLastPageNumber && (_endPageIndex < _totalPageCount))
                {
                    results.Add(new PagerItem(_totalPageCount.ToString(), _totalPageCount, false, PagerItemType.NumericPage));
                }
            }
            if (_pagerOptions.ShowPrevNext)
            {
                AddNext(results);
            }
            if (_pagerOptions.ShowFirstLast)
            {
                AddLast(results);
            }
            StringBuilder builder = new StringBuilder();
            if (_msAjaxPaging)
            {
                foreach (PagerItem item in results)
                {
                    builder.Append(GenerateMsAjaxPagerElement(item));
                }
            }
            else if (_pagerOptions.UseJqueryAjax)
            {
                foreach (PagerItem item2 in results)
                {
                    builder.Append(GenerateJqAjaxPagerElement(item2));
                }
            }
            else
            {
                foreach (PagerItem item3 in results)
                {
                    builder.Append(GeneratePagerElement(item3));
                }
            }
            TagBuilder builder2 = new TagBuilder(_pagerOptions.ContainerTagName);
            if (!string.IsNullOrEmpty(_pagerOptions.Id))
            {
                builder2.GenerateId(_pagerOptions.Id);
            }
            if (!string.IsNullOrEmpty(_pagerOptions.CssClass))
            {
                builder2.AddCssClass(_pagerOptions.CssClass);
            }
            if (!string.IsNullOrEmpty(_pagerOptions.HorizontalAlign))
            {
                string str = "text-align:" + _pagerOptions.HorizontalAlign.ToLower();
                if (_htmlAttributes == null)
                {
                    RouteValueDictionary dictionary = new RouteValueDictionary();
                    dictionary.Add("style", str);
                    _htmlAttributes = dictionary;
                }
                else if (_htmlAttributes.Keys.Contains("style"))
                {
                    IDictionary<string, object> dictionary2;
                    (dictionary2 = _htmlAttributes)["style"] = dictionary2["style"] + ";" + str;
                }
            }
            builder2.MergeAttributes(_htmlAttributes, true);
            string pagerScript = string.Empty;
            if (_pagerOptions.ShowPageIndexBox)
            {
                builder.Append(BuildGoToPageSection(ref pagerScript));
            }
            else
            {
                builder.Length -= _pagerOptions.SeparatorHtml.Length;
            }
            string countScript = string.Empty;
            if (_pagerOptions.ShowTotalItemCount)
            {
                builder.Append(BuildTotalItemCountPageSection(ref countScript));
            }
            builder2.InnerHtml = builder.ToString();
            if (!string.IsNullOrEmpty(pagerScript))
            {
                pagerScript = "<script type=\"text/javascript\">//<![CDATA[\r\n" + pagerScript + "\r\n//]]>\r\n</script>";
            }
            return MvcHtmlString.Create("\r\n<!--MvcPager 1.5 for ASP.NET MVC 3.0 \x00a9 2009-2011 Webdiyer (http://www.webdiyer.com)-->\r\n" + pagerScript + builder2.ToString(TagRenderMode.Normal) + "\r\n<!--MvcPager 1.5 for ASP.NET MVC 3.0 \x00a9 2009-2011 Webdiyer (http://www.webdiyer.com)-->\r\n");
        }
    }
}

