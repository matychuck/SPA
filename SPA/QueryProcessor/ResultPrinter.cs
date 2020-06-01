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
		public static List<string> Print(Dictionary<string, List<int>> resultToPrint, bool testing)
		{
              List<string> results = new List<string>();
            //List<int> results = new List<int>();
            //int i = 0;
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
                        //Console.WriteLine("Results for " + type.ToString().ToLower() + " " + oneVar.Key + ": ");
                        results.AddRange(PrintVariables(oneVar.Value));
                        break;
                    case EntityTypeEnum.Procedure:
                        //Console.WriteLine("Results for " + type.ToString().ToLower() + " " + oneVar.Key + ": ");
                        results.AddRange(PrintProcedures(oneVar.Value));
                        break;
                    default: 
                        //Console.WriteLine("Results for " + type.ToString().ToLower() + " " + oneVar.Key + " (numbers of code line):");
                        results.AddRange(PrintStatements(oneVar.Value));
                        break;
                } 

				/*foreach(int codeLine in oneVar.Value)
                {
                    i++;
					results.Add(codeLine);
                } */
            }
            if(!testing)
                    Console.WriteLine(string.Join(", ", results));

            return results;

		}

		private static int PrintCodeLine(int number, bool lastResult)
        {
            if(lastResult) Console.Write("{0}", number);
            else Console.Write("{0},", number);
            return number;
        }

        private static List<string>PrintVariables(List<int> indexes)
        {
            List<string> results = new List<string>();
            if(indexes.Count != 0)
            {
                for (int i = 0; i < indexes.Count; i++)
                {
                    results.Add( VarTable.VarTable.Instance.GetVar(indexes[i]).Name);
                    /*Console.Write("{0}", VarTable.VarTable.Instance.GetVar(indexes[i]).Name);
                    if (i < indexes.Count - 1)
                    {
                        Console.Write(",");
                    }*/
                }
            } 
            /*else
            {
                Console.Write("none");
            }*/
            return results;
        }

        private static List<string> PrintProcedures(List<int> indexes)
        {
            List<string> results = new List<string>();
            if (indexes.Count != 0)
            {
                for (int i = 0; i < indexes.Count; i++)
                {
                    results.Add(ProcTable.ProcTable.Instance.GetProc(indexes[i]).Name);
                  /*  Console.Write("{0}", ProcTable.ProcTable.Instance.GetProc(indexes[i]).Name);
                    if (i < indexes.Count - 1)
                    {
                        Console.Write(",");
                    } */
                }
            }
           /* else
            {
                Console.Write("none");
            } */
            return results;
        }

        private static List<string> PrintStatements(List<int> indexes)
        {
            List<string> results = new List<string>();
            if (indexes.Count != 0)
            {
                for (int i = 0; i < indexes.Count; i++)
                {
                    results.Add(indexes[i].ToString());
                  /*  Console.Write("{0}", indexes[i]);
                    if (i < indexes.Count - 1)
                    {
                        Console.Write(",");
                    } */
                }
            }
            /*else
            {
                Console.Write("none");
            } */
            return results;
        }
	}

}
