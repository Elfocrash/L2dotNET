using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tables;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Zones.Classes
{
    class swamp : L2Zone
    {
        public swamp()
        {
            ZoneId = IdFactory.Instance.NextId();
        }

        public override void OnInit()
        {
            base.OnInit();
            Enabled = Template.DefaultStatus;
        }

        public override void OnEnter(L2Object obj)
        {
            if (!Enabled)
                return;

            base.OnEnter(obj);

            obj.OnEnterZone(this);

            if (!(obj is L2Player))
                return;

            L2Player p = (L2Player)obj;
            p.IsInDanger = true;
            p.SendPacket(new EtcStatusUpdate(p));
            //  p._stats.base_p_speed += Template._move_bonus;
            p.BroadcastUserInfo();
        }

        public override void OnExit(L2Object obj, bool cls)
        {
            if (!Enabled)
                return;

            base.OnExit(obj, cls);

            obj.OnExitZone(this, cls);

            if (!(obj is L2Player))
                return;

            L2Player p = (L2Player)obj;
            p.IsInDanger = false;
            p.SendPacket(new EtcStatusUpdate(p));
            //  p._stats.base_p_speed -= Template._move_bonus;
            p.BroadcastUserInfo();
        }
    }
}