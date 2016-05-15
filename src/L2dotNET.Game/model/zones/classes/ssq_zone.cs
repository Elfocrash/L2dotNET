
using L2dotNET.GameService.network.l2send;
using L2dotNET.GameService.tables;
namespace L2dotNET.GameService.model.zones.classes
{
    class ssq_zone : L2Zone
    {
        public ssq_zone()
        {
            ZoneID = IdFactory.Instance.nextId();
            _enabled = true;
        }

        public override void onEnter(world.L2Object obj)
        {
            if (!_enabled)
                return;

            base.onEnter(obj);

            obj.onEnterZone(this);

            if (obj is L2Player)
            {
                L2Player p = (L2Player)obj;
                p.sendSystemMessage((SystemMessage.SystemMessageId)Template._entering_message_no);
            }
        }

        public override void onExit(world.L2Object obj, bool cls)
        {
            if (!_enabled)
                return;

            base.onExit(obj, cls);

            obj.onExitZone(this, cls);

            if (obj is L2Player)
            {
                L2Player p = (L2Player)obj;
                p.sendSystemMessage((SystemMessage.SystemMessageId)Template._leaving_message_no);
            }
        }
    }
}
