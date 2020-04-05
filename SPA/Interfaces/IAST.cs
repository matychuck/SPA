using SPA.AST;
using SPA.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.Interfaces
{
    /// <summary>
    /// Interfejs AST
    /// </summary>
    public interface IAST
    {
        /// <summary>
        /// Tworzenie węzła
        /// </summary>
        /// <param name="et">Typ węzła</param>
        /// <returns>Węzeł</returns>
        TNODE CreateTNode(EntityTypeEnum et);

        /// <summary>
        /// Tworzenie kopii węzła
        /// </summary>
        /// <param name="node">Węzeł</param>
        /// <returns>Węzeł</returns>
        TNODE GetTNodeDeepCopy(TNODE node);

        /// <summary>
        /// Ustaw główny węzeł
        /// </summary>
        /// <param name="node">Węzeł</param>
        void SetRoot(TNODE node);

        /// <summary>
        /// Ustaw atrybut dla węzła
        /// </summary>
        /// <param name="node">Węzeł</param>
        /// <param name="attr">Atrybut</param>
        void SetAttr(TNODE node, ATTR attr);

        /// <summary>
        /// Ustaw pierwsze dziecko dla węzła
        /// </summary>
        /// <param name="parent">Węzeł rodzic</param>
        /// <param name="child">Węzeł dziecko</param>
        void SetFirstChild(TNODE parent, TNODE child);

        /// <summary>
        /// Ustaw prawe rodzeństwo dla węzła
        /// </summary>
        /// <param name="nodeL">Lewy węzeł</param>
        /// <param name="nodeR">Prawy węzeł</param>
        void SetRightSibling(TNODE nodeL, TNODE nodeR);

        /// <summary>
        /// Ustaw dziecko dla węzła
        /// </summary>
        /// <param name="child">Węzeł dziecko</param>
        /// <param name="parent">Węzeł rodzic</param>
        void SetChildOfLink(TNODE child, TNODE parent);

        /// <summary>
        /// Ustaw lewe rodzeństwo dla węzła
        /// </summary>
        /// <param name="nodeL">Lewy węzeł</param>
        /// <param name="nodeR">Prawy węzeł</param>
        void SetLeftSibling(TNODE nodeL, TNODE nodeR);

        /// <summary>
        /// Ustaw relację dla węzłów
        /// </summary>
        /// <param name="linkTypeEnum">Typ relacji</param>
        /// <param name="node1">Rodzic relacji</param>
        /// <param name="node2">Dziecko relacji</param>
        void SetLink(LinkTypeEnum linkTypeEnum, TNODE node1, TNODE node2);

        /// <summary>
        /// Ustaw relacji odwrotne dla węzłów
        /// </summary>
        /// <param name="linkTypeEnum">Typ relacji</param>
        /// <param name="node1">Rodzic relacji</param>
        /// <param name="node2">Dziecko relacji</param>
        void SetPrevLink(LinkTypeEnum linkTypeEnum, TNODE node1, TNODE node2);

        /// <summary>
        /// Ustaw n-te dziecko
        /// </summary>
        /// <param name="nth">N</param>
        /// <param name="parent">Węzeł rodzic</param>
        /// <param name="child">Węzeł dziecko</param>
        void SetNthChild(int nth, TNODE parent, TNODE child);

        /// <summary>
        /// Pobierz n-te dziecko
        /// </summary>
        /// <param name="nth">N</param>
        /// <param name="parent">Węzeł rodzic</param>
        /// <returns>Węzeł</returns>
        TNODE GetNthChild(int nth, TNODE parent);

        /// <summary>
        /// Pobierz głowny węzeł
        /// </summary>
        /// <returns>Węzeł</returns>
        TNODE GetRoot();

        /// <summary>
        /// Pobierz typ węzła
        /// </summary>
        /// <param name="node">Węzeł</param>
        /// <returns>Typ węzła</returns>
        EntityTypeEnum GetType(TNODE node);

        /// <summary>
        /// Pobiera atrybut węzła
        /// </summary>
        /// <param name="node">Węzeł</param>
        /// <returns>Atrybut węzła</returns>
        ATTR GetAttr(TNODE node);

        /// <summary>
        /// Pobierz pierwsze dziecko dla węzła
        /// </summary>
        /// <param name="parent">Węzeł rodzic</param>
        /// <returns>Węzeł dziecko</returns>
        TNODE GetFirstChild(TNODE parent);

        /// <summary>
        /// Pobierz wszystkie węzły będące w relacji
        /// </summary>
        /// <param name="node">Węzeł</param>
        /// <param name="linkTypeEnum">Typ relacji</param>
        /// <returns>Węzły będące w relacji</returns>
        List<TNODE> GetLinkedNodes(TNODE node, LinkTypeEnum linkTypeEnum);

        /// <summary>
        /// Pobierz wszystkie węzły będące w odwrotnej relacji
        /// </summary>
        /// <param name="node">Węzeł</param>
        /// <param name="linkTypeEnum">Typ relacji</param>
        /// <returns>Węzły będące w relacji odwrotnej</returns>
        List<TNODE> GetPrevLinkedNodes(TNODE node, LinkTypeEnum linkTypeEnum);

        /// <summary>
        /// Czy węzły są w relacji?
        /// </summary>
        /// <param name="linkTypeEnum">Typ relacji</param>
        /// <param name="node1">Rodzic relacji</param>
        /// <param name="node2">Dziecko relacjiparam>
        /// <returns>Czy są w relacji?</returns>
        bool IsLinked(LinkTypeEnum linkTypeEnum, TNODE node1, TNODE node2);

        /// <summary>
        /// Ustaw relację Parent
        /// </summary>
        /// <param name="parent">Węzeł rodzic</param>
        /// <param name="child">Węzeł dziecko</param>
        void SetParent(TNODE parent, TNODE child);

        /// <summary>
        /// Pobierz Parent
        /// </summary>
        /// <param name="node">Węzeł</param>
        /// <returns>Parent</returns>
        TNODE GetParent(TNODE node);

        /// <summary>
        /// Pobierz węzły będące w relacji Parent z node
        /// </summary>
        /// <param name="node">Węzeł</param>
        /// <returns>Węzły będące w relacji Parent</returns>
        List<TNODE> GetParentedBy(TNODE node);

        /// <summary>
        /// Pobierz Parent*
        /// </summary>
        /// <param name="node">Węzeł</param>
        /// <returns>Węzły Parent*</returns>
        List<TNODE> GetParentStar(TNODE node);

        /// <summary>
        /// Pobierz węzły będące w relacji Parent* z node
        /// </summary>
        /// <param name="node">Węzeł</param>
        /// <returns>Węzły będące w relacji Parent*</returns>
        List<TNODE> GetParentedStarBy(TNODE node);

        /// <summary>
        /// Ustaw relację Follows
        /// </summary>
        /// <param name="node1">Węzeł rodzic</param>
        /// <param name="node2">Węzeł dziecko</param>
        void SetFollows(TNODE node1, TNODE node2);

        /// <summary>
        /// Pobierz węzły Follows 
        /// </summary>
        /// <param name="node">Węzeł</param>
        /// <returns>Węzły Follows</returns>
        List<TNODE> GetFollows(TNODE node);

        /// <summary>
        /// Pobierz węzły relacji Follows*
        /// </summary>
        /// <param name="node">Węzeł</param>
        /// <returns>Węzły relacji Follows*</returns>
        List<TNODE> GetFollowsStar(TNODE node);

        /// <summary>
        /// Pobierz węzły będące w relacji Follows z node
        /// </summary>
        /// <param name="node">Węzeł</param>
        /// <returns>Węzły będące w relacji Follows</returns>
        List<TNODE> GetFollowedBy(TNODE node);

        /// <summary>
        /// Pobierz węzły będące w relacji Follows* z node
        /// </summary>
        /// <param name="node">Węzeł</param>
        /// <returns>Węzły będące w relacji Follows*</returns>
        List<TNODE> GetFollowedStarBy(TNODE node);

        /// <summary>
        /// Czy węzły są w relacji Follows?
        /// </summary>
        /// <param name="node1">Węzeł rodzic</param>
        /// <param name="node2">Węzeł dziecko</param>
        /// <returns>Czy węzły są w relacji?</returns>
        bool IsFollowed(TNODE node1, TNODE node2);

        /// <summary>
        /// Czy węzły są w relacji Follows*?
        /// </summary>
        /// <param name="node1">Węzeł rodzic</param>
        /// <param name="node2">Węzeł dziecko</param>
        /// <returns>Czy węzły są w relacji?</returns>
        bool IsFollowedStar(TNODE node1, TNODE node2);

        /// <summary>
        /// Czy węzły są w relacji Parent?
        /// </summary>
        /// <param name="parent">Węzeł rodzic</param>
        /// <param name="child">Węzeł dziecko</param>
        /// <returns>Czy węzły są w relacji?</returns>
        bool IsParent(TNODE parent, TNODE child);

        /// <summary>
        /// Czy węzły są w relacji Parent*?
        /// </summary>
        /// <param name="parent">Węzeł rodzic</param>
        /// <param name="child">Węzeł dziecko</param>
        /// <returns>Czy węzły są w relacji?</returns>
        bool IsParentStar(TNODE parent, TNODE child);
    }
}
