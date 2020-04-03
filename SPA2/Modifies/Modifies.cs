using SPA2.Interfaces;
using SPA2.ProcTable;
using SPA2.StmtTable;
using SPA2.VarTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA2.Modifies
{
    public sealed class Modifies : IModifies
    {
        private static Modifies _instance = null;

        public static Modifies Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Modifies();
                }
                return _instance;
            }
        }
        private Modifies()
        {

        }
        public List<Variable> GetModified(Statement stmt)
        {
            List<int> varIndexes = stmt.ModifiesList.Where(i => i.Value == true).Select(i => i.Key).ToList();

            return VarTable.VarTable.Instance.Variables.Where(i => varIndexes.Contains(i.Index)).ToList();
        }

        public List<Variable> GetModified(Procedure proc)
        {
            List<int> varIndexes = proc.ModifiesList.Where(i => i.Value == true).Select(i => i.Key).ToList();

            return VarTable.VarTable.Instance.Variables.Where(i => varIndexes.Contains(i.Index)).ToList();
        }

        public List<Procedure> GetModifiesForProcs(Variable var)
        {
            List<Procedure> procedures = new List<Procedure>();

            foreach(Procedure procedure in ProcTable.ProcTable.Instance.Procedures)
            {
                if (IsModified(var, procedure))
                {
                    procedures.Add(procedure);
                }
            }

            return procedures;
        }

        public List<Statement> GetModifiesForStmts(Variable var)
        {
            List<Statement> statements = new List<Statement>();

            foreach (Statement statement in StmtTable.StmtTable.Instance.Statements)
            {
                if (IsModified(var,statement))
                {
                    statements.Add(statement);
                }
            }

            return statements;
        }

        public bool IsModified(Variable var, Statement stat)
        {
            return stat.ModifiesList.TryGetValue(var.Index, out bool value) && value;
        }

        public bool IsModified(Variable var, Procedure proc)
        {
            return proc.ModifiesList.TryGetValue(var.Index, out bool value) && value;
        }

        public void SetModifies(Statement stmt, Variable var)
        {
            if (stmt.ModifiesList.TryGetValue(var.Index, out bool value))
            {
                stmt.ModifiesList[var.Index] = true;
            }
            else
            {
                stmt.ModifiesList.Add(var.Index, true);
            }
        }

        public void SetModifies(Procedure proc, Variable var)
        {
            if (proc.ModifiesList.TryGetValue(var.Index, out bool value))
            {
                proc.ModifiesList[var.Index] = true;
            }
            else
            {
                proc.ModifiesList.Add(var.Index, true);
            }
        }
    }
}
