using SPA2.ProcTable;
using SPA2.StmtTable;
using SPA2.VarTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA2.Interfaces
{
    public interface IModifies
    {
        void SetModifies(Statement stmt, Variable var);

        void SetModifies(Procedure proc, Variable var);

        List<Variable> GetModified(Statement stmt);
        List<Variable> GetModified(Procedure proc);
        List<Statement> GetModifiesForStmts(Variable var);
        List<Procedure> GetModifiesForProcs(Variable var);
        bool IsModified(Variable var, Statement stat);
        bool IsModified(Variable var, Procedure proc);


    }
}
