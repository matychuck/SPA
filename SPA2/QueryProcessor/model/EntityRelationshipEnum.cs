using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA2.QueryProcessor.model
{
    public enum EntityRelationshipEnum
    {
        Modifies,
        Uses,
        Parent,
        ParentS,
        Follows,
        FollowsS,
        Next,
        NextS
    }
}
