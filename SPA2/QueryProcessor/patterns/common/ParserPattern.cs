using SPA2.QueryProcessor.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA2.QueryProcessor.patterns.common
{
    interface ParserPattern
    {
        ParsedSelect parse(String expression);

        bool isMatched(String expression);
    }
}
