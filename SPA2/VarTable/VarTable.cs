using SPA2.AST;
using SPA2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA2.VarTable
{
    class VarTable : IVarTable
    {
        public List<VarRefs> VarRefsList { get; set; }

        public TNODE GetAstRoot(string procName)
        {
            throw new NotImplementedException();
        }

        public TNODE GetAstRoot(int index)
        {
            throw new NotImplementedException();
        }

        public int GetSize()
        {
            throw new NotImplementedException();
        }

        public VarRefs GetVarRefs(string procName)
        {
            throw new NotImplementedException();
        }

        public VarRefs GetVarRefs(int index)
        {
            throw new NotImplementedException();
        }

        public int InsertVar(string procName)
        {
            throw new NotImplementedException();
        }

        public int SetAstRoot(string procName, TNODE node)
        {
            throw new NotImplementedException();
        }
    }
}
