using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SPA.Parser;
using SPA.QueryProcessor;

namespace SPAPipeTester
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {             
                string SimpleCode = File.ReadAllText(args[0]); //pobieranie nazwy pliku z kodem simple
                SimpleCode = Regex.Replace(SimpleCode, @"\r", ""); // usunięcie nowej lini
                Parser Parser = new Parser();
                Parser.CleanData();
                Parser.StartParse(SimpleCode);
                Console.WriteLine("Ready"); //informacja dla PipeTestera, że może wprowadzać zapytania PQL

                string variables;
                string query;
                string PQL;
                List<string> results;

                while(true){
                     variables = Console.ReadLine();
                     query = Console.ReadLine();
                     PQL = variables + query;
                     results = QueryProcessor.ProcessQuery(PQL, testing: true);
                     if(results.Count == 0)
                        Console.WriteLine("none");
                     else
                     {
                        Console.WriteLine(string.Join(", ", results));
                     }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("# " + e.Message);
            }
        }
    }
}
