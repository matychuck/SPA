using SPA2.AST;
using SPA2.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA2.Interfaces
{
    public interface IAST
    {
        TNODE CreateTNode(EntityTypeEnum et);

        void SetRoot(TNODE node);

        void SetAttr(TNODE node, ATTR attr);

        void SetFirstChild(TNODE parent, TNODE child);

        void SetRightSibling(TNODE nodeL, TNODE nodeR);

        void SetChildOfLink(TNODE child, TNODE parent);

        void SetLeftSibling(TNODE nodeL, TNODE nodeR);

        void SetLink(LinkTypeEnum linkTypeEnum, TNODE node1, TNODE node2);

        void SetNthChild(int nth, TNODE parent, TNODE child);

        TNODE GetNthChild(int nth, TNODE parent);

        TNODE GetRoot();

        EntityTypeEnum GetType(TNODE node);

        ATTR GetAttr(TNODE node);

        TNODE GetFirstChild(TNODE parent);

        TNODE_SET GetLinkedNodes(TNODE node, LinkTypeEnum linkTypeEnum);

        bool IsLinked(LinkTypeEnum linkTypeEnum, TNODE node1, TNODE node2);

        void SetParent(TNODE parent, TNODE child);

        TNODE GetParent(TNODE node);

        TNODE_SET GetParentedBy(TNODE node);

        TNODE GetParentStar(TNODE node);

        TNODE_SET GetParentedStarBy(TNODE node);

        void SetFollows(TNODE node1, TNODE node2);

        TNODE GetFollows(TNODE node);

        TNODE GetFollowsStar(TNODE node);

        TNODE_SET GetFollowedBy(TNODE node);

        TNODE_SET GetFollowedStarBy(TNODE node);

        bool IsFollowed(TNODE node1, TNODE node2);

        bool IsFollowedStar(TNODE node1, TNODE node2);

        bool IsParent(TNODE parent, TNODE child);

        bool IsParentStar(TNODE parent, TNODE child);
    }
}
