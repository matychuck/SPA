using SPA2.Interfaces;
using SPA2.ProcTable;
using SPA2.StmtTable;
using SPA2.VarTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA2.Uses
{
    class Uses : IUses
    {
        public ProcTable.ProcTable ProcTable { get; set; }
        public StmtTable.StmtTable StmtTable { get; set; }
        public VarTable.VarTable VarTable { get; set; }

        public List<Variable> GetUsed(Statement stmt)
        {
            List<int> varIndexes = stmt.UsesList.Where(i => i.Value == true).Select(i => i.Key).ToList();

            return VarTable.Variables.Where(i => varIndexes.Contains(i.Index)).ToList();
        }

        public List<Variable> GetUsed(Procedure proc)
        {
            List<int> varIndexes = proc.ModifiesList.Where(i => i.Value == true).Select(i => i.Key).ToList();

            return VarTable.Variables.Where(i => varIndexes.Contains(i.Index)).ToList();
        }

        public List<Procedure> GetUsesForProcs(Variable var)
        {
            List<Procedure> procedures = new List<Procedure>();

            foreach (Procedure procedure in ProcTable.Procedures)
            {
                if (IsUsed(var, procedure))
                {
                    procedures.Add(procedure);
                }
            }

            return procedures;
        }

        public List<Statement> GetUsesForStmts(Variable var)
        {
            List<Statement> statements = new List<Statement>();

            foreach (Statement statement in StmtTable.Statements)
            {
                if (IsUsed(var, statement))
                {
                    statements.Add(statement);
                }
            }

            return statements;
        }

        public bool IsUsed(Variable var, Statement stat)
        {
            return stat.UsesList.TryGetValue(var.Index, out bool value) && value;
        }

        public bool IsUsed(Variable var, Procedure proc)
        {
            return proc.UsesList.TryGetValue(var.Index,out bool value) && value;
        }

        public void SetUses(Statement stmt, Variable var)
        {
            if (stmt.UsesList.ContainsKey(var.Index))
            {
                stmt.UsesList[var.Index] = true;
            }
            else
            {
                stmt.UsesList.Add(var.Index, true);
            }
        }

        public void SetUses(Procedure proc, Variable var)
        {
            if (proc.UsesList.ContainsKey(var.Index))
            {
                proc.UsesList[var.Index] = true;
            }
            else
            {
                proc.UsesList.Add(var.Index, true);
            }
        }
    }
}
