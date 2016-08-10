using L2dotNET.model.player;
using L2dotNET.Network.serverpackets;
using L2dotNET.tables;
using L2dotNET.world;

namespace L2dotNET.model.zones.classes
{
    class ssq_zone : L2Zone
    {
        public ssq_zone()
        {
            ZoneId = IdFactory.Instance.NextId();
            Enabled = true;
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
            p.SendSystemMessage((SystemMessage.SystemMessageId)Template.EnteringMessageNo);
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
            p.SendSystemMessage((SystemMessage.SystemMessageId)Template.LeavingMessageNo);
        }
    }
}