using SPA2.AST;
using SPA2.Enums;
using SPA2.StmtTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA2.Interfaces
{
    public interface IStmtTable
    {
        int InsertStmt(EntityTypeEnum entityTypeEnum, int codeLine);
        Statement GetStmt(int codeLine);
        int GetSize();
        int SetAstRoot(int codeLine, TNODE node);
        TNODE GetAstRoot(int codeLine);
    }
}
