using SPA2.QueryProcessor.model;
using SPA2.QueryProcessor.patterns.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA2.QueryProcessor.patterns
{
    class SelectSuchThatPattern : ParserPattern
    {
        private static SelectSuchThatPattern INSTANCE = new SelectSuchThatPattern();

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
            return new ParsedSelect();
        }
    }
}
