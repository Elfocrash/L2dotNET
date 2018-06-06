using System.Threading.Tasks;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;
using L2dotNET.Templates;

namespace L2dotNET.Models.Npcs.Decor
{
    public class L2PvPSign : L2StaticObject
    {
        public L2PvPSign(int objectId, CharTemplate template) : base(objectId, template)
        {
        }

        public override async Task NotifyActionAsync(L2Player player)
        {
            await player.SendPacketAsync(new NpcHtmlMessage(player, Htm, ObjId, 0));
        }

        public override string AsString()
        {
            return $"L2PvP Sign:{ObjId} {StaticId}";
        }
    }
}