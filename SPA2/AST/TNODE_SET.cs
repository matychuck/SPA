using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA2.AST
{
    public class TNODE_SET
    {
        public List<TNODE> nodes { get; set; }

        public TNODE_SET()
        {
            nodes = new List<TNODE>();
        }
    }
}
