namespace FYKJ.Framework.Utility
{
    public class Diff
    {
        public static string Build(string oldText, string newText, bool toWord = false)
        {
            var diff = new HtmlDiff(oldText ?? string.Empty, newText ?? string.Empty, toWord);
            return diff.Build();
        }
    }
}

