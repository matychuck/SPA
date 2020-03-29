using System;
using SPA2.Enums;
using System.Collections.Generic;

namespace SPA2.QueryProcessor
{
    public class QueryProcessor
    {
        static Dictionary<string, EntityTypeEnum> vars = new Dictionary<string, EntityTypeEnum>();

        public static void processQuery(String query)
        {
            //Console.WriteLine(query);
            string[] queryParts = query.Split(';');

            String selectPart = queryParts[queryParts.Length - 1];

            Array.Resize(ref queryParts, queryParts.Length - 2);
            foreach (String varsDefinition in queryParts) {
                decodeVarDefinitionAndInsertToDict(varsDefinition.Trim());
            }

            printParsingResults();
        }

        private static void decodeVarDefinitionAndInsertToDict(String varsDefinition)
        {
            string[] varsParts = varsDefinition.Replace(" ", ",").Split(','); 
            string varTypeAsString = varsParts[0]; //Typ jako string
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
                default:
                     throw new System.ArgumentException("Wrong typo!");
            }

            for(int i = 1; i < varsParts.Length; i++) {
                if(varsParts[i] != "") //tak nawet takie coś jak "" dodawało...
                    vars.Add(varsParts[i], typeEnum);
            }
        }

        private static void printParsingResults()
        {
            Console.WriteLine("Parsing ended");
            foreach (KeyValuePair<string, EntityTypeEnum> oneVar in vars){
                Console.WriteLine("\t{0} - {1}", oneVar.Key, oneVar.Value);
            }
        }
    }

}
