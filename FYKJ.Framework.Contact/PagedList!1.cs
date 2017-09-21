namespace FYKJ.Framework.Contract
{
    using System;
    using System.Collections.Generic;

    public class PagedList<T> : List<T>, IPagedList
    {
        public PagedList(IList<T> items, int pageIndex, int pageSize)
        {
            PageSize = pageSize;
            TotalItemCount = items.Count;
            CurrentPageIndex = pageIndex;
            for (var i = StartRecordIndex - 1; i < EndRecordIndex; i++)
            {
                Add(items[i]);
            }
        }

        public PagedList(IEnumerable<T> items, int pageIndex, int pageSize, int totalItemCount)
        {
            AddRange(items);
            TotalItemCount = totalItemCount;
            CurrentPageIndex = pageIndex;
            PageSize = pageSize;
        }

        public int CurrentPageIndex { get; set; }

        public int EndRecordIndex
        {
            get
            {
                if (TotalItemCount <= (CurrentPageIndex * PageSize))
                {
                    return TotalItemCount;
                }
                return (CurrentPageIndex * PageSize);
            }
        }

        public int ExtraCount { get; set; }

        public int PageSize { get; set; }

        public int StartRecordIndex => (((CurrentPageIndex - 1) * PageSize) + 1);

        public int TotalItemCount { get; set; }

        public int TotalPageCount => (int) Math.Ceiling(TotalItemCount / ((double) PageSize));
    }
}

