using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPA.Client
{
    public class Program
    {
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        //[STAThread]
        private const string RUN_START = "Uruchamianie...";
        private const string RUN_ERROR = "Błąd uruchamiania: ";
        private const string COMPILE_START = "Rozpoczęcie kompilacji...";
        private const string COMPILE_END = "Kompilacja przebiegła pomyślnie";
        private const string COMPILE_ERROR = "Błąd kompilacji: ";
        private Dictionary<int, int> errorRangeText = new Dictionary<int, int>();

        //[STAThread]
        static void Main(string[] args)
        {
            try
            {
                string SimpleCode = File.ReadAllText(args[0]); //pobieranie nazwy pliku z kodem simple
                SimpleCode = Regex.Replace(SimpleCode, @"\r", ""); // usunięcie nowej lini
                Parser.Parser Parser = new Parser.Parser();
                Parser.CleanData();
                Parser.StartParse(SimpleCode);
                Console.WriteLine("Ready"); //informacja dla PipeTestera, że może wprowadzać zapytania PQL
                string variables = Console.ReadLine();
                string query = Console.ReadLine();
                string PQL = variables + query;
                QueryProcessor.QueryProcessor.ProcessQuery(PQL);
            }
            catch(Exception e)
            {
                Console.WriteLine("# " + e.Message);
            }
                     
        }
    }
}
