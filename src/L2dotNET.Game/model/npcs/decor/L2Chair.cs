using L2dotNET.GameService.Tables;

namespace L2dotNET.GameService.Model.Npcs.Decor
{
    public class L2Chair : L2StaticObject
    {
        public L2Chair()
        {
            ObjID = IdFactory.Instance.nextId();
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