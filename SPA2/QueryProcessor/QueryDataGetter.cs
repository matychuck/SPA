using System;
using SPA2.Enums;
using System.Collections.Generic;
using SPA2.AST;
using SPA2.VarTable;
using SPA2.ProcTable;
using SPA2.StmtTable;
using System.Linq;

namespace SPA2.QueryProcessor
{
	public static class QueryDataGetter
	{
		private static Dictionary<string, List<int>> variableIndexes = new Dictionary<string, List<int>>();
		private static int Sum = -1;
		private static bool AlgorithmNotEnd = true; 

		public static void GetData()
		{
			InsertIndexesIntoVarTables();
			Dictionary<string, string[]> queryDetails = QueryProcessor.GetQueryDetails();
			List<string> suchThatPart = new List<string>();
			try
            {
				suchThatPart = new List<string>(queryDetails["SUCH THAT"]);
			} catch(Exception) { };

			//Algorithm here...
			if(suchThatPart.Count > 0)
				do
				{
					
					foreach(string method in suchThatPart)
					{
						if(method.Length > 0)
							DecodeMethod(method);
					}
					CheckSum();
				} while(AlgorithmNotEnd);

			//After algorithm....
			SendDataToPrint();
		}

		private static void InsertIndexesIntoVarTables()
		{
			Dictionary<string, List<string>> varAttributes = QueryProcessor.GetVarAttributes();
			
			foreach (KeyValuePair<string, EntityTypeEnum> oneVar in QueryProcessor.GetQueryVars())
			{
				Dictionary<string, List<string>> attributes = new Dictionary<string, List<string>>();
				foreach(KeyValuePair<string, List<string>> entry in varAttributes) 
				{
					string[] attrSplitted = entry.Key.Split(new string[] {"."}, System.StringSplitOptions.RemoveEmptyEntries);
					if(oneVar.Key == attrSplitted[0])
						attributes.Add(attrSplitted[1], entry.Value);
				}		

				switch (oneVar.Value)
				{
					case EntityTypeEnum.Procedure:
						variableIndexes.Add(oneVar.Key, GetProcedureIndexes());
						break;

					case EntityTypeEnum.Variable:
						variableIndexes.Add(oneVar.Key, GetVariableIndexes(attributes));
						break;

					case EntityTypeEnum.Assign:
						variableIndexes.Add(oneVar.Key, GetStatementIndexes(attributes, EntityTypeEnum.Assign));
						break;

					case EntityTypeEnum.If:
						variableIndexes.Add(oneVar.Key, GetStatementIndexes(attributes, EntityTypeEnum.If));
						break;

					case EntityTypeEnum.While:
						variableIndexes.Add(oneVar.Key, GetStatementIndexes(attributes, EntityTypeEnum.While));
						break;

					case EntityTypeEnum.Statement:
						variableIndexes.Add(oneVar.Key, GetStatementIndexes(attributes, EntityTypeEnum.Statement));
						break;
					default:
						throw new System.ArgumentException("Wrong typo!");
				}
			}
		}

		private static List<int> GetProcedureIndexes()
		{
			List<int> indexes = new List<int>();
			foreach (Procedure p in ProcTable.ProcTable.Instance.Procedures)
			{
				indexes.Add(p.Index);
			}
			return indexes;
		}
		
		private static List<int> GetVariableIndexes(Dictionary<string, List<string>> attributes)
		{
			List<int> indexes = new List<int>();
			List<string> varName = new List<string>();

			if(attributes.ContainsKey("varname"))
				varName = attributes["varname"];
			if(varName.Count > 1)
				return indexes;
			
			foreach(Variable v in VarTable.VarTable.Instance.Variables)
            {
				if(varName.Count == 1)
				{
					if(v.Name == varName[0])
						indexes.Add(v.Index);
				}
				else
					indexes.Add(v.Index);
            }

			return indexes;
		}

		private static List<int> GetStatementIndexes(Dictionary<string, List<string>> attributes, EntityTypeEnum enumType)
		{
			List<int> indexes = new List<int>();
			List<string> stmtNr = new List<string>();

			if(attributes.ContainsKey("stmt#"))
				stmtNr = attributes["stmt#"];
			
			if(stmtNr.Count > 1)
				return indexes;

			if(stmtNr.Count != 1)
				foreach (Statement s in StmtTable.StmtTable.Instance.Statements)
				{
					if (s.Type == enumType)
						indexes.Add(s.CodeLine);
					else if (enumType == EntityTypeEnum.Statement)
						indexes.Add(s.CodeLine);
				}
			else
			{
				try
				{
					indexes.Add(StmtTable.StmtTable.Instance.GetStmt(Int32.Parse(stmtNr[0])).CodeLine);
				} catch (Exception e)
				{
					throw new ArgumentException("Wrong stmt# = '{0}'", stmtNr[0]);
				}
			}
				

			return indexes;
		}

		private static void SendDataToPrint()
        {
			string[] varsToSelect = QueryProcessor.GetVarToSelect();
			Dictionary<string, List<int>> varIndexesToPrint = new Dictionary<string, List<int>>();
			foreach(string var in varsToSelect)
            {
				string trimedVar = var.Trim();
				varIndexesToPrint.Add(trimedVar, variableIndexes[trimedVar]);
            }

			ResultPrinter.Print(varIndexesToPrint);
		}

		private static void CheckSum()
        {
			int TmpSum = 0;
			foreach (KeyValuePair<string, List<int>> item in variableIndexes)
            {
				TmpSum += item.Value.Count;
            }

			if (TmpSum != Sum)
				Sum = TmpSum;
			else
				AlgorithmNotEnd = false;
		}

		private static void DecodeMethod(string method)
        {
			string[] typeAndArguments = method.Split(new string[] { " ", "(", ")", "," }, System.StringSplitOptions.RemoveEmptyEntries);
            switch(typeAndArguments[0].ToLower()){
				case "modifies":
					QueryMethodChecker.CheckModifiesOrUses(typeAndArguments[1], typeAndArguments[2], Modifies.Modifies.Instance.IsModified, Modifies.Modifies.Instance.IsModified);
					break;
				case "uses":
					QueryMethodChecker.CheckModifiesOrUses(typeAndArguments[1], typeAndArguments[2], Uses.Uses.Instance.IsUsed, Uses.Uses.Instance.IsUsed);
					break;
				case "parent":
					QueryMethodChecker.CheckParentOrFollows(typeAndArguments[1], typeAndArguments[2], AST.AST.Instance.IsParent);
					break;
				case "parent*":
					QueryMethodChecker.CheckParentOrFollows(typeAndArguments[1], typeAndArguments[2], AST.AST.Instance.IsParentStar);
					break;
				case "follows":
					QueryMethodChecker.CheckParentOrFollows(typeAndArguments[1], typeAndArguments[2], AST.AST.Instance.IsFollowed);
					break;
				case "follows*":
					QueryMethodChecker.CheckParentOrFollows(typeAndArguments[1], typeAndArguments[2], AST.AST.Instance.IsFollowedStar);
					break;
				default:
					throw new ArgumentException("Niepoprawna metoda: \"{0}\"", typeAndArguments[0]);
            }
		}

		public static List<int> GetArgIndexes(string var)
        {
			return variableIndexes[var];
        }

		public static void RemoveIndexesFromLists(string firstArgument, string secondArgument, List<int> firstList, List<int> secondList)
        {
			variableIndexes[firstArgument] = variableIndexes[firstArgument].Where(i => firstList.Any(j => j == i)).Distinct().ToList();
			variableIndexes[secondArgument] = variableIndexes[secondArgument].Where(i => secondList.Any(j => j == i)).Distinct().ToList();
		}
	}
}