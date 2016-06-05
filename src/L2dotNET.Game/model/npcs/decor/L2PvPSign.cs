using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tables;

namespace L2dotNET.GameService.Model.Npcs.Decor
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