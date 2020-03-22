using SPA2.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA2.AST
{
    public class TNODE
    {
        public ATTR Attr { get; set; }
        public List<LINK> Links { get; set; }
        public EntityTypeEnum EntityTypeEnum { get; set; }

        public TNODE(EntityTypeEnum entityTypeEnum)
        {
            EntityTypeEnum = entityTypeEnum;
        }
    }
}
