using System.Threading.Tasks;
using L2dotNET.Models.Player;
using L2dotNET.Templates;

namespace L2dotNET.Models.Npcs.Decor
{
    public class L2TownMap : L2StaticObject
    {
        public L2TownMap(int objectId, CharTemplate template) : base(objectId, template)
        {
        }

        public override async Task NotifyActionAsync(L2Player player)
        {
            await player.SendPacketAsync(TownMap);
        }

        public override string AsString()
        {
            return $"L2TownMap:{ObjId} {StaticId}";
        }
    }
}