using Ganss.Xss;

namespace FamilyCookbook.Common.Upload
{
    public static class Sanitize
    {
        public static string HtmlSanitize(string richText)
        {
            var sanitiezer = new HtmlSanitizer();

            richText = sanitiezer.Sanitize(richText);

            return richText;
        }
    }
}
