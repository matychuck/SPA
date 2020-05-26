using System;
using System.Collections.Generic;
using SPA.Enums;
using SPA.VarTable;
using SPA.ProcTable;
using System.Linq;

namespace SPA.QueryProcessor
{
	public static class ResultPrinter
	{
		public static List<int> Print(Dictionary<string, List<int>> resultToPrint)
		{
            List<int> results = new List<int>();
            int i = 0;
            //check boolean
            /*
            if(resultToPrint.ContainsKey("BOOLEAN"))
                if(resultToPrint["BOOLEAN"].Count > 0)
                {
                    Console.WriteLine("\n\tTRUE\n");
                    return new List<int>(new int[] {1});
                } else {
                    Console.WriteLine("\n\tFALSE\n");
                    return new List<int>(new int[] {1});
                } */

			foreach(KeyValuePair<string, List<int>> oneVar in resultToPrint)
            {   
                EntityTypeEnum type = QueryProcessor.GetVarEnumType(oneVar.Key);
                
                switch(type){
                    case EntityTypeEnum.Variable:
                        Console.WriteLine("Results for " + type.ToString().ToLower() + " " + oneVar.Key + ": ");
                        PrintVariables(oneVar.Value);
                        break;
                    case EntityTypeEnum.Procedure:
                        Console.WriteLine("Results for " + type.ToString().ToLower() + " " + oneVar.Key + ": ");
                        PrintProcedures(oneVar.Value);
                        break;
                    default: 
                        Console.WriteLine("Results for " + type.ToString().ToLower() + " " + oneVar.Key + " (numbers of code line):");
                        PrintStatements(oneVar.Value);
                        break;
                }

				foreach(int codeLine in oneVar.Value)
                {
                    i++;
					results.Add(codeLine);
                }

            }

            return results;

		}

		private static int PrintCodeLine(int number, bool lastResult)
        {
            if(lastResult) Console.Write("{0}", number);
            else Console.Write("{0},", number);
            return number;
        }

        private static void PrintVariables(List<int> indexes)
        {
            foreach(int index in indexes)
            {
                Console.Write("\t{0}\n", VarTable.VarTable.Instance.GetVar(index).Name);
            }
        }

        private static void PrintProcedures(List<int> indexes)
        {
            foreach(int index in indexes)
            {
                Console.Write("\t{0}()\n", ProcTable.ProcTable.Instance.GetProc(index).Name);
            }
        }

        private static void PrintStatements(List<int> indexes)
        {
            foreach(int index in indexes)
            {
                Console.Write("\t{0}\n", index);
            }
        }
	}

}
