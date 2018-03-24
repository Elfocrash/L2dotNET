using L2dotNET.Models.player;
using L2dotNET.templates;

namespace L2dotNET.Models.npcs.decor
{
    public class L2TownMap : L2StaticObject
    {
        public L2TownMap(int objectId, CharTemplate template) : base(objectId, template)
        {
        }

        public override void NotifyAction(L2Player player)
        {
            player.SendPacket(TownMap);
        }

        public override string AsString()
        {
            return $"L2TownMap:{ObjId} {StaticId}";
        }
    }
}