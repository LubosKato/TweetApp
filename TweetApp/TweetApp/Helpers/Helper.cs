using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TweetApp.Helpers
{
    public static class Helper
    {
        internal static int AccountNameCount(this string text)
        {
            var remailRegex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase);;
            text = remailRegex.Replace(text, string.Empty);
            MatchCollection matches = Regex.Matches(text, @"@(\w+)");
            return matches.Count;
        }

        internal static List<string> ProcessTweetString(this string accounts)
        {
            string[] words = Array.ConvertAll(accounts.Split(','), p => p.Trim());
            return (from word in words
                    select Regex.Matches(word, @"@(\w+)")
                    into matches where matches.Count > 0 select matches[0].Groups[1].Value).ToList();
        }
    }
}