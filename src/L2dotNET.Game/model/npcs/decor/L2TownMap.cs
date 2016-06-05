using L2dotNET.GameService.tables;

namespace L2dotNET.GameService.model.npcs.decor
{
    public class L2TownMap : L2StaticObject
    {
        public L2TownMap()
        {
            ObjID = IdFactory.Instance.nextId();
        }

        public override void NotifyAction(L2Player player)
        {
            player.sendPacket(townMap);
        }

        public override string asString()
        {
            return "L2TownMap:" + ObjID + " " + StaticID;
        }
    }
}