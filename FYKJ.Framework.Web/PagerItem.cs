namespace FYKJ.Framework.Web.Controls
{
    internal class PagerItem
    {
        public PagerItem(string text, int pageIndex, bool disabled, PagerItemType type)
        {
            Text = text;
            PageIndex = pageIndex;
            Disabled = disabled;
            Type = type;
        }

        internal bool Disabled { get; set; }

        internal int PageIndex { get; set; }

        internal string Text { get; set; }

        internal PagerItemType Type { get; set; }
    }
}

