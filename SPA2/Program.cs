using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPA2.AST;
using SPA2.Enums;
using SPA2.Interfaces;
using SPA2.VarTable;
using SPA2.Parser;

namespace SPA2.AST
{
    class Program
    {
        static void Main(string[] args)
        {
            // tylko do testow
            Parser.Parser parser = new Parser.Parser();
            parser.ReadFile("C:\\Users\\Agnieszka\\Documents\\Informatyka\\studia\\magisterka\\Analiza i testowanie systemow informatycznych\\simpleCode1.txt");


        }
    }
}
