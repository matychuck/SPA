using SPA2.AST;
using SPA2.Enums;
using SPA2.StmtTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA2.Interfaces
{
    /// <summary>
    /// Interfejs StmtTable
    /// </summary>
    public interface IStmtTable
    {
        /// <summary>
        /// Dodaj instrukcję
        /// </summary>
        /// <param name="entityTypeEnum">Typ instrukcji</param>
        /// <param name="codeLine">Numer lini w kodzie</param>
        /// <returns>Jeżeli instrukcja już istnieje: -1; jeżeli nie istnieje: indeks nowo dodanej instrukcji</returns>
        int InsertStmt(EntityTypeEnum entityTypeEnum, int codeLine);

        /// <summary>
        /// Pobierz instrukcję
        /// </summary>
        /// <param name="codeLine">Linia w kodzie</param>
        /// <returns>Instrukcja</returns>
        Statement GetStmt(int codeLine);

        /// <summary>
        /// Pobierz rozmiar listy instrukcji
        /// </summary>
        /// <returns>Rozmiar listy</returns>
        int GetSize();

        /// <summary>
        /// Ustaw węzeł reprezentujący instrukcję w AST
        /// </summary>
        /// <param name="codeLine">Linia w kodzie</param>
        /// <param name="node">Węzeł</param>
        /// <returns>Jeżeli instrukcja nie istnieje: -1; jeżeli istnieje: indeks na liście</returns>
        int SetAstRoot(int codeLine, TNODE node);

        /// <summary>
        /// Pobierz węzeł reprezentujący instrukcję w AST
        /// </summary>
        /// <param name="codeLine">Linia w kodzie</param>
        /// <returns>Węzeł</returns>
        TNODE GetAstRoot(int codeLine);
    }
}
