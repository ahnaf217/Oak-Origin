using System.Text.RegularExpressions;

namespace demo.Helpers
{
    public class RemoveHtmlTagHelper
    {
        public static string RemoveHtmlTags(string input)
        {
            return Regex.Replace(input, "<.*?| &.*?;", string.Empty);
        }
    }
}
