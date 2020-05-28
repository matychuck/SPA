using System;
using SPA.Enums;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SPA.QueryProcessor;

namespace SPA.QueryProcessor
{
    public class QueryProcessor
    {
        private static Dictionary<string, EntityTypeEnum> vars = null;
        private static Dictionary<string, List<string>> queryDetails = null;

        private static void Init()
        {
            vars = new Dictionary<string, EntityTypeEnum>();
            queryDetails = new Dictionary<string, List<string>>();

        }
        public static List<int> ProcessQuery(String query)
        {
            Init();
            query = Regex.Replace(query, @"\t|\n|\r", ""); //usunięcie znaków przejścia do nowej linii i tabulatorów
            string[] queryParts = query.Split(new [] { ';' }, StringSplitOptions.RemoveEmptyEntries);

           for(int i = 0; i < queryParts.Length - 1; i++){
                DecodeVarDefinitionAndInsertToDict(queryParts[i].Trim()); //dekoduje np. assign a, a1;
            }

            String selectPart = queryParts[queryParts.Length - 1];
            ProcessSelectPart(selectPart.Trim()); //dekoduje część "Select ... "
            //PrintParsingResults();
            return QueryDataGetter.GetData();
        }

        private static void DecodeVarDefinitionAndInsertToDict(String varsDefinition)
        {
            string[] varsParts = varsDefinition.Replace(" ", ",").Split(','); 
            string varTypeAsString = varsParts[0]; //Typ jako string (statement, assign, wgile albo procedure)
            EntityTypeEnum typeEnum;

            switch (varTypeAsString.ToLower()) {
                case "stmt":
                    typeEnum = EntityTypeEnum.Statement;
                    break;
                case "assign":
                    typeEnum = EntityTypeEnum.Assign;
                    break;
                case "while":
                    typeEnum = EntityTypeEnum.While;
                    break;
                case "procedure":
                    typeEnum = EntityTypeEnum.Procedure;
                    break;
                case "variable":
                    typeEnum = EntityTypeEnum.Variable;
                    break;
                case "constant":
                    typeEnum = EntityTypeEnum.Constant;
                    break;
                case "prog_line":
                    typeEnum = EntityTypeEnum.Prog_line;
                    break;
                default:
                     throw new System.ArgumentException(string.Format("# Wrong argument: \"{0}\"", varTypeAsString));
            }

            for(int i = 1; i < varsParts.Length; i++) {
                if(varsParts[i] != "") //tak nawet takie coś jak "" dodawało...
                    vars.Add(varsParts[i], typeEnum);
            }
        }

        private static void ProcessSelectPart(string selectPart)
        {  
            //string selectPart = "Select s, V,  b such that Modifies(s,V) and Uses(s,V) with s.stmt#=3 such that Follows(s, a) with V.varname=\"aBc\"";
            string[] splittedselectPart = Regex.Split(selectPart.ToLower(), "(such that)");
            List<string[]> splittedselectPart2 = new List<string[]>();
            List<string> splittedselectPart3 = new List<string>();
            List<string> splittedselectPart4 = new List<string>();
            queryDetails.Add("SELECT", new List<string>());
            queryDetails.Add("SUCH THAT", new List<string>());
            queryDetails.Add("WITH", new List<string>());
            

            foreach (string s in splittedselectPart)
                splittedselectPart2.Add(Regex.Split(s, "(with)"));

            foreach (string[] ss in splittedselectPart2)
                foreach (string s in ss)
                    splittedselectPart3.Add(s);

            splittedselectPart4.Add(splittedselectPart3[0]);
            for (int i = 1; i < splittedselectPart3.Count; i += 2)
                splittedselectPart4.Add(splittedselectPart3[i] + splittedselectPart3[i + 1]);


            foreach (string s in splittedselectPart4)
            {
              //  Console.WriteLine(s);
              //  Console.WriteLine(selectPart.ToLower().IndexOf(s));
                int index = selectPart.ToLower().IndexOf(s);
              //  Console.WriteLine(selectPart.Substring(index, s.Length));
              //  Console.WriteLine("=================");
                string substring;
                string[] substrings;
                string[] separator = { " and ", " And ", " ANd ", " AND ", " anD ", " aND ", " aNd ", " AnD " };
                if (s.StartsWith("such that"))
                {
                    substring = selectPart.Substring(index, s.Length).Substring(9).Trim();
                    substrings = substring.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string sbs in substrings)
                        queryDetails["SUCH THAT"].Add(sbs.Trim());
                }
                else if (s.StartsWith("with"))
                {
                    substring = selectPart.Substring(index, s.Length).Substring(4).Trim();
                    substrings = substring.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string sbs in substrings)
                        queryDetails["WITH"].Add(sbs.Trim());
                }
                else if (s.StartsWith("select"))
                {
                    substring = selectPart.Substring(index, s.Length).Substring(6).Trim();
                    substrings = substring.Split(',');
                    foreach (string sbs in substrings)
                        queryDetails["SELECT"].Add(sbs.Trim());
                }
            }


            //foreach(KeyValuePair<string, List<string>> item in queryDetails){
            //    Console.WriteLine(item.Key);
            //    foreach (string s in item.Value)
            //        Console.WriteLine("\t" + s);

            //}
            /*string[] separatingStrings = { "select", "such that", "with" };
            string[] separatedQuery = selectPart.ToLower().Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
            if(separatedQuery.Length == 3)
            {
                int withStartIndex = selectPart.ToLower().IndexOf("with", 0);
                separatedQuery[2] = selectPart.Substring(withStartIndex+4);
            }

            int ind1 = selectPart.ToLower().IndexOf(separatedQuery[0], 0);
            separatedQuery[0] = selectPart.Substring(ind1, separatedQuery[0].Length);

            if(separatedQuery.Length >= 2)
            {
                int ind2 = selectPart.ToLower().IndexOf(separatedQuery[1], 0);
                separatedQuery[1] = selectPart.Substring(ind2, separatedQuery[1].Length);
            }
            if(separatedQuery.Length >= 3)
            {
                int ind3 = selectPart.ToLower().IndexOf(separatedQuery[2], 0);
                separatedQuery[2] = selectPart.Substring(ind3, separatedQuery[2].Length);
            }
            

            AddQueryDetailsToDict(separatedQuery);*/
        }
        /*
        private static void AddQueryDetailsToDict(string[] separatedQuery)
        {
            string[] dictKeys = { "SELECT", "SUCH THAT", "WITH" };
            
            string selectVars = separatedQuery[0].Trim();
            string[] separatingStrings = { ",", " ",};
            string[] selectVarsInTable = selectVars.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
            queryDetails.Add("SELECT", selectVarsInTable);
            
            for(int i = 1; i < separatedQuery.Length; i++)
            {
                string[] words = separatedQuery[i].Split(new string[] { " and ", }, System.StringSplitOptions.RemoveEmptyEntries);
                for(int j = 0; j < words.Length; j++)
                {
                    words[j] = words[j].Trim();
                }
                queryDetails.Add(dictKeys[i], words);
            }
        } */

        private static void PrintParsingResults()
        {
            Console.WriteLine("QUERY VARIABLES:");
            foreach (KeyValuePair<string, EntityTypeEnum> oneVar in vars)
            {
                Console.WriteLine("\t{0} - {1}", oneVar.Key, oneVar.Value);
            }

            foreach (KeyValuePair<string, List<string>> oneDetail in queryDetails)
            {
                Console.WriteLine("{0}:", oneDetail.Key);
                foreach (string word in oneDetail.Value)
                {
                    Console.WriteLine("\t\"{0}\"", word);
                }

            }
        }

        public static Dictionary<string, EntityTypeEnum> GetQueryVars()
        {
            return vars;
        }

        public static Dictionary<string, List<string>> GetQueryDetails()
        {
            return queryDetails;
        }

        public static Dictionary<string,  List<string>> GetVarAttributes()
        {
            Dictionary<string, List<string>> varAttributes = new Dictionary<string, List<string>>();
            if(queryDetails.ContainsKey("WITH"))
            {
                foreach(string attribute in queryDetails["WITH"])
                {
                    string[] attribtueWithValue = attribute.Split('=');
                    if(!varAttributes.ContainsKey(attribtueWithValue[0].Trim()))
                    {
                        varAttributes[attribtueWithValue[0].Trim()] = new List<string>();
                    } 
                    varAttributes[attribtueWithValue[0].Trim()].Add(attribtueWithValue[1].Trim());
                }
            }
            return varAttributes;
        }

        public static List<string> GetVarToSelect() {
            return queryDetails["SELECT"];
        }

        public static EntityTypeEnum GetVarEnumType(string var)
        {
            try
            {
                return vars[var];
            }
            catch(Exception e)
            {
                throw new ArgumentException(string.Format("# Wrong argument: \"{0}\"", var));
            }
        }
    } 
}
