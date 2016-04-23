using L2dotNET.Game.network.l2send;
using L2dotNET.Game.tables;

namespace L2dotNET.Game.model.npcs.decor
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
