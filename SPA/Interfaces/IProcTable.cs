using SPA.AST;
using SPA.ProcTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.Interfaces
{
    /// <summary>
    /// Interfejs ProcTable
    /// </summary>
    interface IProcTable
    {
        /// <summary>
        /// Dodaj procedurę
        /// </summary>
        /// <param name="procName">Nazwa procedury</param>
        /// <returns>Jeżeli procedura już istnieje: -1; jeżeli nie istnieje: indeks nowo dodanej procedury</returns>
        int InsertProc(string procName);

        /// <summary>
        /// Pobierz procedurę
        /// </summary>
        /// <param name="index">Indeks na liście</param>
        /// <returns>Procedura</returns>
        Procedure GetProc(int index);

        /// <summary>
        /// Pobierz procedurę
        /// </summary>
        /// <param name="procName">Nazwa procedury</param>
        /// <returns>Procedura</returns>
        Procedure GetProc(string procName);

        /// <summary>
        /// Pobierz indeks procedury na liście
        /// </summary>
        /// <param name="procName">Nazwa procedury</param>
        /// <returns>Jeżeli procedura nie istnieje: -1; jeżeli istnieje: indeks na liście</returns>
        int GetProcIndex(string procName);

        /// <summary>
        /// Pobierz rozmiar listy procedur
        /// </summary>
        /// <returns>Rozmiar listy</returns>
        int GetSize();

        /// <summary>
        /// Ustaw węzeł reprezentujący procedurę w AST
        /// </summary>
        /// <param name="procName">Nazwa procedury</param>
        /// <param name="node">Węzeł</param>
        /// <returns>Jeżeli procedura nie istnieje: -1; jeżeli istnieje: indeks na liście</returns>
        int SetAstRoot(string procName, TNODE node);

        /// <summary>
        /// Pobierz węzeł reprezentujący procedurę w AST
        /// </summary>
        /// <param name="procName">Nazwa procedury</param>
        /// <returns>Węzeł</returns>
        TNODE GetAstRoot(string procName);

        /// <summary>
        /// Pobierz węzeł reprezentujący procedurę w AST
        /// </summary>
        /// <param name="index">Indeks procedury na liście</param>
        /// <returns>Węzeł</returns>
        TNODE GetAstRoot(int index);
    }
}
