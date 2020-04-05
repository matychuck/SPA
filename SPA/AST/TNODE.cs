using SPA.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.AST
{
    public class TNODE
    {
        public ATTR Attr { get; set; }
        public List<LINK> Links { get; set; }
        public List<LINK> PrevLinks { get; set; }
        public EntityTypeEnum EntityTypeEnum { get; set; }

        public TNODE(EntityTypeEnum entityTypeEnum)
        {
            PrevLinks = new List<LINK>();
            Links = new List<LINK>();
            EntityTypeEnum = entityTypeEnum;
        }

        public TNODE(TNODE node)
        {
            this.Attr = node.Attr;
            this.Links = node.Links;
            this.PrevLinks = node.PrevLinks;
            this.EntityTypeEnum = node.EntityTypeEnum;
        }
    }
}
