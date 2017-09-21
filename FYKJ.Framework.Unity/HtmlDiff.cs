using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FYKJ.Framework.Utility
{
    internal class HtmlDiff
    {
        private readonly StringBuilder content;
        private readonly string newText;
        private string[] newWords;
        private readonly string oldText;
        private string[] oldWords;
        private readonly string[] specialCaseClosingTags = { "</strong>", "</b>", "</i>", "</big>", "</small>", "</u>", "</sub>", "</sup>", "</strike>", "</s>" };
        private readonly string[] specialCaseOpeningTags = { @"<strong[\>\s]+", @"<b[\>\s]+", @"<i[\>\s]+", @"<big[\>\s]+", @"<small[\>\s]+", @"<u[\>\s]+", @"<sub[\>\s]+", @"<sup[\>\s]+", @"<strike[\>\s]+", @"<s[\>\s]+" };
        private readonly bool toWord;
        private Dictionary<string, List<int>> wordIndices;

        public HtmlDiff(string oldText, string newText, bool toWord = true)
        {
            this.oldText = oldText;
            this.newText = newText;
            this.toWord = toWord;
            content = new StringBuilder();
        }

        public string Build()
        {
            SplitInputsToWords();
            IndexNewWords();
            foreach (DiffOperation operation in Operations())
            {
                PerformOperation(operation);
            }
            return content.ToString();
        }

        private string[] ConvertHtmlToListOfWords(string[] characterString)
        {
            DiffMode character = DiffMode.character;
            string item = string.Empty;
            List<string> list = new List<string>();
            foreach (string str2 in characterString)
            {
                switch (character)
                {
                    case DiffMode.character:
                    {
                        if (!IsStartOfTag(str2))
                        {
                            break;
                        }
                        if (item != string.Empty)
                        {
                            list.Add(item);
                        }
                        item = "<";
                        character = DiffMode.tag;
                        continue;
                    }
                    case DiffMode.tag:
                    {
                        if (!IsEndOfTag(str2))
                        {
                            goto Label_00CE;
                        }
                        item = item + ">";
                        list.Add(item);
                        item = "";
                        if (!IsWhiteSpace(str2))
                        {
                            goto Label_00CA;
                        }
                        character = DiffMode.whitespace;
                        continue;
                    }
                    case DiffMode.whitespace:
                    {
                        if (!IsStartOfTag(str2))
                        {
                            goto Label_00FF;
                        }
                        if (item != string.Empty)
                        {
                            list.Add(item);
                        }
                        item = "<";
                        character = DiffMode.tag;
                        continue;
                    }
                    default:
                    {
                        continue;
                    }
                }
                if (Regex.IsMatch(str2, @"\s"))
                {
                    if (item != string.Empty)
                    {
                        list.Add(item);
                    }
                    item = str2;
                    character = DiffMode.whitespace;
                }
                else
                {
                    item = item + str2;
                }
                continue;
            Label_00CA:
                character = DiffMode.character;
                continue;
            Label_00CE:
                item = item + str2;
                continue;
            Label_00FF:
                if (Regex.IsMatch(str2, @"\s"))
                {
                    item = item + str2;
                }
                else
                {
                    if (item != string.Empty)
                    {
                        list.Add(item);
                    }
                    item = str2;
                    character = DiffMode.character;
                }
            }
            if (item != string.Empty)
            {
                list.Add(item);
            }
            return list.ToArray();
        }

        private string[] Explode(string value)
        {
            return Regex.Split(value, "");
        }

        private string[] ExtractConsecutiveWords(List<string> words, Func<string, bool> condition)
        {
            Func<string, int, bool> predicate = null;
            Func<string, int, bool> func2 = null;
            int? indexOfFirstTag = null;
            for (int i = 0; i < words.Count; i++)
            {
                string arg = words[i];
                if (!condition(arg))
                {
                    indexOfFirstTag = i;
                    break;
                }
            }
            if (indexOfFirstTag.HasValue)
            {
                predicate = delegate (string s, int pos) {
                                                             if (pos < 0)
                                                             {
                                                                 return false;
                                                             }
                                                             int num = pos;
                                                             return num < indexOfFirstTag;
                };
                string[] strArray = words.Where(predicate).ToArray();
                if (indexOfFirstTag.Value > 0)
                {
                    words.RemoveRange(0, indexOfFirstTag.Value);
                }
                return strArray;
            }
            func2 = (s, pos) => (pos >= 0) && (pos <= words.Count);
            string[] strArray2 = words.Where(func2).ToArray();
            words.RemoveRange(0, words.Count);
            return strArray2;
        }

        private DiffMatch FindMatch(int startInOld, int endInOld, int startInNew, int endInNew)
        {
            int num = startInOld;
            int num2 = startInNew;
            int size = 0;
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            for (int i = startInOld; i < endInOld; i++)
            {
                Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
                string key = oldWords[i];
                if (!wordIndices.ContainsKey(key))
                {
                    dictionary = dictionary2;
                    continue;
                }
                foreach (int num5 in wordIndices[key])
                {
                    if (num5 >= startInNew)
                    {
                        if (num5 >= endInNew)
                        {
                            break;
                        }
                        int num6 = (dictionary.ContainsKey(num5 - 1) ? dictionary[num5 - 1] : 0) + 1;
                        dictionary2[num5] = num6;
                        if (num6 > size)
                        {
                            num = (i - num6) + 1;
                            num2 = (num5 - num6) + 1;
                            size = num6;
                        }
                    }
                }
                dictionary = dictionary2;
            }
            if (size == 0)
            {
                return null;
            }
            return new DiffMatch(num, num2, size);
        }

        private void FindMatchingBlocks(int startInOld, int endInOld, int startInNew, int endInNew, List<DiffMatch> matchingBlocks)
        {
            DiffMatch item = FindMatch(startInOld, endInOld, startInNew, endInNew);
            if (item != null)
            {
                if ((startInOld < item.StartInOld) && (startInNew < item.StartInNew))
                {
                    FindMatchingBlocks(startInOld, item.StartInOld, startInNew, item.StartInNew, matchingBlocks);
                }
                matchingBlocks.Add(item);
                if ((item.EndInOld < endInOld) && (item.EndInNew < endInNew))
                {
                    FindMatchingBlocks(item.EndInOld, endInOld, item.EndInNew, endInNew, matchingBlocks);
                }
            }
        }

        private void IndexNewWords()
        {
            wordIndices = new Dictionary<string, List<int>>();
            for (int i = 0; i < newWords.Length; i++)
            {
                string key = newWords[i];
                if (wordIndices.ContainsKey(key))
                {
                    wordIndices[key].Add(i);
                }
                else
                {
                    wordIndices[key] = new List<int>();
                    wordIndices[key].Add(i);
                }
            }
        }

        private void InsertTag(string tag, string cssClass, List<string> words)
        {
            Func<string, bool> condition = null;
            Func<string, bool> predicate = null;
            Func<string, bool> func3 = null;
            Func<string, bool> func4 = null;
            while (words.Count != 0)
            {
                if (condition == null)
                {
                    condition = x => !IsTag(x);
                }
                string[] strArray = ExtractConsecutiveWords(words, condition);
                string str = string.Empty;
                bool flag = false;
                if (strArray.Length != 0)
                {
                    string str2 = WrapText(string.Join("", strArray), tag, cssClass);
                    content.Append(str2);
                }
                else
                {
                    if (predicate == null)
                    {
                        predicate = x => Regex.IsMatch(words[0], x);
                    }
                    if (specialCaseOpeningTags.FirstOrDefault(predicate) != null)
                    {
                        str = "<ins class='mod'>";
                        if (tag == "del")
                        {
                            words.RemoveAt(0);
                        }
                    }
                    else if (specialCaseClosingTags.Contains(words[0]))
                    {
                        str = "</ins>";
                        flag = true;
                        if (tag == "del")
                        {
                            words.RemoveAt(0);
                        }
                    }
                }
                if ((words.Count == 0) && (str.Length == 0))
                {
                    return;
                }
                if (flag)
                {
                    if (func3 == null)
                    {
                        func3 = x => IsTag(x);
                    }
                    content.Append(str + string.Join("", ExtractConsecutiveWords(words, func3)));
                }
                else
                {
                    if (func4 == null)
                    {
                        func4 = x => IsTag(x);
                    }
                    content.Append(string.Join("", ExtractConsecutiveWords(words, func4)) + str);
                }
            }
        }

        private bool IsClosingTag(string item)
        {
            return Regex.IsMatch(item, @"^\s*</[^>]+>\s*$");
        }

        private bool IsEndOfTag(string val)
        {
            return (val == ">");
        }

        private bool IsOpeningTag(string item)
        {
            return Regex.IsMatch(item, @"^\s*<[^>]+>\s*$");
        }

        private bool IsStartOfTag(string val)
        {
            return (val == "<");
        }

        private bool IsTag(string item)
        {
            return (IsOpeningTag(item) || IsClosingTag(item));
        }

        private bool IsWhiteSpace(string value)
        {
            return Regex.IsMatch(value, @"\s");
        }

        private List<DiffMatch> MatchingBlocks()
        {
            List<DiffMatch> matchingBlocks = new List<DiffMatch>();
            FindMatchingBlocks(0, oldWords.Length, 0, newWords.Length, matchingBlocks);
            return matchingBlocks;
        }

        private List<DiffOperation> Operations()
        {
            int startInOld = 0;
            int startInNew = 0;
            List<DiffOperation> list = new List<DiffOperation>();
            List<DiffMatch> list2 = MatchingBlocks();
            list2.Add(new DiffMatch(oldWords.Length, newWords.Length, 0));
            for (int i = 0; i < list2.Count; i++)
            {
                DiffMatch match = list2[i];
                bool flag = startInOld == match.StartInOld;
                bool flag2 = startInNew == match.StartInNew;
                DiffAction none = DiffAction.none;
                if (!flag && !flag2)
                {
                    none = DiffAction.replace;
                }
                else if (flag && !flag2)
                {
                    none = DiffAction.insert;
                }
                else if (!flag)
                {
                    none = DiffAction.delete;
                }
                else
                {
                    none = DiffAction.none;
                }
                if (none != DiffAction.none)
                {
                    list.Add(new DiffOperation(none, startInOld, match.StartInOld, startInNew, match.StartInNew));
                }
                if (match.Size != 0)
                {
                    list.Add(new DiffOperation(DiffAction.equal, match.StartInOld, match.EndInOld, match.StartInNew, match.EndInNew));
                }
                startInOld = match.EndInOld;
                startInNew = match.EndInNew;
            }
            return list;
        }

        private void PerformOperation(DiffOperation operation)
        {
            switch (operation.Action)
            {
                case DiffAction.equal:
                    ProcessEqualOperation(operation);
                    return;

                case DiffAction.delete:
                    ProcessDeleteOperation(operation, "diffdel");
                    return;

                case DiffAction.insert:
                    ProcessInsertOperation(operation, "diffins");
                    return;

                case DiffAction.none:
                    break;

                case DiffAction.replace:
                    ProcessReplaceOperation(operation);
                    break;

                default:
                    return;
            }
        }

        private void ProcessDeleteOperation(DiffOperation operation, string cssClass)
        {
            List<string> words = oldWords.Where((s, pos) => ((pos >= operation.StartInOld) && (pos < operation.EndInOld))).ToList();
            InsertTag("del", cssClass, words);
        }

        private void ProcessEqualOperation(DiffOperation operation)
        {
            string[] strArray = newWords.Where((s, pos) => ((pos >= operation.StartInNew) && (pos < operation.EndInNew))).ToArray();
            content.Append(string.Join("", strArray));
        }

        private void ProcessInsertOperation(DiffOperation operation, string cssClass)
        {
            InsertTag("ins", cssClass, newWords.Where((s, pos) => ((pos >= operation.StartInNew) && (pos < operation.EndInNew))).ToList());
        }

        private void ProcessReplaceOperation(DiffOperation operation)
        {
            ProcessDeleteOperation(operation, "diffmod");
            ProcessInsertOperation(operation, "diffmod");
        }

        private void SplitInputsToWords()
        {
            if (toWord)
            {
                oldWords = ConvertHtmlToListOfWords(Explode(oldText));
                newWords = ConvertHtmlToListOfWords(Explode(newText));
            }
            else
            {
                oldWords = Explode(oldText);
                newWords = Explode(newText);
            }
        }

        private string WrapText(string text, string tagName, string cssClass)
        {
            return string.Format("<{0} class='{1}'>{2}</{0}>", tagName, cssClass, text);
        }
    }
}

