﻿using SPA2.AST;
using SPA2.Enums;
using SPA2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA2.StmtTable
{
    public class StmtTable : IStmtTable
    {
        public List<Statement> Statements { get; set; }
        public StmtTable()
        {
            Statements = new List<Statement>();
        }
        public TNODE GetAstRoot(int codeLine)
        {
            var proc = GetStmt(codeLine);
            return proc == null ? null : proc.AstRoot;
        }

        public int GetSize()
        {
            return Statements.Count();
        }

        public Statement GetStmt(int codeLine)
        {
            return Statements.Where(i => i.CodeLine == codeLine).FirstOrDefault();
        }

        public int InsertStmt(EntityTypeEnum entityTypeEnum, int codeLine)
        {
            if (Statements.Where(i => i.CodeLine == codeLine).Any())
            {
                return -1;
            }
            else
            {
                Statement variable = new Statement(entityTypeEnum,codeLine);
                Statements.Add(variable);
                return 0;
            }
        }

        public int SetAstRoot(int codeLine, TNODE node)
        {
            var procedure = GetStmt(codeLine);
            if (procedure == null)
            {
                return -1;

            }
            else
            {
                procedure.AstRoot = node;
                return 0;
            }
        }
    }
}