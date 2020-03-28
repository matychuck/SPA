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
        Variable GetVar(int index);
        Variable GetVar(string varName);
        int GetVarIndex(string varName);
        int GetSize();
        
    }
}
