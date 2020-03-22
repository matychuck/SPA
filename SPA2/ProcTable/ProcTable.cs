using SPA2.AST;
using SPA2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA2.ProcTable
{
    class ProcTable : IProcTable
    {
        public List<ProcRefs> ProcRefsList { get; set; }

        public TNODE GetAstRoot(string procName)
        {
            throw new NotImplementedException();
        }

        public TNODE GetAstRoot(int index)
        {
            throw new NotImplementedException();
        }

        public ProcRefs GetProcRefs(string procName)
        {
            throw new NotImplementedException();
        }

        public ProcRefs GetProcRefs(int index)
        {
            throw new NotImplementedException();
        }

        public int GetSize()
        {
            throw new NotImplementedException();
        }

        public int InsertProc(string procName)
        {
            throw new NotImplementedException();
        }

        public int SetAstRoot(string procName, TNODE node)
        {
            throw new NotImplementedException();
        }
    }
}
