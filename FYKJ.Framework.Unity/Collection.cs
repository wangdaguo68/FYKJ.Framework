using System;
using System.Collections.Generic;
using System.Linq;

namespace FYKJ.Framework.Utility
{
    public static class Collection
    {
        public static T Random<T>(this IEnumerable<T> collection)
        {
            return collection.Random(1).SingleOrDefault();
        }

        public static IEnumerable<T> Random<T>(this IEnumerable<T> collection, int count)
        {
            var rd = new Random();
            return (from c in collection
                orderby rd.Next()
                select c).Take(count);
        }
    }
}

