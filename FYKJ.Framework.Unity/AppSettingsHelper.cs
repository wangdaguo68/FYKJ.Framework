using System;
using System.Collections.Specialized;
using System.Configuration;

namespace FYKJ.Framework.Utility
{
    public class AppSettingsHelper
    {
        private static readonly NameValueCollection appSettings = ConfigurationManager.AppSettings;

        private static bool getBoolean(string key, bool? defaultValue)
        {
            return getValue(key, (v, pv) => bool.TryParse(v, out pv), defaultValue);
        }

        public static bool GetBoolean(string key)
        {
            return getBoolean(key, null);
        }

        public static bool GetBoolean(string key, bool defaultValue)
        {
            return getBoolean(key, defaultValue);
        }

        public static bool GetBoolValue(string key)
        {
            bool result = false;
            if (appSettings[key] != null)
            {
                bool.TryParse(appSettings[key], out result);
            }
            return result;
        }

        private static int getInt32(string key, int? defaultValue)
        {
            return getValue(key, (v, pv) => int.TryParse(v, out pv), defaultValue);
        }

        public static int GetInt32(string key)
        {
            return getInt32(key, null);
        }

        public static int GetInt32(string key, int defaultValue)
        {
            return getInt32(key, defaultValue);
        }

        public static int GetIntValue(string key)
        {
            int result = 0;
            if (appSettings[key] != null)
            {
                int.TryParse(appSettings[key], out result);
            }
            return result;
        }

        public static string GetString(string key)
        {
            return getValue(key, true, null);
        }

        public static string GetString(string key, string defaultValue)
        {
            return getValue(key, false, defaultValue);
        }

        private static string[] getStringArray(string key, string separator, bool valueRequired, string[] defaultValue)
        {
            string str = getValue(key, valueRequired, null);
            if (!string.IsNullOrEmpty(str))
            {
                return str.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            }
            if (valueRequired)
            {
                throw new ApplicationException("在配置文件的appSettings节点集合中找不到key为" + key + "的子节点，且没有指定默认值");
            }
            return defaultValue;
        }

        public static string[] GetStringArray(string key, string separator)
        {
            return getStringArray(key, separator, true, null);
        }

        public static string[] GetStringArray(string key, string separator, string[] defaultValue)
        {
            return getStringArray(key, separator, false, defaultValue);
        }

        public static TimeSpan GetTimeSpan(string key)
        {
            return TimeSpan.Parse(getValue(key, true, null));
        }

        public static TimeSpan GetTimeSpan(string key, TimeSpan defaultValue)
        {
            string s = getValue(key, false, null);
            if (s == null)
            {
                return defaultValue;
            }
            return TimeSpan.Parse(s);
        }

        private static string getValue(string key, bool valueRequired, string defaultValue)
        {
            string str = appSettings[key];
            if (str != null)
            {
                return str;
            }
            if (valueRequired)
            {
                throw new ApplicationException("在配置文件的appSettings节点集合中找不到key为" + key + "的子节点");
            }
            return defaultValue;
        }

        private static T getValue<T>(string key, Func<string, T, bool> parseValue, T? defaultValue) where T: struct
        {
            string str = appSettings[key];
            if (str != null)
            {
                T local = default(T);
                if (!parseValue(str, local))
                {
                    throw new ApplicationException(string.Format("子节点 '{0}' 的值 {1} 无效", key, typeof(T).FullName));
                }
                return local;
            }
            if (!defaultValue.HasValue)
            {
                throw new ApplicationException("在配置文件的appSettings节点集合中找不到key为" + key + "的子节点，且没有指定默认值");
            }
            return defaultValue.Value;
        }
    }
}

