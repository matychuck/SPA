using System;

namespace SPA2.QueryProcessor.model
{
    public class SuchThatPart
    {
        private EntityRelationshipEnum entityRelationshipEnum { get; set; }
        private String arg1 { get; set; }
        private String arg2 { get; set; }

        public SuchThatPart(EntityRelationshipEnum entityRelationshipEnum, String arg1, String arg2)
        {
            this.entityRelationshipEnum = entityRelationshipEnum;
            this.arg1 = arg1;
            this.arg2 = arg2;
        }

    }
}
