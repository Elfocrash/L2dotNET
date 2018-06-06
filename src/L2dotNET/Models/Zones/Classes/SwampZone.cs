using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;
using L2dotNET.Tables;

namespace L2dotNET.Models.Zones.Classes
{
    class SwampZone : L2Zone
    {
        public SwampZone(IdFactory idFactory) : base(idFactory)
        {
            ZoneId = idFactory.NextId();
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
            p.SendPacketAsync(new EtcStatusUpdate(p));
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
            p.SendPacketAsync(new EtcStatusUpdate(p));
            //  p._stats.base_p_speed -= Template._move_bonus;
            p.BroadcastUserInfo();
        }
    }
}