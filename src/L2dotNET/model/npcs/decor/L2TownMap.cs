using L2dotNET.model.player;
using L2dotNET.tables;

namespace L2dotNET.model.npcs.decor
{
    public class L2TownMap : L2StaticObject
    {
        public L2TownMap()
        {
            ObjId = IdFactory.Instance.NextId();
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