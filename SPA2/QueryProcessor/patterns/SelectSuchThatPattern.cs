using SPA2.QueryProcessor.model;
using SPA2.QueryProcessor.patterns.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SPA2.QueryProcessor.patterns
{
    class SelectSuchThatPattern : ParserPattern
    {
        private static SelectSuchThatPattern INSTANCE = new SelectSuchThatPattern();

        private static string pattern = "select *"; //TODO:
        RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace;

        private SelectSuchThatPattern()
        {

        }

        //TODO: w metodzie przypadek z BOOLEAN

        public static SelectSuchThatPattern getInstance()
        {
            return INSTANCE;
        }

        public ParsedSelect parse(string expression)
        {
            //TODO:
            return new ParsedSelect();
        }

        public bool isMatched(String expression)
        {
            foreach (Match match in Regex.Matches(expression, pattern, options))
            {
                return true;
            }
            return false;
        }
    }
}
