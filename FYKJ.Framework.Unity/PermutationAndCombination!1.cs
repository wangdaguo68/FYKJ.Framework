using System.Collections.Generic;

namespace FYKJ.Framework.Utility
{
    public class PermutationAndCombination<T>
    {
        public static List<T[]> GetCombination(T[] t, int n)
        {
            if (t.Length < n)
            {
                return null;
            }
            int[] b = new int[n];
            List<T[]> list = new List<T[]>();
            GetCombination(ref list, t, t.Length, n, b, n);
            return list;
        }

        private static void GetCombination(ref List<T[]> list, T[] t, int n, int m, int[] b, int M)
        {
            for (int i = n; i >= m; i--)
            {
                b[m - 1] = i - 1;
                if (m > 1)
                {
                    GetCombination(ref list, t, i - 1, m - 1, b, M);
                }
                else
                {
                    if (list == null)
                    {
                        list = new List<T[]>();
                    }
                    T[] item = new T[M];
                    for (int j = 0; j < b.Length; j++)
                    {
                        item[j] = t[b[j]];
                    }
                    list.Add(item);
                }
            }
        }

        public static List<T[]> GetPermutation(T[] t)
        {
            return GetPermutation(t, 0, t.Length - 1);
        }

        public static List<T[]> GetPermutation(T[] t, int n)
        {
            if (n > t.Length)
            {
                return null;
            }
            List<T[]> list = new List<T[]>();
            List<T[]> combination = GetCombination(t, n);
            for (int i = 0; i < combination.Count; i++)
            {
                List<T[]> list3 = new List<T[]>();
                GetPermutation(ref list3, combination[i], 0, n - 1);
                list.AddRange(list3);
            }
            return list;
        }

        public static List<T[]> GetPermutation(T[] t, int startIndex, int endIndex)
        {
            if ((startIndex < 0) || (endIndex > (t.Length - 1)))
            {
                return null;
            }
            List<T[]> list = new List<T[]>();
            GetPermutation(ref list, t, startIndex, endIndex);
            return list;
        }

        private static void GetPermutation(ref List<T[]> list, T[] t, int startIndex, int endIndex)
        {
            if (startIndex == endIndex)
            {
                if (list == null)
                {
                    list = new List<T[]>();
                }
                T[] array = new T[t.Length];
                t.CopyTo(array, 0);
                list.Add(array);
            }
            else
            {
                for (int i = startIndex; i <= endIndex; i++)
                {
                    Swap(ref t[startIndex], ref t[i]);
                    GetPermutation(ref list, t, startIndex + 1, endIndex);
                    Swap(ref t[startIndex], ref t[i]);
                }
            }
        }

        public static void Swap(ref T a, ref T b)
        {
            T local = a;
            a = b;
            b = local;
        }
    }
}

