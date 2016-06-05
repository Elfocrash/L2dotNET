using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Tables;

namespace L2dotNET.GameService.Model.Npcs.Decor
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