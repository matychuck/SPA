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
        Procedure GetProc(int index);
        Procedure GetProc(string procName);
        int GetProcIndex(string procName);
        int GetSize();
        int SetAstRoot(string procName, TNODE node);
        TNODE GetAstRoot(string procName);
        TNODE GetAstRoot(int index);
    }
}
