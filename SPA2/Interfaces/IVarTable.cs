using SPA2.AST;
using SPA2.VarTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA2.Interfaces
{
    interface IVarTable
    {
        int InsertVar(string procName);
        VarRefs GetVarRefs(string procName);
        VarRefs GetVarRefs(int index);
        int GetSize();
        int SetAstRoot(string procName, TNODE node);
        TNODE GetAstRoot(string procName);
        TNODE GetAstRoot(int index);
    }
}
