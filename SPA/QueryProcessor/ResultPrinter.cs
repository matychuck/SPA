﻿using System;
using System.Collections.Generic;

namespace SPA.QueryProcessor
{
	public static class ResultPrinter
	{
		public static void Print(Dictionary<string, List<int>> resultToPrint)
		{
			Console.WriteLine("\n* * * RESULTS * * *");
			foreach(KeyValuePair<string, List<int>> oneVar in resultToPrint)
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