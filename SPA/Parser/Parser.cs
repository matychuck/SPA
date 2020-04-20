using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPA.AST;

namespace SPA.Parser
{
    public class Parser
    {
        List<String> reservedWords; // lista słów kluczowych
        public Parser()
        {
            reservedWords = new List<string>();
            reservedWords.Add("procedure");
            reservedWords.Add("while");
            reservedWords.Add("if");
            reservedWords.Add("then");
            reservedWords.Add("else");
            reservedWords.Add("call");
        }


        /// <summary>
        /// wczytanie znaku z linii
        /// </summary>
        /// <param name="line">odczytywana linia</param>
        /// <param name="index">indeks, ktory ma byc wczytany</param>
        /// <returns></returns>
        public char GetChar(string line, int index)
        {
            char character = line[index];
            if ((char)9 == character)
                character = ' ';
            return character;
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
                throw new Exception("ParseProcedure: Nieoczekiwany koniec pliku, linia: "+lineNumber);
            }

            string token = "";
            char character;
            while (true)
            {
                fileLine = lines.ElementAt(lineNumber);
                if (startIndex >= fileLine.Length)
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
                while (fileLine == "")
                {
                    lineNumber++;
                    if (lineNumber >= lines.Count)
                    {
                        endIndex = -1;
                        if (test) lineNumber = lineNumberIn;
                        return "";
                    }
                    fileLine = lines.ElementAt(lineNumber);
                }

                character = GetChar(fileLine, startIndex);
                while (character == ' ')
                {
                    startIndex++;
                    if (startIndex >= fileLine.Length)
                    {
                        lineNumber++;
                        startIndex = 0;
                        if(lineNumber>=lines.Count)
                        {
                            endIndex = -1;
                            if (test) lineNumber = lineNumberIn;
                            return token;
                        }
                        break;
                    }
                    character = GetChar(fileLine, startIndex);
                }
                if (character != ' ') break;

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
            if (token != "procedure") throw new Exception("ParseProcedure: Brak słowa procedure, linia: "+lineNumber);
            startIndex = endIndex;

            token = GetToken(lines, ref lineNumber, startIndex, out endIndex, false); // wczytanie nazwy procedury
            TNODE newNode = AST.AST.Instance.CreateTNode(Enums.EntityTypeEnum.Procedure);
            if (IsVarName(token)) {
                ProcTable.ProcTable.Instance.InsertProc(token);
                ProcTable.ProcTable.Instance.SetAstRoot(token, newNode);
                if (token == "Main") AST.AST.Instance.SetRoot(newNode);
            }
            else throw new Exception("ParseProcedure: Błędna nazwa procedury, "+token+", linia: "+lineNumber);
            string procedureName = token;
            startIndex = endIndex;

            token = GetToken(lines, ref lineNumber, startIndex, out endIndex, true); // wczytanie {
            if (token != "{") throw new Exception("ParseProcedure: Brak nawiasu { po nazwie procedury, linia: "+lineNumber);

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
            if (token != "{") throw new Exception("ParseStmtLst: Brak znaku {, linia: "+lineNumber);
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
            if (lineNumber == lines.Count && token != "}") throw new Exception("ParseStmtLst: Brak znaku }, linia: "+lineNumber);
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
            if (token != "while") throw new Exception("ParseWhile: Brak słowa kluczowego while, linia: "+lineNumber);
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
                if (VarTable.VarTable.Instance.GetVarIndex(token) == -1) throw new Exception("ParseAssign: Zmienna nie została przypisana, " + token+", linia: "+lineNumber);
                else
                {
                    VarTable.Variable var = new VarTable.Variable(token);
                    SetUsesForFamily(whileNode, var);
                }
            }
            else throw new Exception("ParseWhile: Wymagana nazwa zmiennej, "+token+", linia: "+lineNumber);
            startIndex = endIndex;

            token = GetToken(lines, ref lineNumber, startIndex, out endIndex, true);
            if (token != "{") throw new Exception("ParseWhile: Brak znaku {, linia: "+lineNumber);

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
                var.Index = VarTable.VarTable.Instance.GetVarIndex(var.Name);
                Modifies.Modifies.Instance.SetModifies(proc, var);
            }
            else
            {
                StmtTable.Statement stmt = StmtTable.StmtTable.Instance.Statements.Where(i => i.AstRoot == node).FirstOrDefault();
                var.Index = VarTable.VarTable.Instance.GetVarIndex(var.Name);
                Modifies.Modifies.Instance.SetModifies(stmt, var);
            }
            if (AST.AST.Instance.GetParent(node) != null) SetModifiesForFamily(AST.AST.Instance.GetParent(node), var);
        }

        public void SetUsesForFamily(TNODE node, VarTable.Variable var)
        {
            if (node.EntityTypeEnum == Enums.EntityTypeEnum.Procedure)
            {
                ProcTable.Procedure proc = ProcTable.ProcTable.Instance.Procedures.Where(i => i.AstRoot == node).FirstOrDefault();
                var.Index = VarTable.VarTable.Instance.GetVarIndex(var.Name);
                Uses.Uses.Instance.SetUses(proc, var);
            }
            else
            {
                StmtTable.Statement stmt = StmtTable.StmtTable.Instance.Statements.Where(i => i.AstRoot == node).FirstOrDefault();
                var.Index = VarTable.VarTable.Instance.GetVarIndex(var.Name);
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
            if (!IsVarName(token)) throw new Exception("ParseAssign: Wymagana nazwa zmiennej, " + token+", linia: "+lineNumber);
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
            if (token != "=") throw new Exception("ParseAssign: Brak znaku =, linia: "+lineNumber);
            startIndex = endIndex;

            // parsowanie wszystkiego po =
            ParseExpr(lines, startIndex, ref lineNumber, out endIndex, procedureName, null);
            startIndex = endIndex;
        }

        // potem do wyrzucenia
        /// <summary>
        /// Parsowanie przypisania assign
        /// </summary>
        /// <param name="lines">linie kodu SIMPLE</param>
        /// <param name="lineNumber">aktualnie przetwarzana linia</param>
        /// <param name="startIndex">indeks od którego ma zacząć czytać w danej linii</param>
        /// <param name="endIndex">indeks, na którym skończyło czytać w danej linii</param>
        /// <param name="procedureName">nazwa przetwarzanej procedury</param>
        public void ParseAssignOld(List<string> lines, int startIndex, ref int lineNumber, out int endIndex, string procedureName, TNODE parent)
        {
            string token = GetToken(lines, ref lineNumber, startIndex, out endIndex, false);
            if (!IsVarName(token)) throw new Exception("ParseAssign: Wymagana nazwa zmiennej, " + token+", linia: "+lineNumber);
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
            if (token != "=") throw new Exception("ParseAssign: Brak znaku =, linia: "+lineNumber);
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
                    TNODE oldAssignRoot;
                    switch (token)
                    {
                        case "+":
                            oldAssignRoot = AST.AST.Instance.GetTNodeDeepCopy(expressionRoot);
                            expressionRoot = AST.AST.Instance.CreateTNode(Enums.EntityTypeEnum.Plus);
                            AST.AST.Instance.SetChildOfLink(oldAssignRoot, expressionRoot);
                            break;
                        case "-":
                            oldAssignRoot = AST.AST.Instance.GetTNodeDeepCopy(expressionRoot);
                            expressionRoot = AST.AST.Instance.CreateTNode(Enums.EntityTypeEnum.Minus);
                            AST.AST.Instance.SetChildOfLink(oldAssignRoot, expressionRoot);
                            break;
                        case "*":
                            // tworzenie drzewa dla mnozenia
                            break;
                        default:
                            throw new Exception("ParseAssign: Nieobsługiwane działanie, " + token+", linia: "+lineNumber);
                    }
                    expectedOperation = false;
                }
                else // factor z gramatyki
                {
                    if (IsVarName(token)) // spodziewana nazwa zmiennej
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
                    else if (IsConstValue(token)) // spodziewana nazwa stalej
                    {
                        if (expressionRoot == null) expressionRoot = AST.AST.Instance.CreateTNode(Enums.EntityTypeEnum.Constant);
                        else
                        {
                            TNODE rightSide = AST.AST.Instance.CreateTNode(Enums.EntityTypeEnum.Constant);
                            AST.AST.Instance.SetChildOfLink(rightSide, expressionRoot);
                        }
                    }
                    else throw new Exception("ParseAssign: Spodziewana zmienna lub stała, " + token+", linia: "+lineNumber);
                    expectedOperation = true;
                }
                token = GetToken(lines, ref lineNumber, startIndex, out endIndex, true);
                if (token == ";")
                {
                    token = GetToken(lines, ref lineNumber, startIndex, out endIndex, false);
                    break;
                }
            }
            if (lineNumber == lines.Count && token != ";") throw new Exception("ParseAssign: Spodziewano się znaku ; linia: "+lineNumber);

            //łączenie tyci drzewka expresion z assign
            AST.AST.Instance.SetChildOfLink(expressionRoot, assignNode);
        }

        /// <summary>
        /// Parsowanie expr z gramatyki
        /// </summary>
        /// <param name="lines">linie kodu SIMPLE</param>
        /// <param name="lineNumber">aktualnie przetwarzana linia</param>
        /// <param name="startIndex">indeks od którego ma zacząć czytać w danej linii</param>
        /// <param name="endIndex">indeks, na którym skończyło czytać w danej linii</param>
        /// <param name="procedureName">nazwa przetwarzanej procedury</param>
        /// <returns>czy wykryto zakonczenie przypisania assign</returns>
        public bool ParseExpr(List<string> lines, int startIndex, ref int lineNumber, out int endIndex, string procedureName, TNODE parent)
        {
            bool endAssign=false; // czy wykryto zakonczenie assign
            string token = "";
            endIndex = startIndex;
            bool expectedOperation = false; // czy spodziewane jest dzialanie: +, -, *, ()
            bool possibleBracketClose = false; // czy mozliwy jest )
            bool bracketsPaired = true; // czy nawiasy sa parami
            int tokenCount = 0; // liczba wczytanych tokenow
            while (lineNumber < lines.Count)
            {
                token = GetToken(lines, ref lineNumber, startIndex, out endIndex, true); // sprawdzenie kolejnego tokena
                tokenCount++;
                if (expectedOperation)
                {
                    token = GetToken(lines, ref lineNumber, startIndex, out endIndex, false); // pobranie tokena
                    startIndex = endIndex;
                    switch (token)
                    {
                        case "+":

                            expectedOperation = false;
                            break;
                        case "-":

                            expectedOperation = false;
                            break;
                        case "*":

                            expectedOperation = false;
                            break;
                        case ")":
                            if (!possibleBracketClose) throw new Exception("ParseExpr: niespodziewany znak ), linia: "+lineNumber);


                            bracketsPaired = true;
                            expectedOperation = true;
                            break;
                        default:
                            throw new Exception("ParseExpr: Nieobsługiwane działanie, " + token+", linia: "+lineNumber);
                    } 
                }
                else // factor z gramatyki
                {
                    if (IsVarName(token)) // spodziewana nazwa zmiennej
                    {
                        token = GetToken(lines, ref lineNumber, startIndex, out endIndex, false);
                        startIndex = endIndex;
                        expectedOperation = true;
                    }
                    else if (IsConstValue(token)) // spodziewana nazwa stalej
                    {
                        token = GetToken(lines, ref lineNumber, startIndex, out endIndex, false);
                        startIndex = endIndex;
                        expectedOperation = true;
                    }
                    else if (token == "(")
                    {
                        if (tokenCount == 1)
                        {
                            possibleBracketClose = true;
                            bracketsPaired = false;
                            token = GetToken(lines, ref lineNumber, startIndex, out endIndex, false);
                            startIndex = endIndex;
                        }
                        else
                        {
                            // parsowanie po (
                            endAssign = ParseExpr(lines, startIndex, ref lineNumber, out endIndex, procedureName, null);
                            startIndex = endIndex;
                            expectedOperation = true;
                            if (endAssign)
                            {
                                token = ";";
                                break;
                            }
                        }
                    }
                    else throw new Exception("ParseExpr: Spodziewana zmienna lub stała, " + token + ", linia: " +lineNumber);
                }

                token = GetToken(lines, ref lineNumber, startIndex, out endIndex, true);
                if (token == ";")
                {
                    if(!bracketsPaired) throw new Exception("ParseExpr: Brak nawiasu zamykajacego, wystapil " + token + ", linia: " + lineNumber);
                    token = GetToken(lines, ref lineNumber, startIndex, out endIndex, false);
                    endAssign = true;
                    break;
                }
            }
            if (lineNumber == lines.Count && token != ";") throw new Exception("ParseExpr: Spodziewano się znaku ; linia: "+lineNumber);
            return endAssign;
        }

        /// <summary>
        /// Parsowanie call
        /// </summary>
        /// <param name="lines">linie kodu SIMPLE</param>
        /// <param name="lineNumber">aktualnie przetwarzana linia</param>
        /// <param name="startIndex">indeks od którego ma zacząć czytać w danej linii</param>
        /// <param name="endIndex">indeks, na którym skończyło czytać w danej linii</param>
        /// <param name="procedureName">nazwa przetwarzanej procedury</param>
        public void ParseCall(List<string> lines, int startIndex, ref int lineNumber, out int endIndex, string procedureName, TNODE parent)
        {
            string token = GetToken(lines, ref lineNumber, startIndex, out endIndex, false);
            if (token != "call") throw new Exception("ParseCall: Brak słowa kluczowego call, linia: "+lineNumber);

            startIndex = endIndex;

            token = GetToken(lines, ref lineNumber, startIndex, out endIndex, false);
            if (IsVarName(token))
            {
                
            }
            else throw new Exception("ParseCall: Wymagana nazwa procedury, " + token+", linia: "+lineNumber);
            startIndex = endIndex;

            token = GetToken(lines, ref lineNumber, startIndex, out endIndex, false);
            startIndex = endIndex;
            if (token != ";") throw new Exception("ParseCall: Brak znaku ; linia: "+lineNumber);
        }


        /// <summary>
        /// Parsowanie if then else
        /// </summary>
        /// <param name="lines">linie kodu SIMPLE</param>
        /// <param name="lineNumber">aktualnie przetwarzana linia</param>
        /// <param name="startIndex">indeks od którego ma zacząć czytać w danej linii</param>
        /// <param name="endIndex">indeks, na którym skończyło czytać w danej linii</param>
        /// <param name="procedureName">nazwa przetwarzanej procedury</param>
        public void ParseIf(List<string> lines, int startIndex, ref int lineNumber, out int endIndex, string procedureName, TNODE parent)
        {
            string token = GetToken(lines, ref lineNumber, startIndex, out endIndex, false);
            if (token != "if") throw new Exception("ParseIf: Brak słowa kluczowego if, linia: " + lineNumber);
            
            startIndex = endIndex;

            token = GetToken(lines, ref lineNumber, startIndex, out endIndex, false);
            if (IsVarName(token))
            {
               
            }
            else throw new Exception("ParseIf: Wymagana nazwa zmiennej, " + token+", linia: " + lineNumber);
            startIndex = endIndex;

            // pobranie THEN
            token = GetToken(lines, ref lineNumber, startIndex, out endIndex, false);
            if (token != "then") throw new Exception("ParseIf: Brak słowa kluczowego then, linia: " + lineNumber);
            startIndex = endIndex;

            // sprawdzenie czy jest {
            token = GetToken(lines, ref lineNumber, startIndex, out endIndex, true);
            if (token != "{") throw new Exception("ParseIf: Brak znaku {, linia: " + lineNumber);

            // parsowanie STMTLST po THEN
            Parse(lines, startIndex, ref lineNumber, out endIndex, procedureName, parent);
            startIndex = endIndex;

            // musi wystąpić ELSE
            token = GetToken(lines, ref lineNumber, startIndex, out endIndex, false);
            if (token != "else") throw new Exception("ParseIf: Brak słowa kluczowego else, linia: "+lineNumber);
            startIndex = endIndex;

            // sprawdzenie czy jest {
            token = GetToken(lines, ref lineNumber, startIndex, out endIndex, true);
            if (token != "{") throw new Exception("ParseIf: Brak znaku {, linia: " + lineNumber);

            // parsowanie STMTLST po ELSE
            Parse(lines, startIndex, ref lineNumber, out endIndex, procedureName, parent);
            startIndex = endIndex;
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
                case "call":
                    ParseCall(lines, startIndex, ref lineNumber, out endIndex, procedureName, parent);
                    break;
                case "if":
                    ParseIf(lines, startIndex, ref lineNumber, out endIndex, procedureName, parent);
                    break;
                default:
                    if (IsVarName(token))
                    {
                        ParseAssign(lines, startIndex, ref lineNumber, out endIndex, procedureName, parent);
                        break;
                    }
                    else throw new Exception("Parse: Niespodziewany token: " + token+", linia: "+lineNumber);
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
        public void StartParse(string code) 
        {

            List<string> lines = code.Split(new[] { '\r', '\n' }).ToList();

            if (lines.Count == 0 || (lines.Count == 1 && string.IsNullOrEmpty(lines[0])))
                
                throw new Exception("StartParse: Pusty kod");

            int lineNumber = 0;
            int index = 0;
            int endIndex;
            string token;
            int countToken = 0;
            while (lineNumber < lines.Count) // parsowanie programu - min 1 procedura
            {
                token = GetToken(lines, ref lineNumber, index, out endIndex, true);
                if (token != "") countToken++;
                if (token == "")
                {
                    if (countToken > 0) break; // nastapil koniec pliku
                    else throw new Exception("StartParse: Pusty kod");
                }

                if (token != "procedure") throw new Exception("StartParse: Spodziewano się słowa kluczowego procedure, linia: " + lineNumber);
                Parse(lines, index, ref lineNumber, out endIndex, "", null);
                index = endIndex;
            }
        }

        public void CleanData()
        {
            AST.AST.Instance.Root = null;
            VarTable.VarTable.Instance.Variables.Clear();
            StmtTable.StmtTable.Instance.Statements.Clear();
            ProcTable.ProcTable.Instance.Procedures.Clear();
        }
    }
}
