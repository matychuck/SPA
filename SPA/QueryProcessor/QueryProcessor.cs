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
        private static Dictionary<string, string[]> queryDetails = null;

        private static void Init()
        {
            vars = new Dictionary<string, EntityTypeEnum>();
            queryDetails = new Dictionary<string, string[]>();

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
                default:
                     throw new System.ArgumentException("# Wrong argument: \"{0}\"", varTypeAsString);
            }

            for(int i = 1; i < varsParts.Length; i++) {
                if(varsParts[i] != "") //tak nawet takie coś jak "" dodawało...
                    vars.Add(varsParts[i], typeEnum);
            }
        }

        private static void ProcessSelectPart(string selectPart)
        {
            string[] separatingStrings = { "select", "such that", "with" };
            string[] separatedQuery = selectPart.ToLower().Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
            AddQueryDetailsToDict(separatedQuery);
        }

        private static void AddQueryDetailsToDict(string[] separatedQuery)
        {
            string[] dictKeys = { "SELECT", "SUCH THAT", "WITH" };
            
            string selectVars = separatedQuery[0].Trim();
            string[] separatingStrings = { ",", " ",};
            string[] selectVarsInTable = selectVars.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
            queryDetails.Add("SELECT", selectVarsInTable);
            
            for(int i = 1; i < separatedQuery.Length; i++)
            {
                string[] words = separatedQuery[i].Split(new string[] { "and", }, System.StringSplitOptions.RemoveEmptyEntries);
                for(int j = 0; j < words.Length; j++)
                {
                    words[j] = words[j].Trim();
                }
                queryDetails.Add(dictKeys[i], words);
            }
        }

        public static Dictionary<string, EntityTypeEnum> GetQueryVars()
        {
            return vars;
        }

        public static Dictionary<string, string[]> GetQueryDetails()
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

        public static string[] GetVarToSelect() {
            return queryDetails["SELECT"];
        }

        public static EntityTypeEnum GetVarEnumType(string var)
        {
            return vars[var];
        }
    } 
}
