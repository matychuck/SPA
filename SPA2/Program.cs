using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPA2.AST;
using SPA2.Enums;
using SPA2.Interfaces;
using SPA2.VarTable;
using SPA2.QueryProcessor;
using SPA2.StmtTable;
namespace SPA2.AST
{
    class Program
    {
        static void Main(string[] args)
        {
            // tylko do testow
            Parser.Parser parser = new Parser.Parser();
            parser.ReadFile("C:\\Users\\Artur\\Desktop\\STUDIA =)\\AiTSI\\simpleCode1.txt");

          //  string query = "stmt s;select s";
          //  string query = "stmt s1, s2; assign a, a1; while w; procedure p;Select s1 such that Follows (s1, s2) and Parent(w, s1);";
            string query = "stmt s, s1; assign a; variable v;Select s such that Follows(s,s1) with s.stmt#= 3";
          // string query = "procedure p;Select p;";
            QueryProcessor.QueryProcessor.ProcessQuery(query);
            Console.ReadLine();
        }
    }
}
