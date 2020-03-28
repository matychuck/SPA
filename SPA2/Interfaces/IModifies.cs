using SPA2.ProcTable;
using SPA2.StmtTable;
using SPA2.VarTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA2.Interfaces
{
    /// <summary>
    /// Interfejs Modifies
    /// </summary>
    public interface IModifies
    {
        /// <summary>
        /// Ustaw relację Modifies między zmienną a instrukcją
        /// </summary>
        /// <param name="stmt">Instrukcja</param>
        /// <param name="var">Zmienna</param>
        void SetModifies(Statement stmt, Variable var);

        /// <summary>
        /// Ustaw relację Modifies między zmienną a procedurą
        /// </summary>
        /// <param name="proc">Procedura</param>
        /// <param name="var">Zmienna</param>
        void SetModifies(Procedure proc, Variable var);

        /// <summary>
        /// Pobierz zmienne będące w relacji Modifies z instrukcją
        /// </summary>
        /// <param name="stmt">Instrukcja</param>
        /// <returns>Zmienne</returns>
        List<Variable> GetModified(Statement stmt);

        /// <summary>
        /// Pobierz zmienne będące w relacji Modifies z procedurą
        /// </summary>
        /// <param name="proc">Procedura</param>
        /// <returns>Zmienne</returns>
        List<Variable> GetModified(Procedure proc);

        /// <summary>
        /// Pobierz instrukcje które są w relacji Modifies ze zmienną
        /// </summary>
        /// <param name="var">Zmienna</param>
        /// <returns>Instrukcje</returns>
        List<Statement> GetModifiesForStmts(Variable var);

        /// <summary>
        /// Pobierz procedury które są w relacji Modifies ze zmienną
        /// </summary>
        /// <param name="var">Zmienna</param>
        /// <returns>Procedury</returns>
        List<Procedure> GetModifiesForProcs(Variable var);

        /// <summary>
        /// Czy zmienna jest w relacji Modifies z instrukcją?
        /// </summary>
        /// <param name="var">Zmienna</param>
        /// <param name="stat">Instrukcja</param>
        /// <returns>Czy są w relacji?</returns>
        bool IsModified(Variable var, Statement stat);

        /// <summary>
        /// Czy zmienna jest w relacji Modifies z procedurą?
        /// </summary>
        /// <param name="var">Zmienna</param>
        /// <param name="proc">Procedura</param>
        /// <returns>Czy są w relacji?</returns>
        bool IsModified(Variable var, Procedure proc);


    }
}
