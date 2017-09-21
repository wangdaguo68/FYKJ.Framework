namespace FYKJ.Framework.Contract
{
    using System.Linq;

    public static class PageLinqExtensions
    {
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> allItems, int pageIndex, int pageSize)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            var count = (pageIndex - 1) * pageSize;
            var items = allItems.Skip(count).Take(pageSize).ToList();
            return new PagedList<T>(items, pageIndex, pageSize, allItems.Count());
        }
    }
}

