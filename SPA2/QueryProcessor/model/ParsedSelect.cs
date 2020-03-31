using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA2.QueryProcessor.model
{
    public class ParsedSelect
    {
        private String selectValue { get; set; }
        private SuchThatPart suchThatValue { get; set; }
        private String withValue { get; set; }

        public ParsedSelect()
        {

        }
        public ParsedSelect(string selectValue, SuchThatPart suchThatValue, string withValue)
        {
            this.selectValue = selectValue;
            this.suchThatValue = suchThatValue;
            this.withValue = withValue;
        }

    }
}
