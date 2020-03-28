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
    public interface IUses
    {
        // Zapisuje fakt, że Uses(stmt,var) jest prawdziwe
        void SetUses(Statement stmt, Variable var);
        // Zapisuje fakt, że Uses(proc,var) jest prawdziwe
        void SetUses(Procedure proc, Variable var);
        // Zwraca listę zmiennych, które używane są przez instrukcję [Uses(stmt,var)]
        List<Variable> GetUsed(Statement stmt);
        // Zwraca listę zmiennych, które używane są przez procedure [Uses(proc,var)]
        List<Variable> GetUsed(Procedure proc);
        // Zwraca listę instrukcji takich, że Uses(stmt,var) 
        List<Statement> GetUsesForStmts(Variable var);
        // Zwraca listę procedur takich, że Uses(proc,var) 
        List<Procedure> GetUsesForProcs(Variable var);
        // Jeśli zmienna 'var' używana jest przez instrukcję 'stat' zwraca TRUE, w przeciwnym razie FALSE
        bool IsUsed(Variable var, Statement stat);
        // Jeśli zmienna 'var' używana jest przez procedurę 'proc' zwraca TRUE, w przeciwnym razie FALSE
        bool IsUsed(Variable var, Procedure proc);
    }
}
