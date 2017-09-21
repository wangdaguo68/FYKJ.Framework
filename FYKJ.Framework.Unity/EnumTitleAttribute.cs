using System;

namespace FYKJ.Framework.Utility
{
    public class EnumTitleAttribute : Attribute
    {
        private bool _IsDisplay = true;

        public EnumTitleAttribute(string title, params string[] synonyms)
        {
            Title = title;
            Synonyms = synonyms;
            Order = 0x7fffffff;
        }

        public int Category { get; set; }

        public string Description { get; set; }

        public bool IsDisplay
        {
            get
            {
                return _IsDisplay;
            }
            set
            {
                _IsDisplay = value;
            }
        }

        public string Letter { get; set; }

        public int Order { get; set; }

        public string[] Synonyms { get; set; }

        public string Title { get; set; }
    }
}

