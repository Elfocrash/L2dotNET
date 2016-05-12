using L2dotNET.GameService.network.l2send;
using L2dotNET.GameService.tables;

namespace L2dotNET.GameService.model.npcs.decor
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
