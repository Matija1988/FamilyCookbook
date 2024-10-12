using Ganss.Xss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
