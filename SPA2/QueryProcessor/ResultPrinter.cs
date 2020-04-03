using System;
using System.Collections.Generic;

namespace SPA2.QueryProcessor
{
	public static class ResultPrinter
	{
		public static void Print(Dictionary<string, int[]> resultToPrint)
		{
			Console.WriteLine("\n* * * RESULTS * * *");
			foreach(KeyValuePair<string, int[]> oneVar in resultToPrint)
            {
				Console.WriteLine("-------------------");
				Console.WriteLine("{0}: ", oneVar.Key);
				foreach(int codeLine in oneVar.Value)
                {
					PrintCodeLine(codeLine);
                }

            }

		}

		private static void PrintCodeLine(int number)
        {
			Console.WriteLine(">>>> {0}", number);
        }
	}

}
