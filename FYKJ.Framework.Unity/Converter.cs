using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace FYKJ.Framework.Utility
{
    public static class Converter
    {
        public static DateTime CutOff(this DateTime dateTime, long cutTicks = 0x989680L)
        {
            return new DateTime(dateTime.Ticks - (dateTime.Ticks % cutTicks), dateTime.Kind);
        }

        public static bool ToBool(this string s, bool defalut = false)
        {
            bool result = defalut;
            if (bool.TryParse(s, out result))
            {
                return result;
            }
            return defalut;
        }

        public static string ToCnDataString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        public static string ToCnDataString(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ToCnDataString();
            }
            return string.Empty;
        }

        public static string ToCnDayPrice(this decimal price, string format = "0.00")
        {
            if (price < 0M)
            {
                return "暂无报价";
            }
            return string.Format("&yen;{0}/晚起", price.ToString(format));
        }

        public static string ToCnPrice(this decimal price, string format = "0.00")
        {
            if (price < 0M)
            {
                return "暂无报价";
            }
            return string.Format("&yen;{0}", price.ToString(format));
        }

        public static DateTime ToDateTime(this string s, DateTime defalut = new DateTime())
        {
            DateTime result = defalut;
            if (DateTime.TryParse(s, out result))
            {
                return result;
            }
            return defalut;
        }

        public static string ToDay(this DateTime date)
        {
            int day = DateTime.Now.Day;
            if (day == date.Day)
            {
                return "今天";
            }
            if ((day - date.Day) == 1)
            {
                return "昨天";
            }
            return date.ToString("yyyy-MM-dd");
        }

        public static decimal ToDecimal(this string s, [Optional, DecimalConstant(0, 0, 0, 0, (uint) 0)] decimal defalut)
        {
            decimal result = defalut;
            if (decimal.TryParse(s, out result))
            {
                return result;
            }
            return defalut;
        }

        public static double ToDouble(this string s, double defalut = 0.0)
        {
            double result = defalut;
            if (double.TryParse(s, out result))
            {
                return result;
            }
            return defalut;
        }

        public static T ToEnum<T>(this string s) where T: struct
        {
            T result = default(T);
            Enum.TryParse(s, true, out result);
            return result;
        }

        public static Guid ToGuid(this string s)
        {
            var empty = Guid.Empty;
            return Guid.TryParse(s, out empty) ? empty : Guid.Empty;
        }

        public static int ToInt(this decimal value)
        {
            decimal num = value - ((int) value);
            if (num >= 0.5M)
            {
                return (((int) value) + 1);
            }
            return (int) value;
        }

        public static int ToInt(this double value)
        {
            return ((decimal) value).ToInt();
        }

        public static int ToInt(this string s, int defalut = 0)
        {
            int result = defalut;
            if (int.TryParse(s, out result))
            {
                return result;
            }
            return defalut;
        }

        public static string ToPrice(this decimal price, string format = "0.00")
        {
            return price.ToString(format);
        }

        public static Tuple<int, int> ToPriceRange(this string priceParam)
        {
            if (priceParam.Contains("-"))
            {
                string[] strArray = priceParam.Split('-');
                if (strArray.Length == 2)
                {
                    return new Tuple<int, int>(strArray[0].ToInt(), strArray[1].ToInt());
                }
            }
            return new Tuple<int, int>(0, 0);
        }

        public static double ToScore(this double score)
        {
            double num = score - ((int) score);
            if ((0.0 < num) && (num <= 0.5))
            {
                return (((int) score) + 0.5);
            }
            if ((0.0 < num) && (num > 0.5))
            {
                return ((int) score) + 1;
            }
            return score;
        }

        public static string ToShortPrice(this decimal price, int decimalPlaces = 0)
        {
            if (price < 0M)
            {
                return "暂无价格";
            }
            return price.ToString("f" + decimalPlaces);
        }

        public static string ToShortPriceRange(this decimal fromPrice, decimal toPrice)
        {
            if (fromPrice == toPrice)
            {
                return fromPrice.ToShortPrice();
            }
            return string.Format("{0}-{1}", fromPrice.ToShortPrice(), toPrice.ToShortPrice());
        }

        public static string ToStar(this string s, int start = 1)
        {
            StringBuilder builder = new StringBuilder();
            if (string.IsNullOrWhiteSpace(s))
            {
                return "*";
            }
            char ch = s[0];
            if (('A' < ch) && (ch < 'z'))
            {
                string[] strArray = s.Split(' ');
                if ((strArray.Length > 1) && (strArray[0].Length <= 10))
                {
                    builder.Append(strArray[0]);
                    if (!string.IsNullOrWhiteSpace(strArray[1]))
                    {
                        builder.Append(" ");
                        builder.Append(strArray[1].Substring(0, 1).ToUpper());
                    }
                    else
                    {
                        builder.Append("*");
                    }
                }
                else
                {
                    string str = strArray[0];
                    if (str.Length > 10)
                    {
                        str = s.Substring(0, 10);
                    }
                    builder.Append(str);
                    builder.Append("*");
                }
            }
            else
            {
                string str2 = s.Substring(0, start);
                builder.Append(str2);
                builder.Append("**");
            }
            return builder.ToString();
        }

        public static string ToWeek(this string date)
        {
            int index = Convert.ToInt32(date.ToDateTime().DayOfWeek);
            string[] strArray = { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            return strArray[index];
        }
    }
}

