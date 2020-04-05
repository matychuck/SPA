using SPA.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.AST
{
    public class LINK
    {
        public LinkTypeEnum LinkTypeEnum { get; set; }
        public TNODE LinkNode { get; set; }

        public LINK(LinkTypeEnum linkTypeEnum, TNODE nodeToLink)
        {
            LinkNode = nodeToLink;
            LinkTypeEnum = linkTypeEnum;
        }
    }
}
