using SPA.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.VarTable
{
    public class Variable
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public Variable(string name)
        {
            Name = name;
        }

    }
}
