using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPA.QueryProcessor;
using System.IO;
using System.Text.RegularExpressions;
using SPA.Parser;
using System.Collections.Generic;

namespace SPATests
{
    [TestClass]
    public class SPAFollowsTests
    {
        private string testFollows1 = "stmt s,s1; assign a; variable v; Select s such that Follows(s,s1) with s.stmt#= 3";

        [TestMethod]
        public void TestMethod1()
        {
            List<int> results = new List<int>();
            List<int> expectedResults = new List<int>() { 3 };
            string SimpleCode = File.ReadAllText(@"C:\Main.txt"); //pobieranie nazwy pliku z kodem simple
            SimpleCode = Regex.Replace(SimpleCode, @"\r", ""); // usuniêcie nowej lini
            Parser Parser = new Parser();
            Parser.CleanData();
            Parser.StartParse(SimpleCode);
            results = QueryProcessor.ProcessQuery(testFollows1);
            CollectionAssert.AreEqual(expectedResults, results,"Wyniki nie s¹ poprawne!");
        }
    }
}
