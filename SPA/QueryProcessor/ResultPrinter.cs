using System;
using System.Collections.Generic;

namespace SPA.QueryProcessor
{
	public static class ResultPrinter
	{
		public static List<int> Print(Dictionary<string, List<int>> resultToPrint)
		{
            List<int> results = new List<int>();
            int i = 0;
			foreach(KeyValuePair<string, List<int>> oneVar in resultToPrint)
            {
				foreach(int codeLine in oneVar.Value)
                {
                    i++;
					results.Add(PrintCodeLine(codeLine, i == resultToPrint.Count));
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
	}

}
