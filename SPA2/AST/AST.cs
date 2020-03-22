using SPA2.Enums;
using SPA2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA2.AST
{
    public class AST : IAST
    {

        public TNODE Root { get; set; }

        public TNODE CreateTNode(EntityTypeEnum et)
        {
            return new TNODE(et);
        }

        public ATTR GetAttr(TNODE node)
        {
            return node.Attr;
        }

        public TNODE GetFirstChild(TNODE parent)
        {
            LINK childLink = parent.Links.Where(i => i.LinkTypeEnum == LinkTypeEnum.Child).FirstOrDefault();
            return childLink == null ? null : childLink.LinkNode;
        }

        public TNODE_SET GetFollowedBy(TNODE node)
        {
            throw new NotImplementedException();
        }

        public TNODE_SET GetFollowedStarBy(TNODE node)
        {
            throw new NotImplementedException();
        }

        public TNODE GetFollows(TNODE node)
        {
            throw new NotImplementedException();
        }

        public TNODE GetFollowsStar(TNODE node)
        {
            throw new NotImplementedException();
        }

        public TNODE_SET GetLinkedNodes(TNODE node, LinkTypeEnum linkTypeEnum)
        {
            TNODE_SET node_Set = new TNODE_SET();
            node_Set.nodes = node.Links.Where(i => i.LinkTypeEnum == linkTypeEnum).Select(i => i.LinkNode).ToList();
            return node_Set;

        }

        public TNODE GetNthChild(int nth, TNODE parent)
        {
            LINK link = parent.Links.Where(i => i.LinkTypeEnum == LinkTypeEnum.Child).ElementAtOrDefault(nth);
            return link == null ? null : link.LinkNode;
        }

        public TNODE GetParent(TNODE node)
        {
            return node.Links.Where(x => x.LinkTypeEnum == LinkTypeEnum.Parent).Select(y => y.LinkNode).FirstOrDefault();
        }

        public TNODE_SET GetParentedBy(TNODE node)
        {
            throw new NotImplementedException();
        }

        public TNODE_SET GetParentedStarBy(TNODE node)
        {
            throw new NotImplementedException();
        }

        public TNODE GetParentStar(TNODE node)
        {
            throw new NotImplementedException();
        }

        public TNODE GetRoot()
        {
            return Root;
        }

        public EntityTypeEnum GetType(TNODE node)
        {
            return node.EntityTypeEnum;
        }

        //Does node2 follow node1
        public bool IsFollowed(TNODE node1, TNODE node2)
        {
            return node2.Links.Any(x => x.LinkTypeEnum == LinkTypeEnum.Follows && x.LinkNode == node1);
        }

        public bool IsFollowedStar(TNODE node1, TNODE node2)
        {
            if (node2.Links.Any(x => x.LinkTypeEnum == LinkTypeEnum.Follows && x.LinkNode == node1))
            {
                return true;
            }
            else
            {
                if (node2.Links.Where(x => x.LinkTypeEnum == LinkTypeEnum.Follows).Count() != 0)
                {
                    foreach (TNODE onRight in node2.Links.Where(x => x.LinkTypeEnum == LinkTypeEnum.Follows).Select(y => y.LinkNode))
                    {
                        IsFollowedStar(onRight, node1);
                    }
                }
            }
            return false;
        }

        public bool IsLinked(LinkTypeEnum linkTypeEnum, TNODE node1, TNODE node2)
        {
            return node1.Links.Where(i => i.LinkTypeEnum == linkTypeEnum && i.LinkNode == node2).Any();
        }

        public bool IsParent(TNODE parent, TNODE child)
        {
            return parent.Links.Any(x => x.LinkTypeEnum == LinkTypeEnum.Child && x.LinkNode == child);
        }

        public bool IsParentStar(TNODE parent, TNODE child)
        {
            if (parent.Links.Any(x => x.LinkTypeEnum == LinkTypeEnum.Child && x.LinkNode == child))
            {
                return true;
            }
            else
            {
                if (parent.Links.Where(x => x.LinkTypeEnum == LinkTypeEnum.Child).Count() != 0)
                {
                    foreach (TNODE relative in parent.Links.Where(x => x.LinkTypeEnum == LinkTypeEnum.Child).Select(y => y.LinkNode))
                    {
                        IsParentStar(relative, child);
                    }
                }
            }
            return false;
        }

        public void SetAttr(TNODE node, ATTR attr)
        {
            node.Attr = attr;
        }

        public void SetChildOfLink(TNODE child, TNODE parent)
        {
            if (!parent.Links.Any(x => x.LinkTypeEnum == LinkTypeEnum.Child && x.LinkNode == child))
            {
                parent.Links.Add(new LINK(LinkTypeEnum.Child, child));
            }

        }

        public void SetFirstChild(TNODE parent, TNODE child)
        {
            if (parent.Links.Any(x => x.LinkTypeEnum == LinkTypeEnum.Child))
            {
                var link = parent.Links.Where(x => x.LinkTypeEnum == LinkTypeEnum.Child && x.LinkNode == child).FirstOrDefault();
                parent.Links.Remove(link);
                parent.Links.Insert(0, new LINK(LinkTypeEnum.Child, child));
            }
            else
            {
                parent.Links.Add(new LINK(LinkTypeEnum.Child, child));
            }

        }

        public void SetFollows(TNODE node1, TNODE node2)
        {
            node1.Links.Add(new LINK(LinkTypeEnum.Follows, node2));
        }

        public void SetLeftSibling(TNODE nodeL, TNODE nodeR)
        {
            nodeR.Links.Add(new LINK(LinkTypeEnum.LeftSibling, nodeL));
        }

        public void SetLink(LinkTypeEnum linkTypeEnum, TNODE node1, TNODE node2)
        {
            node1.Links.Add(new LINK(linkTypeEnum, node2));
        }

        public void SetNthChild(int nth, TNODE parent, TNODE child)
        {
            if (parent.Links.Count() > nth)
            {
                if (parent.Links[nth - 1].LinkTypeEnum == LinkTypeEnum.Child)
                {
                    parent.Links[nth - 1].LinkNode = child;
                }
            }
            else if (parent.Links.Count() - 1 == nth)
            {
                parent.Links.Add(new LINK(LinkTypeEnum.Child, child));
            }
        }

        public void SetParent(TNODE parent, TNODE child)
        {
            child.Links.Add(new LINK(LinkTypeEnum.Parent, parent));
        }

        public void SetRightSibling(TNODE nodeL, TNODE nodeR)
        {
            nodeL.Links.Add(new LINK(LinkTypeEnum.RightSibling, nodeR));
        }

        public void SetRoot(TNODE node)
        {
            Root = node;
        }

        //metody pomocnicze
        private TNODE ReturnParent(List<TNODE> parents, TNODE child)
        {
            foreach (TNODE parent in parents)
            {
                if (parent.Links.Any(x => x.LinkTypeEnum == LinkTypeEnum.Child && x.LinkNode == child)) return parent;
                else ReturnParent(parent.Links.Where(x => x.LinkTypeEnum == LinkTypeEnum.Child).Select(y => y.LinkNode).ToList(), child);
            }
            return null;
        }
    }
}
