using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;
using L2dotNET.Tables;

namespace L2dotNET.Models.Zones.Classes
{
    class SsqZone : L2Zone
    {
        public SsqZone(IdFactory idFactory) : base(idFactory)
        {
            ZoneId = idFactory.NextId();
            Enabled = true;
        }

        public override void OnEnter(L2Object obj)
        {
            if (!Enabled)
                return;

            base.OnEnter(obj);

            obj.OnEnterZoneAsync(this);

            if (!(obj is L2Player))
                return;

            L2Player p = (L2Player)obj;
            p.SendSystemMessage((SystemMessageId)Template.EnteringMessageNo);
        }

        public override void OnExit(L2Object obj, bool cls)
        {
            if (!Enabled)
                return;

            base.OnExit(obj, cls);

            obj.OnExitZoneAsync(this, cls);

            if (!(obj is L2Player))
                return;

            L2Player p = (L2Player)obj;
            p.SendSystemMessage((SystemMessageId)Template.LeavingMessageNo);
        }
    }
}