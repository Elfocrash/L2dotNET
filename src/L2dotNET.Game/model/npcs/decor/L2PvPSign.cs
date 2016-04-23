using L2dotNET.Game.network.l2send;
using L2dotNET.Game.tables;

namespace L2dotNET.Game.model.npcs.decor
{
    public class L2PvPSign : L2StaticObject
    {
        public L2PvPSign()
        {
            ObjID = IdFactory.Instance.nextId();
        }

        public override void NotifyAction(L2Player player)
        {
            player.sendPacket(new NpcHtmlMessage(player, htm, ObjID, 0));
        }

        public override string asString()
        {
            return "L2PvP Sign:" + ObjID + " " + StaticID + " " + ClanID;
        }
    }
}
