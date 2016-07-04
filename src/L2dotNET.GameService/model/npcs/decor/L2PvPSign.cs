using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tables;

namespace L2dotNET.GameService.Model.Npcs.Decor
{
    public class L2PvPSign : L2StaticObject
    {
        public L2PvPSign()
        {
            ObjId = IdFactory.Instance.nextId();
        }

        public override void NotifyAction(L2Player player)
        {
            player.SendPacket(new NpcHtmlMessage(player, htm, ObjId, 0));
        }

        public override string AsString()
        {
            return "L2PvP Sign:" + ObjId + " " + StaticID + " " + ClanID;
        }
    }
}