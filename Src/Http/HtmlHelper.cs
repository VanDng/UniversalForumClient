using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UniversalForumClient.Http
{
    /*
     * Reference: https://stackoverflow.com/questions/1691767/c-sharp-remove-spaces-in-html-source-in-between-markups
     */

    static class HtmlHelper
    {
        // positive look behind for ">", one or more whitespace (non-greedy), positive lookahead for "<"
        private static readonly Regex InsignificantHtmlWhitespace = new Regex(@"(?<=>)\s+?(?=<)");

        // Known not to handle HTML comments or CDATA correctly, which we don't use.
        public static string RemoveInsignificantHtmlWhiteSpace(string html)
        {
            return InsignificantHtmlWhitespace.Replace(html, String.Empty).Trim();
        }
    }
}
