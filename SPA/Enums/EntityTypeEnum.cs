using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.Enums
{
    public enum EntityTypeEnum
    {
        /// <summary>
        /// Program
        /// </summary>
        Program = 0,

        /// <summary>
        /// Procedura
        /// </summary>
        Procedure = 1,

        /// <summary>
        /// Lista poleceń
        /// </summary>
        Stmtlist = 2,

        /// <summary>
        /// Przypisanie
        /// </summary>
        Assign = 3,

        /// <summary>
        /// Wywołanie
        /// </summary>
        Call = 4,

        /// <summary>
        /// Pętla
        /// </summary>
        While = 5,

        /// <summary>
        /// Warunek
        /// </summary>
        If = 6,

        /// <summary>
        /// Dodawanie
        /// </summary>
        Plus = 7,

        /// <summary>
        /// Zmienna
        /// </summary>
        Variable = 8,

        /// <summary>
        /// Stała
        /// </summary>
        Constant = 9,

        /// <summary>
        /// Polecenie
        /// </summary>
        Statement = 10,

        /// <summary>
        /// Odejmowanie
        /// </summary>
        Minus = 11,

        /// <summary>
        /// Mnozenie
        /// </summary>
        Multiply = 12,

        /// <summary>
        /// Dzielenie
        /// </summary>
        Divide = 13,
    }
}
