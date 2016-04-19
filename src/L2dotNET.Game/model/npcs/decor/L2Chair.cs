using L2dotNET.Game.tables;

namespace L2dotNET.Game.model.npcs.decor
{
    public class L2Chair : L2StaticObject
    {
        public L2Chair()
        {
            ObjID = IdFactory.getInstance().nextId();
            Closed = 0;
            MaxHP = 0;
            CurHP = 0;
        }

        public bool IsUsedAlready = false;

        public override string asString()
        {
            return "L2Chair:" + ObjID + " " + StaticID + " " + ClanID;
        }
    }
}
