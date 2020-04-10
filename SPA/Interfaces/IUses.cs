using SPA.ProcTable;
using SPA.StmtTable;
using SPA.VarTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.Interfaces
{
    /// <summary>
    /// Interfejs Uses
    /// </summary>
    public interface IUses
    {
        /// <summary>
        /// Ustaw relację Uses między zmienną a instrukcją
        /// </summary>
        /// <param name="stmt">Instrukcja</param>
        /// <param name="var">Zmienna</param>
        void SetUses(Statement stmt, Variable var);

        /// <summary>
        /// Ustaw relację Uses między zmienną a procedurą
        /// </summary>
        /// <param name="proc">Procedura</param>
        /// <param name="var">Zmienna</param>
        void SetUses(Procedure proc, Variable var);

        /// <summary>
        /// Pobierz zmienne będące w relacji Uses z instrukcją
        /// </summary>
        /// <param name="stmt">Instrukcja</param>
        /// <returns>Zmienne</returns>
        List<Variable> GetUsed(Statement stmt);

        /// <summary>
        /// Pobierz zmienne będące w relacji Uses z procedurą
        /// </summary>
        /// <param name="proc">Procedura</param>
        /// <returns>Zmienne</returns>
        List<Variable> GetUsed(Procedure proc);

        /// <summary>
        /// Pobierz instrukcje które są w relacji Uses ze zmienną
        /// </summary>
        /// <param name="var">Zmienna</param>
        /// <returns>Instrukcje</returns>
        List<Statement> GetUsesForStmts(Variable var);

        /// <summary>
        /// Pobierz procedury które są w relacji Uses ze zmienną
        /// </summary>
        /// <param name="var">Zmienna</param>
        /// <returns>Procedury</returns>
        List<Procedure> GetUsesForProcs(Variable var);

        /// <summary>
        /// Czy zmienna jest w relacji Uses z instrukcją?
        /// </summary>
        /// <param name="var">Zmienna</param>
        /// <param name="stat">Instrukcja</param>
        /// <returns>Czy są w relacji?</returns>
        bool IsUsed(Variable var, Statement stat);

        /// <summary>
        /// Czy zmienna jest w relacji Uses z procedurą?
        /// </summary>
        /// <param name="var">Zmienna</param>
        /// <param name="proc">Procedura</param>
        /// <returns>Czy są w relacji?</returns>
        bool IsUsed(Variable var, Procedure proc);
    }
}
