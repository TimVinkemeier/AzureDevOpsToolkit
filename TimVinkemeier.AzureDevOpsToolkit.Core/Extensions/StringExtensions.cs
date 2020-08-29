namespace TimVinkemeier.AzureDevOpsToolkit.Core.Extensions
{
    public static class StringExtensions
    {
        public static int CountOccurencesOf(this string text, string pattern)
        {
            var count = 0;
            var i = 0;
            while ((i = text.IndexOf(pattern, i)) != -1)
            {
                i += pattern.Length;
                count++;
            }
            return count;
        }
    }
}