using System;
using SPA2.Enums;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SPA2.QueryProcessor.patterns.common;
using SPA2.QueryProcessor.model;

namespace SPA2.QueryProcessor
{
    public class QueryProcessor
    {
        static Dictionary<string, EntityTypeEnum> vars = new Dictionary<string, EntityTypeEnum>();

        public static void processQuery(String query)
        {
            //Console.WriteLine(query);
            query = Regex.Replace(query, @"\t|\n|\r", ""); //usunięcie znaków przejścia do nowej linii i tabulatorów
            string[] queryParts = query.Split(new [] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            
           for(int i = 0; i < queryParts.Length - 2; i++){
                decodeVarDefinitionAndInsertToDict(queryParts[i].Trim()); //dekoduje np. assign a, a1;
            }
         //   printParsingResults();

            String selectPart = queryParts[queryParts.Length - 1];
            processSelectPart(selectPart.Trim());
        }

        private static void decodeVarDefinitionAndInsertToDict(String varsDefinition)
        {
            string[] varsParts = varsDefinition.Replace(" ", ",").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries); 
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
                default:
                     throw new System.ArgumentException("Wrong typo!");
            }

            for(int i = 1; i < varsParts.Length; i++) {
                if(varsParts[i] != "") //tak nawet takie coś jak "" dodawało...
                    vars.Add(varsParts[i], typeEnum);
            }
        }

        private static void processSelectPart(string selectPart)
        {
            ParsedSelect parsed = null;
            foreach (ParserPattern pattern in SelectPatterns.getSelectPatterns())
            {
                if(pattern.isMatched(selectPart))
                {
                    parsed = pattern.parse(selectPart);
                }
            }

            if(parsed != null)
            {
                //TODO: wykonujemy operacje (pobieramy informacje z tabel i drzewa) i sprawdzamy czy podane dane (parametry) są tymi z deklaracji
            } else
            {
                Console.WriteLine("Brak instrukcji");
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
