using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Tables;

namespace L2dotNET.GameService.Model.Npcs.Decor
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
            return "L2TownMap:" + ObjId + " " + StaticId;
        }
    }
}