using L2dotNET.Models.player;
using L2dotNET.Network.serverpackets;
using L2dotNET.templates;

namespace L2dotNET.Models.npcs.decor
{
    public class L2PvPSign : L2StaticObject
    {
        public L2PvPSign(int objectId, CharTemplate template) : base(objectId, template)
        {
        }

        public override void NotifyAction(L2Player player)
        {
            player.SendPacket(new NpcHtmlMessage(player, Htm, ObjId, 0));
        }

        public override string AsString()
        {
            return $"L2PvP Sign:{ObjId} {StaticId}";
        }
    }
}