using SPA2.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA2.ProcTable
{
    public class Procedure
    {
        public int Index { get;set; }
        public string Name { get; set; }
        public TNODE AstRoot { get; set; }
        public Dictionary<int,bool> ModifiesList { get; set; }
        public Procedure(string name)
        {
            Name = name;
            ModifiesList = new Dictionary<int, bool>();
        }


    }
}
