using L2dotNET.model.player;
using L2dotNET.tables;
using L2dotNET.templates;

namespace L2dotNET.model.npcs.decor
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