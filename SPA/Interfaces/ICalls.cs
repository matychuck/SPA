using SPA.ProcTable;
using System;
using System.Collections.Generic;
using System.Text;

namespace SPA.Interfaces
{
    public interface ICalls
    {
        /// <summary>
        /// Zwróć wywoływane procedury w Procedurze
        /// </summary>
        /// <param name="proc">Procedura</param>
        /// <returns>Procedury</returns>
        List<Procedure> GetCalls(string proc);

        /// <summary>
        /// Zwróć wywoływane procedury* w Procedurze
        /// </summary>
        /// <param name="proc">Procedura</param>
        /// <returns>Procedury</returns>
        List<Procedure> GetCallsStar(string proc);

        /// <summary>
        /// Zwróć procedury, które wywołują Procedurę
        /// </summary>
        /// <param name="proc">Procedura</param>
        /// <returns>Procedury</returns>
        List<Procedure> GetCalledBy(string proc);

        /// <summary>
        /// Zwróć procedury, które wywołują Procedurę*
        /// </summary>
        /// <param name="proc">Procedura*</param>
        /// <returns>Procedury</returns>
        List<Procedure> GetCalledByStar(string proc);

        /// <summary>
        /// Czy Procedura 1 wywołuje Procedurę 2
        /// </summary>
        /// <param name="proc1">Procedura 1</param>
        /// <param name="proc2">Procedura 2</param>
        /// <returns>Czy wywołuje?</returns>
        bool IsCalls(string proc1, string proc2);

        /// <summary>
        /// Czy Procedura 1 wywołuje Proceduę 2 *
        /// </summary>
        /// <param name="proc1">Procedura 1</param>
        /// <param name="proc2">Procedura 2</param>
        /// <returns>Czy wywołuje *?</returns>
        bool IsCallsStar(string proc1, string proc2);

        /// <summary>
        /// Czy Procedura 1 jest wywoływana przez Procedurę 2
        /// </summary>
        /// <param name="proc1">Procedura 1</param>
        /// <param name="proc2">Procedura 2</param>
        /// <returns>Czy jest wywoływana?</returns>
        bool IsCalledBy(string proc1, string proc2);

        /// <summary>
        /// Czy Procedura 1 jest wywoływana przez Procedurę 2 *
        /// </summary>
        /// <param name="proc1">Procedura 1</param>
        /// <param name="proc2">Procedura 2</param>
        /// <returns>Czy jest wywoływana *?</returns>
        bool IsCalledStarBy(string proc1, string proc2);

    }
}
