using System;
using SPA.Enums;
using System.Collections.Generic;
using SPA.AST;
using SPA.VarTable;
using SPA.ProcTable;
using SPA.StmtTable;

namespace SPA.QueryProcessor
{
    public static class QueryMethodChecker
    {

        public static void CheckModifiesOrUses(string firstArgument, string secondArgument,
                                        Func<Variable, Procedure, bool> methodForProc,
                                        Func<Variable, Statement, bool> methodForStmt)
        {
            EntityTypeEnum firstArgType = QueryProcessor.GetVarEnumType(firstArgument);
            if (firstArgType == EntityTypeEnum.Procedure)
                CheckProcedureModifiesOrUses(firstArgument, secondArgument, methodForProc);
            else
                CheckStatementModifiesOrUses(firstArgument, secondArgument, methodForStmt);
        }

        private static void CheckProcedureModifiesOrUses(string firstArgument, string secondArgument, Func<Variable, Procedure, bool> IsModifiedOrUsedByProc)
        {
            List<int> firstArgIndexes = QueryDataGetter.GetArgIndexes(firstArgument);
            List<int> secondArgIndexes = QueryDataGetter.GetArgIndexes(secondArgument);

            List<int> procStayinIndexes = new List<int>();
            List<int> varStayinIndexes = new List<int>();

            Procedure proc;
            Variable var;
            foreach (int firstInd in firstArgIndexes)
                foreach (int secondInd in secondArgIndexes)
                {
                    proc = ProcTable.ProcTable.Instance.GetProc(firstInd);
                    var = VarTable.VarTable.Instance.GetVar(secondInd);
                    //Modifies.Modifies.Instance.IsModified
                    if (IsModifiedOrUsedByProc(var, proc))
                    {
                        procStayinIndexes.Add(firstInd);
                        varStayinIndexes.Add(secondInd);
                       
                    }
                }
            QueryDataGetter.RemoveIndexesFromLists(firstArgument, secondArgument,
                                                   procStayinIndexes,
                                                   varStayinIndexes);
        }

        private static void CheckStatementModifiesOrUses(string firstArgument, string secondArgument, Func<Variable, Statement, bool> IsModifiedOrUsedByStmt)
        {
            List<int> firstArgIndexes = QueryDataGetter.GetArgIndexes(firstArgument);
            List<int> secondArgIndexes = QueryDataGetter.GetArgIndexes(secondArgument);

            List<int> stmtStayinIndexes = new List<int>();
            List<int> varStayinIndexes = new List<int>();

            Statement stmt;
            Variable var;
            foreach (int firstInd in firstArgIndexes)
                foreach (int secondInd in secondArgIndexes)
                {
                    stmt = StmtTable.StmtTable.Instance.GetStmt(firstInd);
                    var = VarTable.VarTable.Instance.GetVar(secondInd);
                    //Modifies.Modifies.Instance.IsModified
                    if (IsModifiedOrUsedByStmt(var, stmt))
                    {
                        stmtStayinIndexes.Add(firstInd);
                        varStayinIndexes.Add(secondInd);
                    }
                }
            QueryDataGetter.RemoveIndexesFromLists(firstArgument, secondArgument,
                                                   stmtStayinIndexes,
                                                   varStayinIndexes);
        }

        public static void CheckParentOrFollows(string firstArgument, string secondArgument, Func<TNODE, TNODE, bool> method)
        {
            EntityTypeEnum firstArgType = QueryProcessor.GetVarEnumType(firstArgument);
            EntityTypeEnum secondArgType = QueryProcessor.GetVarEnumType(secondArgument);

            List<int> firstArgIndexes = QueryDataGetter.GetArgIndexes(firstArgument);
            List<int> secondArgIndexes = QueryDataGetter.GetArgIndexes(secondArgument);

            List<int> firstStayinIndexes = new List<int>();
            List<int> secondStayinIndexes = new List<int>();

            TNODE first;
            TNODE second;
            foreach (int firstInd in firstArgIndexes)
                foreach (int secondInd in secondArgIndexes)
                {
                    first = GetNodeByType(firstArgType, firstInd);
                    second = GetNodeByType(secondArgType, secondInd);
                    if(method(first, second))
                    {
                        firstStayinIndexes.Add(firstInd);
                        secondStayinIndexes.Add(secondInd);
                    }
                }

             QueryDataGetter.RemoveIndexesFromLists(firstArgument, secondArgument,
                                                   firstStayinIndexes,
                                                   secondStayinIndexes);

        }

        private static TNODE GetNodeByType(EntityTypeEnum et, int ind)
        {
            TNODE node;
            if(et == EntityTypeEnum.Procedure)
            {
                node = ProcTable.ProcTable.Instance.GetAstRoot(ind);
            } else {
                node = StmtTable.StmtTable.Instance.GetAstRoot(ind);
            }

            return node;
        }
    }

}
