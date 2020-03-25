using SPA2.AST;
using SPA2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA2.ProcTable
{
    public class ProcTable : IProcTable
    {
        public List<Procedure> Procedures { get; set; }

        public ProcTable()
        {
            Procedures = new List<Procedure>();
        }

        public TNODE GetAstRoot(string procName)
        {
            var proc = GetProc(procName);
            return proc == null ? null : proc.AstRoot;
        }

        public TNODE GetAstRoot(int index)
        {
            var proc = GetProc(index);
            return proc == null ? null : proc.AstRoot;
        }

        public Procedure GetProc(int index)
        {
            return Procedures.Where(i => i.Index == index).FirstOrDefault();
        }

        public Procedure GetProc(string procName)
        {
            return Procedures.Where(i => i.Name == procName).FirstOrDefault();
        }

        public int GetProcIndex(string procName)
        {
            var procedure = GetProc(procName);
            return procedure == null ? -1 : procedure.Index;
        }

        public int GetSize()
        {
            return Procedures.Count();
        }

        public int InsertProc(string procName)
        {
            if (Procedures.Where(i => i.Name == procName).Any())
            {
                return -1;
            }
            else
            {
                Procedure variable = new Procedure(procName);
                variable.Index = GetSize();
                Procedures.Add(variable);
                return GetProcIndex(procName);
            }
        }

        public int SetAstRoot(string procName, TNODE node)
        {
            var procedure = GetProc(procName);
            if(procedure == null)
            {
                return -1;

            }
            else
            {
                procedure.AstRoot = node;
                return procedure.Index;
            }
        }
    }
}
