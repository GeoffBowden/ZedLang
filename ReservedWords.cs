using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZedLang
{
    public static class ExtensionMethodsForReservedWords
    {
        private static List<string> Symbols = new List<string>()
        {
            "=", ">", "<", "+", "-", "=", "*", "%", "!"
        };
        private static List<string> ReservedWords = new List<string>()
        {
            "not", "and", "or", "xor", "implies", "ifandonlyif", "if", "then"
        };
        private static Char[] WordSeperators = new Char[] { ' ', '\t' };
        public static bool IsaReservedWord(this string word)
        {
            return ReservedWords.Contains(word.ToLower());
        }
        public static List<string> BreakLine(this string line)
        {
            return line.Split(WordSeperators, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
        public static string ContainsaReservedWord(this string line)
        {
            CultureInfo culture = new CultureInfo("en-GB");
            foreach ( var word in ReservedWords )
            {
                if ( culture.CompareInfo.IndexOf(line, word, CompareOptions.IgnoreCase) >= 0 )
                {
                    return word;
                }
            }
            return string.Empty;
        }
        public static bool IsAnOpenBracket( this string word )
        {
            return word == "(";
        }
        public static bool IsACloseBracket(this string word)
        {
            return word == ")";
        }
    }
}
