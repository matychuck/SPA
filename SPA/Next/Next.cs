using SPA.Enums;
using SPA.Interfaces;
using SPA.StmtTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPA.Next
{
    public sealed class Next : INext
    {
        private static Next _instance = null;

        public static Next Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Next();
                }
                return _instance;
            }
        }
        private Next()
        {

        }

        public List<Statement> GetNext(Statement statement)
        {
            List<Statement> nextStatements = new List<Statement>();
            var astNode = statement.AstRoot;
            var previousFollow = AST.AST.Instance.GetPrevLinkedNodes(astNode, LinkTypeEnum.Follows).FirstOrDefault();
            //jezeli nastęnik nie jest pusty i nie jest ifem -> bo if przechodzi do dzieci 
            if (previousFollow != null && astNode.EntityTypeEnum != EntityTypeEnum.If)
            {
                var nextStatement = StmtTable.StmtTable.Instance.Statements.Where(i => i.AstRoot == previousFollow).FirstOrDefault();
                nextStatements.Add(nextStatement);
            }
            // jeżeli jesteśmy w ife lub while - przechodzimy rowniez do pierwszego dziecka dla stmtLst

            foreach (var stmtLst in astNode.Links.Where(i=>i.LinkTypeEnum == LinkTypeEnum.Child && i.LinkNode.EntityTypeEnum == EntityTypeEnum.Stmtlist).Select(i=>i.LinkNode))
            {
                var firstChild = AST.AST.Instance.GetFirstChild(stmtLst);
                if (firstChild != null)
                {
                    var nextStatement = StmtTable.StmtTable.Instance.Statements.Where(i => i.AstRoot == firstChild).FirstOrDefault();
                    nextStatements.Add(nextStatement);

                }
            }

            //jeżeli node nie ma followsa  
            var tempNode = AST.AST.Instance.GetParent(astNode);
            while (previousFollow == null && tempNode != null)
            {
                if (tempNode.EntityTypeEnum != EntityTypeEnum.While && tempNode.EntityTypeEnum != EntityTypeEnum.If) 
                {
                    break;
                }
                //jezeli rodzic jest whilem - powrot do rodzica 
                if(tempNode.EntityTypeEnum == EntityTypeEnum.While)
                {
                    var tempStmt = StmtTable.StmtTable.Instance.Statements.Where(i => i.AstRoot == tempNode).FirstOrDefault();
                    nextStatements.Add(tempStmt);
                    break;
                }
                //jezeli rodzic jest ifem
                if (tempNode.EntityTypeEnum == EntityTypeEnum.If)
                {
                    //gdy rodzic ma następnika - to do niego przechodzimy 
                    var previousFollowForIf =  AST.AST.Instance.GetPrevLinkedNodes(tempNode, LinkTypeEnum.Follows).FirstOrDefault();
                    if (previousFollowForIf != null)
                    {
                        var tempStmt = StmtTable.StmtTable.Instance.Statements.Where(i => i.AstRoot == previousFollowForIf).FirstOrDefault();
                        nextStatements.Add(tempStmt);
                        break;

                    }
                    //a jeżeli nie - przejść do parenta i zobaczyć co dalej
                    else
                    {
                        tempNode = AST.AST.Instance.GetParent(tempNode);
                        if(tempNode != null)
                        {
                            previousFollow = AST.AST.Instance.GetPrevLinkedNodes(tempNode, LinkTypeEnum.Follows).FirstOrDefault();
                        }

                    }
                    

                }

            }

            return nextStatements;
        }

        public List<Statement> GetNextStar(Statement statement)
        {
            List<Statement> statements = new List<Statement>();
            return GetNextStar(statement, statements);
        }
        private List<Statement> GetNextStar(Statement statement, List<Statement> statements)
        {
            foreach (Statement statement1 in GetNext(statement))
            {
                if (!statements.Any(x => x == statement1))
                {
                    statements.Add(statement1);
                    GetNextStar(statement1, statements);
                }
            }
            return statements;
        }

        public bool IsNext(int line1, int line2)
        {
            var stmt = StmtTable.StmtTable.Instance.GetStmt(line1);
            return GetNext(stmt).Any(i => i.CodeLine == line2);
        }

        public bool IsNextStar(int line1, int line2)
        {
            var stmt = StmtTable.StmtTable.Instance.GetStmt(line1);
            var stmtList = GetNextStar(stmt);
            return stmtList.Any(i => i.CodeLine == line2);
        }
    }
}
