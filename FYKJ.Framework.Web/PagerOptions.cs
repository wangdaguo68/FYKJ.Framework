using System;

namespace FYKJ.Framework.Web.Controls
{
    public class PagerOptions
    {
        private string _containerTagName;

        public PagerOptions()
        {
            AutoHide = true;
            PageIndexParameterName = "pageIndex";
            NumericPagerItemCount = 10;
            AlwaysShowFirstLastPageNumber = false;
            ShowPrevNext = true;
            PrevPageText = "上一页";
            NextPageText = "下一页";
            ShowNumericPagerItems = true;
            ShowFirstLast = true;
            FirstPageText = "首页";
            LastPageText = "尾页";
            ShowMorePagerItems = true;
            MorePageText = "...";
            ShowDisabledPagerItems = true;
            SeparatorHtml = "&nbsp;&nbsp;";
            UseJqueryAjax = false;
            ShowPageIndexBox = false;
            ShowGoButton = true;
            PageIndexBoxType = PageIndexBoxType.TextBox;
            MaximumPageIndexItems = 80;
            GoButtonText = "跳转";
            ContainerTagName = "div";
            InvalidPageIndexErrorMessage = "页索引无效";
            PageIndexOutOfRangeErrorMessage = "页索引超出范围";
            MaxPageIndex = 0;
            CssClass = "pages";
            ShowTotalItemCount = false;
        }

        public bool AlwaysShowFirstLastPageNumber { get; set; }

        public bool AutoHide { get; set; }

        public string ContainerTagName
        {
            get
            {
                return _containerTagName;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("ContainerTagName不能为null或空字符串", "ContainerTagName");
                }
                _containerTagName = value;
            }
        }

        public string CssClass { get; set; }

        public string CurrentPageNumberFormatString { get; set; }

        public string CurrentPagerItemWrapperFormatString { get; set; }

        public string FirstPageText { get; set; }

        public string GoButtonText { get; set; }

        public string GoToPageSectionWrapperFormatString { get; set; }

        public string HorizontalAlign { get; set; }

        public string Id { get; set; }

        public string InvalidPageIndexErrorMessage { get; set; }

        public string LastPageText { get; set; }

        public int MaximumPageIndexItems { get; set; }

        public int MaxPageIndex { get; set; }

        public string MorePagerItemWrapperFormatString { get; set; }

        public string MorePageText { get; set; }

        public string NavigationPagerItemWrapperFormatString { get; set; }

        public string NextPageText { get; set; }

        public int NumericPagerItemCount { get; set; }

        public string NumericPagerItemWrapperFormatString { get; set; }

        public PageIndexBoxType PageIndexBoxType { get; set; }

        public string PageIndexBoxWrapperFormatString { get; set; }

        public string PageIndexOutOfRangeErrorMessage { get; set; }

        public string PageIndexParameterName { get; set; }

        public string PageNumberFormatString { get; set; }

        public string PagerItemWrapperFormatString { get; set; }

        public string PrevPageText { get; set; }

        public bool SEOForFirstPage { get; set; }

        public string SeparatorHtml { get; set; }

        public bool ShowDisabledPagerItems { get; set; }

        public bool ShowFirstLast { get; set; }

        public bool ShowGoButton { get; set; }

        public bool ShowMorePagerItems { get; set; }

        public bool ShowNumericPagerItems { get; set; }

        public bool ShowPageIndexBox { get; set; }

        public bool ShowPrevNext { get; set; }

        public bool ShowTotalItemCount { get; set; }

        internal bool UseJqueryAjax { get; set; }
    }
}

