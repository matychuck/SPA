using SPA2.QueryProcessor.model;
using SPA2.QueryProcessor.patterns.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA2.QueryProcessor.patterns
{
    class SelectSuchThatWithPattern : ParserPattern
    {
        private static SelectSuchThatWithPattern INSTANCE = new SelectSuchThatWithPattern();

        private SelectSuchThatWithPattern()
        {

        }

        public static SelectSuchThatWithPattern getInstance()
        {
            return INSTANCE;
        }

        public ParsedSelect parse(string expression)
        {
            return new ParsedSelect();
        }
    }
}
