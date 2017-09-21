using System;
using System.Text;
using System.Text.RegularExpressions;

namespace FYKJ.Framework.Utility
{
    public class StringUtil
    {
        public static string CutRightString(string inputString, int len)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                return string.Empty;
            }
            return Reverse(CutString(Reverse(inputString), len));
        }

        public static string CutString(string inputString, int len)
        {
            if ((inputString == null) || (inputString == ""))
            {
                return "";
            }
            inputString = inputString.Trim();
            if (Encoding.Default.GetBytes(inputString).Length <= len)
            {
                return inputString;
            }
            string s = "";
            for (int i = 0; i < inputString.Length; i++)
            {
                if (Encoding.Default.GetBytes(s).Length >= len)
                {
                    break;
                }
                s = s + inputString.Substring(i, 1);
            }
            return (s + "...");
        }

        public static string CutString(string inputString, int len, string end)
        {
            inputString = inputString.Trim();
            if (Encoding.Default.GetBytes(inputString).Length <= len)
            {
                return inputString;
            }
            string s = "";
            for (int i = 0; i < inputString.Length; i++)
            {
                if (Encoding.Default.GetBytes(s).Length >= len)
                {
                    break;
                }
                s = s + inputString.Substring(i, 1);
            }
            return (s + end);
        }

        public static string DeleteUnVisibleChar(string sourceString)
        {
            StringBuilder builder = new StringBuilder(0x83);
            for (int i = 0; i < sourceString.Length; i++)
            {
                int num2 = sourceString[i];
                if (num2 >= 0x10)
                {
                    builder.Append(sourceString[i].ToString());
                }
            }
            return builder.ToString();
        }

        public static string DelLastComma(string origin)
        {
            if (origin.IndexOf(",", StringComparison.Ordinal) == -1)
            {
                return origin;
            }
            return origin.Substring(0, origin.LastIndexOf(",", StringComparison.Ordinal));
        }

        public static string GetArrayString(string[] stringArray)
        {
            string str = null;
            for (int i = 0; i < stringArray.Length; i++)
            {
                str = str + stringArray[i];
            }
            return str;
        }

        public static int GetByteCount(string strTmp)
        {
            int num = 0;
            for (int i = 0; i < strTmp.Length; i++)
            {
                if (Encoding.UTF8.GetByteCount(strTmp.Substring(i, 1)) == 3)
                {
                    num += 2;
                }
                else
                {
                    num++;
                }
            }
            return num;
        }

        public static int GetByteIndex(int intIns, string strTmp)
        {
            int num = 0;
            if (strTmp.Trim() == "")
            {
                return intIns;
            }
            for (int i = 0; i < strTmp.Length; i++)
            {
                if (Encoding.UTF8.GetByteCount(strTmp.Substring(i, 1)) == 3)
                {
                    num += 2;
                }
                else
                {
                    num++;
                }
                if (num >= intIns)
                {
                    return (i + 1);
                }
            }
            return num;
        }

        public static int GetStringCount(string[] stringArray, string findString)
        {
            int num = -1;
            string arrayString = GetArrayString(stringArray);
            string str2 = arrayString;
            while (str2.IndexOf(findString, StringComparison.Ordinal) >= 0)
            {
                str2 = arrayString.Substring(str2.IndexOf(findString, StringComparison.Ordinal));
                num++;
            }
            return num;
        }

        public static int GetStringCount(string sourceString, string findString)
        {
            int num = 0;
            int length = findString.Length;
            string str = sourceString;
            while (str.IndexOf(findString, StringComparison.Ordinal) >= 0)
            {
                str = str.Substring(str.IndexOf(findString, StringComparison.Ordinal) + length);
                num++;
            }
            return num;
        }

        public static string GetSubString(string sourceString, string startString)
        {
            try
            {
                int index = sourceString.ToUpper().IndexOf(startString, StringComparison.Ordinal);
                if (index > 0)
                {
                    return sourceString.Substring(index);
                }
                return sourceString;
            }
            catch
            {
                return "";
            }
        }

        public static string GetSubString(string sourceString, string beginRemovedString, string endRemovedString)
        {
            try
            {
                if (sourceString.IndexOf(beginRemovedString, StringComparison.Ordinal) != 0)
                {
                    beginRemovedString = "";
                }
                if (sourceString.LastIndexOf(endRemovedString, sourceString.Length - endRemovedString.Length, StringComparison.Ordinal) < 0)
                {
                    endRemovedString = "";
                }
                int length = beginRemovedString.Length;
                int num2 = (sourceString.Length - beginRemovedString.Length) - endRemovedString.Length;
                if (num2 > 0)
                {
                    return sourceString.Substring(length, num2);
                }
                return sourceString;
            }
            catch
            {
                return sourceString;
            }
        }

        public static string HtmlEncode(string str, bool encodeBlank = true)
        {
            if ((str == "") || (str == null))
            {
                return "";
            }
            StringBuilder builder = new StringBuilder(str);
            builder.Replace("&", "&amp;");
            builder.Replace("<", "&lt;");
            builder.Replace(">", "&gt;");
            builder.Replace("\"", "&quot;");
            builder.Replace("'", "&#39;");
            builder.Replace("\t", "&nbsp; &nbsp; ");
            if (encodeBlank)
            {
                builder.Replace(" ", "&nbsp;");
            }
            builder.Replace("\r", "");
            builder.Replace("\n\n", "<p><br/></p>");
            builder.Replace("\n", "<br />");
            return builder.ToString();
        }

        public static string LeftSplit(string sourceString, char splitChar)
        {
            string str = null;
            string[] strArray = sourceString.Split(splitChar);
            if (strArray.Length > 0)
            {
                str = strArray[0];
            }
            return str;
        }

        public static string Remove(string sourceString, string removedString)
        {
            try
            {
                if (sourceString.IndexOf(removedString, StringComparison.Ordinal) < 0)
                {
                    throw new Exception("原字符串中不包含移除字符串！");
                }
                string str = sourceString;
                int length = sourceString.Length;
                int count = removedString.Length;
                int startIndex = length - count;
                if (sourceString.Substring(startIndex).ToUpper() == removedString.ToUpper())
                {
                    str = sourceString.Remove(startIndex, count);
                }
                return str;
            }
            catch
            {
                return sourceString;
            }
        }

        public static string RemoveHtml(string inputString)
        {
            return Regex.Replace(inputString, "<[^>]+>", "");
        }

        public static string Reverse(string s)
        {
            var array = s.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }

        public static string RightSplit(string sourceString, char splitChar)
        {
            string str = null;
            string[] strArray = sourceString.Split(splitChar);
            if (strArray.Length > 0)
            {
                str = strArray[strArray.Length - 1];
            }
            return str;
        }

        public static string TextEncode(string s)
        {
            StringBuilder builder = new StringBuilder(s);
            builder.Replace("&", "&amp;");
            builder.Replace("<", "&lt;");
            builder.Replace(">", "&gt;");
            builder.Replace("\"", "&quot;");
            builder.Replace("'", "&#39;");
            return builder.ToString();
        }

        public static string ToDBC(string input)
        {
            char[] chArray = input.ToCharArray();
            for (int i = 0; i < chArray.Length; i++)
            {
                if (chArray[i] == '　')
                {
                    chArray[i] = ' ';
                }
                else if ((chArray[i] > 0xff00) && (chArray[i] < 0xff5f))
                {
                    chArray[i] = (char) (chArray[i] - 0xfee0);
                }
            }
            return new string(chArray);
        }

        public static string ToSBC(string input)
        {
            char[] chArray = input.ToCharArray();
            for (int i = 0; i < chArray.Length; i++)
            {
                if (chArray[i] == ' ')
                {
                    chArray[i] = '　';
                }
                else if (chArray[i] < '\x007f')
                {
                    chArray[i] = (char) (chArray[i] + 0xfee0);
                }
            }
            return new string(chArray);
        }
    }
}

