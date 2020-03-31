using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA2.QueryProcessor.patterns.common
{
    class SelectPatterns
    {
        private SelectPatterns()
        {

        }

        public static List<ParserPattern> getSelectPatterns()
        {
            List<ParserPattern> parserPatterns = new List<ParserPattern>();

            parserPatterns.Add(SelectPattern.getInstance());
            parserPatterns.Add(SelectSuchThatPattern.getInstance());
            parserPatterns.Add(SelectSuchThatWithPattern.getInstance());

            return parserPatterns;

        }
    }
}
