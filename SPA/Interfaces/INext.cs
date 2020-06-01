using SPA.StmtTable;
using System;
using System.Collections.Generic;
using System.Text;

namespace SPA.Interfaces
{
    /// <summary>
    /// Interfejs INext
    /// </summary>
    public interface INext
    {
        /// <summary>
        /// Zwróć następną operację
        /// </summary>
        /// <param name="statement">Operacja</param>
        /// <returns>Operacje</returns>
        List<Statement> GetNext(Statement statement);

        /// <summary>
        /// Zwróć następujące operacje
        /// </summary>
        /// <param name="statement">Operacja</param>
        /// <returns>Operacje</returns>
        List<Statement> GetNextStar(Statement statement);

        /// <summary>
        /// Czy operacja1 następuje po operacji2
        /// </summary>
        /// <param name="line1">Numer linni operacji1</param>
        /// <param name="line2">Numer linni operacji2</param>
        /// <returns>Czy następuje?</returns>
        bool IsNext(int line1, int line2);

        /// <summary>
        /// Czy operacja1 następuje po operacji2 *
        /// </summary>
        /// <param name="line1">Numer linni operacji1</param>
        /// <param name="line2">Numer linni operacji2</param>
        /// <returns>Czy następuje *?</returns>
        bool IsNextStar(int line1, int line2);
    }
}
