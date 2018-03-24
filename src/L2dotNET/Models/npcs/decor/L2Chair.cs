using L2dotNET.templates;

namespace L2dotNET.Models.npcs.decor
{
    public sealed class L2Chair : L2StaticObject
    {
        public bool IsUsedAlready = false;

        public L2Chair(int objectId, CharTemplate template) : base(objectId, template)
        {
            Closed = 0;
            //MaxHp = 0;
            CurHp = 0;
        }

        public override string AsString()
        {
            return $"L2Chair:{ObjId} {StaticId}";
        }
    }
}