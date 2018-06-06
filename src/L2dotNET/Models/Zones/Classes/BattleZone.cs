using L2dotNET.Tables;

namespace L2dotNET.Models.Zones.Classes
{
    class BattleZone : L2Zone
    {
        public BattleZone(IdFactory idFactory) : base(idFactory)
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
        }

        public override void OnExit(L2Object obj, bool cls)
        {
            if (!Enabled)
                return;

            base.OnExit(obj, cls);

            obj.OnExitZoneAsync(this, cls);
        }
    }
}