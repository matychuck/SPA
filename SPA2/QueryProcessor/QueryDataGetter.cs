using System;
using SPA2.Enums;
using System.Collections.Generic;

namespace SPA2.QueryProcessor
{
	public static class QueryDataGetter
	{
		private static Dictionary<string, int[]> variableIndexes = new Dictionary<string, int[]>();
		public static void GetData()
		{
			InsertIndexesIntoVarTables();
			
			//Algorithm here...
			//foreach(KeyValuePair<string, int[]> item in variableIndexes)
   //         {
			//	Console.WriteLine(item.Key);
   //         }
			//After algorithm....
			SendDataToPrint();
		}

		private static void InsertIndexesIntoVarTables()
		{
			Dictionary<string, List<string>> varAttributes = QueryProcessor.GetVarAttributes();

			foreach (KeyValuePair<string, EntityTypeEnum> oneVar in QueryProcessor.GetQueryVars())
			{
				List<string> attributes = null;

				if (varAttributes.ContainsKey(oneVar.Key))
					attributes = varAttributes[oneVar.Key];

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

		private static int[] GetProcedureIndexes()
		{
			return new int[] { 1, 2, 3 };
		}

		private static int[] GetVariableIndexes(List<string> attributes)
		{
			return new int[] { 1, 2, 3 };
		}

		private static int[] GetStatementIndexes(List<string> attributes, EntityTypeEnum enumType)
		{
			return new int[] { 1, 2, 3 };
		}

		private static void SendDataToPrint()
        {
			string[] varsToSelect = QueryProcessor.GetVarToSelect();
			Dictionary<string, int[]> varIndexesToPrint = new Dictionary<string, int[]>();
			foreach(string var in varsToSelect)
            {
				string trimedVar = var.Trim();
				varIndexesToPrint.Add(trimedVar, variableIndexes[trimedVar]);
            }

			ResultPrinter.Print(varIndexesToPrint);
		}
}
}