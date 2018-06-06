using L2dotNET.Models.Player;
using L2dotNET.Templates;

namespace L2dotNET.Models.Npcs.Decor
{
    public class L2TownMap : L2StaticObject
    {
        public L2TownMap(int objectId, CharTemplate template) : base(objectId, template)
        {
        }

        public override void NotifyAction(L2Player player)
        {
            player.SendPacketAsync(TownMap);
        }

        public override string AsString()
        {
            return $"L2TownMap:{ObjId} {StaticId}";
        }
    }
}