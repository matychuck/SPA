using SPA.AST;
using SPA.VarTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.Interfaces
{
    /// <summary>
    /// Interfejs VarTable
    /// </summary>
    interface IVarTable
    {
        /// <summary>
        /// Dodaj zmienną
        /// </summary>
        /// <param name="procName">Nazwa zmiennej</param>
        /// <returns>Jeżeli zmienna już istnieje: -1; jeżeli nie istnieje: indeks nowo dodanej zmiennej</returns>
        int InsertVar(string procName);

        /// <summary>
        /// Pobierz zmienną
        /// </summary>
        /// <param name="index">Indeks na liście</param>
        /// <returns>Zmienna</returns>
        Variable GetVar(int index);

        /// <summary>
        /// Pobierz zmienną
        /// </summary>
        /// <param name="varName">Nazwa zmiennej</param>
        /// <returns>Zmienna</returns>
        Variable GetVar(string varName);

        /// <summary>
        /// Pobierz indeks dla zmiennej z listy
        /// </summary>
        /// <param name="varName">Nazwa zmiennej</param>
        /// <returns>Jeżeli zmienna nie istnieje: -1; jeżeli istnieje: indeks na liście</returns>
        int GetVarIndex(string varName);

        /// <summary>
        /// Pobierz rozmiar listy 
        /// </summary>
        /// <returns>Rozmiar listy</returns>
        int GetSize();
        
    }
}
