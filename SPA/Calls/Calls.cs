using SPA.AST;
using SPA.Enums;
using SPA.Interfaces;
using SPA.ProcTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPA.Calls
{
    public sealed class Calls : ICalls
    {
        private static Calls _instance = null;

        public static Calls Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Calls();
                }
                return _instance;
            }
        }

        private Calls()
        {

        }
        public List<Procedure> GetCalledBy(string proc)
        {
            List<Procedure> procedures = new List<Procedure>();
            foreach (Procedure procedure in ProcTable.ProcTable.Instance.Procedures)
            {
                if (IsCalls(procedure.Name, proc))
                {
                    procedures.Add(procedure);
                }
            }
            return procedures;
        }

        public List<Procedure> GetCalledByStar(string proc)
        {
            List<Procedure> procedures = new List<Procedure>();
            foreach(Procedure procedure in ProcTable.ProcTable.Instance.Procedures)
            {
                if (IsCallsStar(procedure.Name, proc))
                {
                    procedures.Add(procedure);
                }
            }
            return procedures;
        }

        public List<Procedure> GetCalls(string proc)
        {
            List<Procedure> procedures = new List<Procedure>();
            TNODE procNode = ProcTable.ProcTable.Instance.GetAstRoot(proc);
            TNODE stmtLstChild = AST.AST.Instance.GetFirstChild(procNode);
            List<string> procNames = AST.AST.Instance
                .GetLinkedNodes(stmtLstChild, LinkTypeEnum.Child)
                .Where(i => i.EntityTypeEnum == EntityTypeEnum.Call)
                .Select(i => i.Attr.Name)
                .ToList();
            foreach(string proce in procNames)
            {
                Procedure findProcedure = ProcTable.ProcTable.Instance.GetProc(proce);
                if(findProcedure != null)
                {
                    procedures.Add(findProcedure);
                }
            }
            return procedures;
                
        }

        public List<Procedure> GetCallsStar(string proc)
        {
            List<Procedure> procedures = new List<Procedure>();
            return GetCallsStar(proc, procedures);
        }

        private List<Procedure> GetCallsStar(string proc,List<Procedure> procedures)
        {
            foreach (Procedure procedure in GetCalls(proc))
            {
                procedures.Add(procedure);
                GetCallsStar(procedure.Name, procedures);
            }
            return procedures;
        }

        public bool IsCalls(string proc1, string proc2)
        {
            return GetCalls(proc1)
                .Where(i => i.Name == proc2)
                .Any();
        }

        public bool IsCallsStar(string proc1, string proc2)
        {
            return GetCallsStar(proc1)
                .Where(i => i.Name == proc2)
                .Any();
        }
        public bool IsCalledBy(string proc1, string proc2)
        {
            return GetCalledBy(proc1)
                .Where(i => i.Name == proc2)
                .Any();
        }

        public bool IsCalledStarBy(string proc1, string proc2)
        {
            return GetCalledByStar(proc1)
                .Where(i => i.Name == proc2)
                .Any();
        }
    }
}
