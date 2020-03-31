﻿using SPA2.QueryProcessor.model;
using SPA2.QueryProcessor.patterns.common;
using System;
using System.Text.RegularExpressions;

namespace SPA2.QueryProcessor.patterns
{
    class SelectPattern : ParserPattern
    {
        private static SelectPattern INSTANCE = new SelectPattern();

        private static string pattern = "select *";
        RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace;

        private SelectPattern()
        {

        }

        public static SelectPattern getInstance()
        {
            return INSTANCE;
        }

        public ParsedSelect parse(String expression)
        {
            Console.WriteLine("select parse()");
            foreach (Match match in Regex.Matches(expression, pattern, options))
            {
                Console.WriteLine("Found {0} at index {1}.", match.Value, match.Index);
            }
            return new ParsedSelect();
        }
    }
}
