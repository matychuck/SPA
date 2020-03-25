using SPA2.AST;
using SPA2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA2.VarTable
{
    public class VarTable : IVarTable
    {
        public List<Variable> Variables { get; set; }

        public VarTable()
        {
            Variables = new List<Variable>();
        }

        public int GetSize()
        {
            return Variables.Count();
        }

        public Variable GetVar(int index)
        {
            return Variables.Where(i => i.Index == index).FirstOrDefault();
        }

        public Variable GetVar(string varName)
        {
            return Variables.Where(i => i.Name == varName).FirstOrDefault();
        }

        public int GetVarIndex(string varName)
        {
            var variable = GetVar(varName);
            return variable == null ? -1 : variable.Index;
        }

        public int InsertVar(string varName)
        {
            if(Variables.Where(i=>i.Name == varName).Any())
            {
                return -1;
            }
            else
            {
                Variable variable = new Variable(varName);
                variable.Index = GetSize();
                Variables.Add(variable);
                return GetVarIndex(varName);
            }
        }
    }
}
