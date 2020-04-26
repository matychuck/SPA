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
        /*private const string RUN_START = "Uruchamianie...";
        private const string RUN_ERROR = "Błąd uruchamiania: ";
        private const string COMPILE_START = "Rozpoczęcie kompilacji...";
        private const string COMPILE_END = "Kompilacja przebiegła pomyślnie";
        private const string COMPILE_ERROR = "Błąd kompilacji: ";
        private Dictionary<int, int> errorRangeText = new Dictionary<int, int>();
        */
        [STAThread]
        static void Main(string[] args)
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            /*
            
         */
        }
    }
}
