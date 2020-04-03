﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPA2.AST;

namespace SPA2.Parser
{
    public class Parser
    {
        List<String> reservedWords; // lista słów kluczowych
        public Parser()
        {
            reservedWords = new List<string>();
            reservedWords.Add("procedure");
            reservedWords.Add("while");
        }

        /// <summary>
        /// Na podstawie linii SIMPLE zwraca token (np. słowo kluczowe 'procedure' albo nazwę zmiennej)
        /// </summary>
        /// <param name="lines">linie kodu SIMPLE</param>
        /// <param name="lineNumber">aktualnie przetwarzana linia</param>
        /// <param name="startIndex">indeks od którego ma zacząć czytać w danej linii</param>
        /// <param name="endIndex">indeks, na którym skończyło czytać w danej linii</param>
        /// <param name="test">wczytywanie testowe (odtwarza lineNumber)</param>
        /// <returns>odczytany token</returns>
        public string GetToken(List<string> lines, ref int lineNumber, int startIndex, out int endIndex, bool test)
        {
            int lineNumberIn = lineNumber;

            string fileLine = "";
            if (startIndex == -1)
            {
                lineNumber++;
                startIndex = 0;
            }
            if (lineNumber >= lines.Count)
            {
                throw new Exception("ParseProcedure: Nieoczekiwany koniec pliku");
            }

            fileLine = lines.ElementAt(lineNumber);
            if(startIndex >= fileLine.Length)
            {
                lineNumber++;
                startIndex = 0;
                if (lineNumber >= lines.Count)
                {
                    endIndex = -1;
                    if (test) lineNumber = lineNumberIn;
                    return "";
                }
                fileLine = lines.ElementAt(lineNumber);
            }

            string token = "";
            char character;
            character = fileLine[startIndex];
            while(character == ' ')
            {
                startIndex++;
                if(startIndex > fileLine.Length)
                {
                    endIndex = -1;
                    if (test) lineNumber = lineNumberIn;
                    return token;
                }
                character = fileLine[startIndex];
            }
            for (int index = startIndex; index < fileLine.Length; index++)
            {
                character = fileLine[index];
                if (Char.IsLetter(character) || Char.IsDigit(character))
                {
                    token += character;
                }
                else
                {
                    if (token == "")
                    {
                        token += character;
                        endIndex = index + 1;
                        if (endIndex > fileLine.Length) endIndex = -1;
                        if (test) lineNumber = lineNumberIn;
                        return token;
                    }
                    else
                    {
                        endIndex = index;
                        if (test) lineNumber = lineNumberIn;
                        return token;
                    }
                }
            }
            endIndex = fileLine.Length + 1;
            if (endIndex > fileLine.Length) endIndex = -1;
            if (test) lineNumber = lineNumberIn;
            return token;
        }


        /// <summary>
        /// Parsowanie procedury
        /// </summary>
        /// <param name="lines">linie kodu SIMPLE</param>
        /// <param name="lineNumber">aktualnie przetwarzana linia</param>
        /// <param name="startIndex">indeks od którego ma zacząć czytać w danej linii</param>
        /// <param name="endIndex">indeks, na którym skończyło czytać w danej linii</param>
        public void ParseProcedure(List<string> lines, int startIndex, ref int lineNumber, out int endIndex)
        {
            string token = GetToken(lines, ref lineNumber, startIndex, out endIndex, false);
            if (token != "procedure") throw new Exception("ParseProcedure: Brak słowa procedure");
            startIndex = endIndex;

            token = GetToken(lines, ref lineNumber, startIndex, out endIndex, false); // wczytanie nazwy procedury
            TNODE newNode = AST.AST.Instance.CreateTNode(Enums.EntityTypeEnum.Procedure);
            if (IsVarName(token)) {
                ProcTable.ProcTable.Instance.InsertProc(token);
                ProcTable.ProcTable.Instance.SetAstRoot(token, newNode);
                if (token == "Main") AST.AST.Instance.SetRoot(newNode);
            }
            else throw new Exception("ParseProcedure: Błędna nazwa procedury, "+token);
            string procedureName = token;
            startIndex = endIndex;

            token = GetToken(lines, ref lineNumber, startIndex, out endIndex, true); // wczytanie {
            if (token != "{") throw new Exception("ParseProcedure: Brak nawiasu { po nazwie procedury");

            Parse(lines, startIndex, ref lineNumber, out endIndex, procedureName, newNode);
        }


        /// <summary>
        /// Parsowanie listy instrukcji (np. dla procedury albo pętli while)
        /// </summary>
        /// <param name="lines">linie kodu SIMPLE</param>
        /// <param name="lineNumber">aktualnie przetwarzana linia</param>
        /// <param name="startIndex">indeks od którego ma zacząć czytać w danej linii</param>
        /// <param name="endIndex">indeks, na którym skończyło czytać w danej linii</param>
        /// <param name="procedureName">nazwa przetwarzanej procedury</param>
        public void ParseStmtLst(List<string> lines, int startIndex, ref int lineNumber, out int endIndex, string procedureName, TNODE parent)
        {
            string token = GetToken(lines, ref lineNumber, startIndex, out endIndex, false);
            if (token != "{") throw new Exception("ParseStmtLst: Brak znaku {");
            TNODE newNode = AST.AST.Instance.CreateTNode(Enums.EntityTypeEnum.Stmtlist); // tworzenie i łączenie stmtList z parentem
            AST.AST.Instance.SetChildOfLink(newNode, parent);
            startIndex = endIndex;

            while(lineNumber < lines.Count)
            {
                Parse(lines, startIndex, ref lineNumber, out endIndex, procedureName, parent);
                startIndex = endIndex;

                token = GetToken(lines, ref lineNumber, startIndex, out endIndex, true);
                if(token == "}")
                {
                    token = GetToken(lines, ref lineNumber, startIndex, out endIndex, false);
                    break;
                }
            }
            if (lineNumber == lines.Count && token != "}") throw new Exception("ParseStmtLst: Brak znaku }");
        }

        /// <summary>
        /// Parsowanie pętli while
        /// </summary>
        /// <param name="lines">linie kodu SIMPLE</param>
        /// <param name="lineNumber">aktualnie przetwarzana linia</param>
        /// <param name="startIndex">indeks od którego ma zacząć czytać w danej linii</param>
        /// <param name="endIndex">indeks, na którym skończyło czytać w danej linii</param>
        /// <param name="procedureName">nazwa przetwarzanej procedury</param>
        public void ParseWhile(List<string> lines, int startIndex, ref int lineNumber, out int endIndex, string procedureName, TNODE parent)
        {
            string token = GetToken(lines, ref lineNumber, startIndex, out endIndex, false);
            if (token != "while") throw new Exception("ParseWhile: Brak słowa kluczowego while");
            StmtTable.StmtTable.Instance.InsertStmt(Enums.EntityTypeEnum.While, lineNumber);
            startIndex = endIndex;

            TNODE whileNode = AST.AST.Instance.CreateTNode(Enums.EntityTypeEnum.While); // tworzenie node dla while
            StmtTable.StmtTable.Instance.SetAstRoot(lineNumber, whileNode);
            AST.AST.Instance.SetParent(whileNode, parent); //ustawianie parenta dla while

            TNODE stmtListNode = AST.AST.Instance.GetNthChild(0, parent);
            SettingFollows(whileNode, stmtListNode);

            AST.AST.Instance.SetChildOfLink(whileNode, stmtListNode); //łączenie stmlList z while
            TNODE variableNode = AST.AST.Instance.CreateTNode(Enums.EntityTypeEnum.Variable); // tworzenie node dla zmiennej po lewej stronie while node
            AST.AST.Instance.SetChildOfLink(variableNode, whileNode);

            token = GetToken(lines, ref lineNumber, startIndex, out endIndex, false);
            if (IsVarName(token)) {
                if (VarTable.VarTable.Instance.GetVarIndex(token) == -1) throw new Exception("ParseAssign: Zmienna nie została przypisana, " + token);
                else
                {
                    VarTable.Variable var = new VarTable.Variable(token);
                    SetUsesForFamily(whileNode, var);
                }
            }
            else throw new Exception("ParseWhile: Wymagana nazwa zmiennej, "+token);
            startIndex = endIndex;

            token = GetToken(lines, ref lineNumber, startIndex, out endIndex, true);
            if (token != "{") throw new Exception("ParseWhile: Brak znaku {");

            Parse(lines, startIndex, ref lineNumber, out endIndex, procedureName, whileNode);
        }


        public void SettingFollows(TNODE node, TNODE stmt)
        {
            List<TNODE> siblingsList = AST.AST.Instance.GetLinkedNodes(stmt, Enums.LinkTypeEnum.Child); //pobieranie siblings o ile istnieją
            if (siblingsList.Count() != 0)
            {
                TNODE prevStmt = siblingsList[siblingsList.Count() - 1];
                AST.AST.Instance.SetFollows(prevStmt, node);
            }
        }

        public void SetModifiesForFamily(TNODE node, VarTable.Variable var)
        {
            if (node.EntityTypeEnum == Enums.EntityTypeEnum.Procedure)
            {
                ProcTable.Procedure proc = ProcTable.ProcTable.Instance.Procedures.Where(i => i.AstRoot == node).FirstOrDefault();
                Modifies.Modifies.Instance.SetModifies(proc, var);
            }
            else
            {
                StmtTable.Statement stmt = StmtTable.StmtTable.Instance.Statements.Where(i => i.AstRoot == node).FirstOrDefault();
                Modifies.Modifies.Instance.SetModifies(stmt, var);
            }
            if (AST.AST.Instance.GetParent(node) != null) SetModifiesForFamily(AST.AST.Instance.GetParent(node), var);
        }

        public void SetUsesForFamily(TNODE node, VarTable.Variable var)
        {
            if (node.EntityTypeEnum == Enums.EntityTypeEnum.Procedure)
            {
                ProcTable.Procedure proc = ProcTable.ProcTable.Instance.Procedures.Where(i => i.AstRoot == node).FirstOrDefault();
                Uses.Uses.Instance.SetUses(proc, var);
            }
            else
            {
                StmtTable.Statement stmt = StmtTable.StmtTable.Instance.Statements.Where(i => i.AstRoot == node).FirstOrDefault();
                Uses.Uses.Instance.SetUses(stmt, var);
            }
            if (AST.AST.Instance.GetParent(node) != null) SetUsesForFamily(AST.AST.Instance.GetParent(node), var);
        }

        /// <summary>
        /// Parsowanie przypisania assign
        /// </summary>
        /// <param name="lines">linie kodu SIMPLE</param>
        /// <param name="lineNumber">aktualnie przetwarzana linia</param>
        /// <param name="startIndex">indeks od którego ma zacząć czytać w danej linii</param>
        /// <param name="endIndex">indeks, na którym skończyło czytać w danej linii</param>
        /// <param name="procedureName">nazwa przetwarzanej procedury</param>
        public void ParseAssign(List<string> lines, int startIndex, ref int lineNumber, out int endIndex, string procedureName, TNODE parent)
        {
            string token = GetToken(lines, ref lineNumber, startIndex, out endIndex, false);
            if (!IsVarName(token)) throw new Exception("ParseAssign: Wymagana nazwa zmiennej, " + token);
            StmtTable.StmtTable.Instance.InsertStmt(Enums.EntityTypeEnum.Assign, lineNumber);
            startIndex = endIndex;

            TNODE assignNode = AST.AST.Instance.CreateTNode(Enums.EntityTypeEnum.Assign); // tworzenie node dla assign
            VarTable.Variable var = new VarTable.Variable(token);
            VarTable.VarTable.Instance.InsertVar(token);
            StmtTable.StmtTable.Instance.SetAstRoot(lineNumber, assignNode);
            AST.AST.Instance.SetParent(assignNode, parent); //ustawianie parenta dla assign

            TNODE stmtListNode = AST.AST.Instance.GetNthChild(0, parent);
            SettingFollows(assignNode, stmtListNode);
            AST.AST.Instance.SetChildOfLink(assignNode, stmtListNode); //łączenie stmlList z assign
            TNODE variableNode = AST.AST.Instance.CreateTNode(Enums.EntityTypeEnum.Variable); // tworzenie node dla zmiennej po lewej stronie assign node
            AST.AST.Instance.SetChildOfLink(variableNode, assignNode);
            SetModifiesForFamily(assignNode, var); // ustawianie Modifies

            token = GetToken(lines, ref lineNumber, startIndex, out endIndex, false);
            if (token != "=") throw new Exception("ParseAssign: Brak znaku =");
            startIndex = endIndex;

            token = "";
            bool expectedOperation = false;
            TNODE expressionRoot = null; // zmienna przechowująca aktualny wierzchołek expression po prawej stronie assign
            while (lineNumber < lines.Count)
            {
                token = GetToken(lines, ref lineNumber, startIndex, out endIndex, false);
                startIndex = endIndex;
                if (expectedOperation)
                {
                    switch (token)
                    {
                        case "+":
                            TNODE oldAssignRoot = AST.AST.Instance.GetTNodeDeepCopy(expressionRoot);
                            expressionRoot = AST.AST.Instance.CreateTNode(Enums.EntityTypeEnum.Plus);
                            AST.AST.Instance.SetChildOfLink(oldAssignRoot, expressionRoot);
                            break;
                        default:
                            throw new Exception("ParseAssign: Nieobsługiwane działanie, " + token);
                    }
                    expectedOperation = false;
                }
                else
                {
                    if (IsVarName(token))
                    {
                        if (expressionRoot == null)
                        {
                            expressionRoot = AST.AST.Instance.CreateTNode(Enums.EntityTypeEnum.Variable);

                            VarTable.Variable usesVar = new VarTable.Variable(token); // ustawianie Uses
                            SetUsesForFamily(assignNode, usesVar);
                        }
                        else
                        {
                            TNODE rightSide = AST.AST.Instance.CreateTNode(Enums.EntityTypeEnum.Variable);
                            AST.AST.Instance.SetChildOfLink(rightSide, expressionRoot);

                            VarTable.Variable usesVar = new VarTable.Variable(token); // ustawianie Uses
                            SetUsesForFamily(assignNode, usesVar);
                        }
                    }
                    else if (IsConstValue(token))
                    {
                        if (expressionRoot == null) expressionRoot = AST.AST.Instance.CreateTNode(Enums.EntityTypeEnum.Constant);
                        else
                        {
                            TNODE rightSide = AST.AST.Instance.CreateTNode(Enums.EntityTypeEnum.Constant);
                            AST.AST.Instance.SetChildOfLink(rightSide, expressionRoot);
                        }
                    }
                    else throw new Exception("ParseAssign: Spodziewana zmienna lub stała, " + token);
                    expectedOperation = true;
                }
                token = GetToken(lines, ref lineNumber, startIndex, out endIndex, true);
                if (token == ";")
                {
                    token = GetToken(lines, ref lineNumber, startIndex, out endIndex, false);
                    break;
                }
            }
            if (lineNumber == lines.Count && token != ";") throw new Exception("ParseAssign: Spodziewano się znaku ;");

            //łączenie tyci drzewka expresion z assign
            AST.AST.Instance.SetChildOfLink(expressionRoot, assignNode);
        }


        /// <summary>
        /// Parsowanie
        /// </summary>
        /// <param name="lines">linie kodu SIMPLE</param>
        /// <param name="lineNumber">aktualnie przetwarzana linia</param>
        /// <param name="startIndex">indeks od którego ma zacząć czytać w danej linii</param>
        /// <param name="endIndex">indeks, na którym skończyło czytać w danej linii</param>
        /// <param name="procedureName">nazwa przetwarzanej procedury</param>
        public void Parse(List<string> lines, int startIndex, ref int lineNumber, out int endIndex, string procedureName, TNODE parent)
        {
            string token = GetToken(lines, ref lineNumber, startIndex, out endIndex, true);              
            switch(token)
            {
                case "procedure":
                    ParseProcedure(lines, startIndex, ref lineNumber, out endIndex);
                    break;
                case "{":
                    ParseStmtLst(lines, startIndex, ref lineNumber, out endIndex, procedureName, parent);
                    break;
                case "while":
                    ParseWhile(lines, startIndex, ref lineNumber, out endIndex, procedureName, parent);
                    break;
                default:
                    if (IsVarName(token))
                    {
                        ParseAssign(lines, startIndex, ref lineNumber, out endIndex, procedureName, parent);
                        break;
                    }
                    else throw new Exception("Parse: Niespodziewany token: " + token);
            }
        }


        /// <summary>
        /// Sprawdzenie czy nazwa zmiennej lub procedury jest zgodna z gramatyką
        /// </summary>
        /// <param name="name">testowana nazwa</param>
        /// <returns>czy nazwa jest prawidłowa</returns>
        public bool IsVarName(string name)
        {
            if (name.Length == 0) return false;
            if (!Char.IsLetter(name[0])) return false;
            else if(reservedWords.IndexOf(name) > 0) return false;

            return true;
        }

        /// <summary>
        /// Sprawdzenie czy zmienna jest stałą
        /// </summary>
        /// <param name="name">testowana nazwa</param>
        /// <returns>czy nazwa jest prawidłowa</returns>
        public bool IsConstValue(string name)
        {
            long test;
            if (name.Length == 0) return false;
            return Int64.TryParse(name, out test);
        }


        /// <summary>
        /// Odczytanie pliku z kodem SIMPLE
        /// </summary>
        /// <param name="filename">ścieżka do pliku</param>
        public void ReadFile(string filename) 
        {
            List<string> lines = File.ReadLines(filename).ToList();
            if (lines.Count == 0) throw new Exception("ReadFile: Pusty plik");

            int lineNumber = 0;
            int index = 0;
            int endIndex;
            string token = GetToken(lines, ref lineNumber, index, out endIndex, true);
            if(token != "procedure") throw new Exception("ReadFile: Spodziewano się słowa kluczowego procedure");
            Parse(lines, index, ref lineNumber, out endIndex, "", null);

        }
    }
}
