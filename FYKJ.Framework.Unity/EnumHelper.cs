using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FYKJ.Framework.Utility
{
    public class EnumHelper
    {
        public static readonly string ENUM_TITLE_SEPARATOR = ",";

        public static string GetAllEnumTitle(Enum e, Enum language = null)
        {
            if (e.Equals(0))
            {
                return "";
            }
            var strArray = e.ToString().Split(ENUM_TITLE_SEPARATOR.ToArray(), StringSplitOptions.RemoveEmptyEntries);
            var type = e.GetType();
            var str = strArray.Select(str2 => type.GetField(str2.Trim())).Where(field => field != null).Select(field => field.GetCustomAttributes(typeof (EnumTitleAttribute), false) as EnumTitleAttribute[]).Where(customAttributes => (customAttributes != null) && (customAttributes.Length > 0)).Aggregate("", (current, customAttributes) => current + customAttributes[0].Title + ENUM_TITLE_SEPARATOR);
            return str.TrimEnd(ENUM_TITLE_SEPARATOR.ToArray());
        }

        public static Dictionary<T, string> GetAllItemList<T>(Enum language = null) where T: struct
        {
            return GetItemValueList<T, T>(true, language);
        }

        public static Dictionary<TKey, string> GetAllItemValueList<T, TKey>(Enum language = null) where T: struct
        {
            return GetItemValueList<T, TKey>(true, language);
        }

        public static string GetDayOfWeekTitle(DayOfWeek day, Enum language = null)
        {
            switch (day)
            {
                case DayOfWeek.Sunday:
                    return "周日";

                case DayOfWeek.Monday:
                    return "周一";

                case DayOfWeek.Tuesday:
                    return "周二";

                case DayOfWeek.Wednesday:
                    return "周三";

                case DayOfWeek.Thursday:
                    return "周四";

                case DayOfWeek.Friday:
                    return "周五";

                case DayOfWeek.Saturday:
                    return "周六";
            }
            return "";
        }

        public static Dictionary<int, string> GetEnumDictionary<TEnum>(IEnumerable<TEnum> exceptTypes = null) where TEnum: struct
        {
            Dictionary<TEnum, string> itemList = GetItemList<TEnum>();
            Dictionary<int, string> dictionary2 = new Dictionary<int, string>();
            foreach (KeyValuePair<TEnum, string> pair in itemList)
            {
                if ((exceptTypes == null) || !exceptTypes.Contains(pair.Key))
                {
                    int hashCode = pair.Key.GetHashCode();
                    dictionary2.Add(hashCode, pair.Value);
                }
            }
            return dictionary2;
        }

        public static string GetEnumTitle(Enum e, Enum language = null)
        {
            if (e.Equals(0))
            {
                return "";
            }
            string[] strArray = e.ToString().Split(ENUM_TITLE_SEPARATOR.ToArray(), StringSplitOptions.RemoveEmptyEntries);
            Type type = e.GetType();
            string str = "";
            foreach (string str2 in strArray)
            {
                FieldInfo field = type.GetField(str2.Trim());
                if (field != null)
                {
                    EnumTitleAttribute[] customAttributes = field.GetCustomAttributes(typeof(EnumTitleAttribute), false) as EnumTitleAttribute[];
                    if (((customAttributes != null) && (customAttributes.Length > 0)) && customAttributes[0].IsDisplay)
                    {
                        str = str + customAttributes[0].Title + ENUM_TITLE_SEPARATOR;
                    }
                }
            }
            return str.TrimEnd(ENUM_TITLE_SEPARATOR.ToArray());
        }

        public static EnumTitleAttribute GetEnumTitleAttribute(Enum e, Enum language = null)
        {
            if (e.Equals(0))
            {
                return null;
            }
            string[] strArray = e.ToString().Split(ENUM_TITLE_SEPARATOR.ToArray(), StringSplitOptions.RemoveEmptyEntries);
            Type type = e.GetType();
            foreach (string str in strArray)
            {
                FieldInfo field = type.GetField(str.Trim());
                if (field != null)
                {
                    EnumTitleAttribute[] customAttributes = field.GetCustomAttributes(typeof(EnumTitleAttribute), false) as EnumTitleAttribute[];
                    if ((customAttributes != null) && (customAttributes.Length > 0))
                    {
                        return customAttributes[0];
                    }
                }
            }
            return null;
        }

        public static Dictionary<T, EnumTitleAttribute> GetItemAttributeList<T>(Enum language = null) where T: struct
        {
            if (!typeof(T).IsEnum)
            {
                throw new Exception("参数必须是枚举！");
            }
            Dictionary<T, EnumTitleAttribute> dictionary = new Dictionary<T, EnumTitleAttribute>();
            foreach (object obj2 in typeof(T).GetEnumValues())
            {
                EnumTitleAttribute enumTitleAttribute = GetEnumTitleAttribute(obj2 as Enum, language);
                if (enumTitleAttribute != null)
                {
                    dictionary.Add((T) obj2, enumTitleAttribute);
                }
            }
            return dictionary;
        }

        public static List<T> GetItemKeyList<T>(Enum language = null) where T: struct
        {
            List<T> list = new List<T>();
            foreach (object obj2 in typeof(T).GetEnumValues())
            {
                list.Add((T) obj2);
            }
            return list;
        }

        public static Dictionary<T, string> GetItemList<T>(Enum language = null) where T: struct
        {
            return GetItemValueList<T, T>(false, language);
        }

        public static Dictionary<int, string> GetItemValueList<T>(Enum language = null) where T: struct
        {
            return GetItemValueList<T, int>(false, language);
        }

        public static Dictionary<TKey, string> GetItemValueList<T, TKey>(bool isAll, Enum language = null) where T: struct
        {
            if (!typeof(T).IsEnum)
            {
                throw new Exception("参数必须是枚举！");
            }
            Dictionary<TKey, string> dictionary = new Dictionary<TKey, string>();
            foreach (KeyValuePair<T, EnumTitleAttribute> pair in from t in GetItemAttributeList<T>()
                orderby t.Value.Order
                select t)
            {
                if (isAll || (pair.Value.IsDisplay && (pair.Key.ToString() != "None")))
                {
                   
                    if ((pair.Key.ToString() == "None") && isAll)
                    {
                        dictionary.Add((TKey)(object)pair.Key, "全部");
                    }
                    else if (!string.IsNullOrEmpty(pair.Value.Title))
                    {
                        dictionary.Add((TKey)(object)pair.Key, pair.Value.Title);
                    }
                }
            }
            return dictionary;
        }

        public static Dictionary<string, T> GetTitleAndSynonyms<T>(Enum language = null) where T: struct
        {
            Dictionary<string, T> dictionary = new Dictionary<string, T>();
            foreach (object obj2 in typeof(T).GetEnumValues())
            {
                FieldInfo field = typeof(T).GetField(obj2.ToString());
                if (field != null)
                {
                    EnumTitleAttribute[] customAttributes = field.GetCustomAttributes(typeof(EnumTitleAttribute), false) as EnumTitleAttribute[];
                    if (((customAttributes != null) && (customAttributes.Length >= 1)) && customAttributes[0].IsDisplay)
                    {
                        if (!dictionary.ContainsKey(customAttributes[0].Title))
                        {
                            dictionary.Add(customAttributes[0].Title, (T) obj2);
                        }
                        if ((customAttributes[0].Synonyms != null) && (customAttributes[0].Synonyms.Length >= 1))
                        {
                            foreach (string str in customAttributes[0].Synonyms)
                            {
                                if (!dictionary.ContainsKey(str))
                                {
                                    dictionary.Add(str, (T) obj2);
                                }
                            }
                        }
                    }
                }
            }
            return dictionary;
        }

        public static T Parse<T>(string obj)
        {
            if (string.IsNullOrEmpty(obj))
            {
                return default(T);
            }
            return (T) Enum.Parse(typeof(T), obj);
        }

        public static T TryParse<T>(string obj, T defT )
        {
            try
            {
                return Parse<T>(obj);
            }
            catch
            {
                return defT;
            }
        }
    }
}

