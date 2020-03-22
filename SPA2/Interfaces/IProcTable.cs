using SPA2.AST;
using SPA2.ProcTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA2.Interfaces
{
    interface IProcTable
    {
        int InsertProc(string procName);
        ProcRefs GetProcRefs(string procName);
        ProcRefs GetProcRefs(int index);
        int GetSize();
        int SetAstRoot(string procName, TNODE node);
        TNODE GetAstRoot(string procName);
        TNODE GetAstRoot(int index);
    }
}
