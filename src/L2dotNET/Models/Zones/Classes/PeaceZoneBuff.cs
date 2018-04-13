using L2dotNET.Tables;

namespace L2dotNET.Models.Zones.Classes
{
    class PeaceZoneBuff : L2Zone
    {
        public PeaceZoneBuff()
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
        }

        public override void OnExit(L2Object obj, bool cls)
        {
            if (!Enabled)
                return;

            base.OnExit(obj, cls);

            obj.OnExitZone(this, cls);
        }
    }
}