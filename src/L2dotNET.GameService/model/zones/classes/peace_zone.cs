using L2dotNET.GameService.Tables;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Zones.Classes
{
    class peace_zone : L2Zone
    {
        public peace_zone()
        {
            ZoneId = IdFactory.Instance.NextId();
            Enabled = true;
        }

        public override void OnEnter(L2Object obj)
        {
            if (!Enabled)
            {
                return;
            }

            base.OnEnter(obj);

            obj.OnEnterZone(this);
        }

        public override void OnExit(L2Object obj, bool cls)
        {
            if (!Enabled)
            {
                return;
            }

            base.OnExit(obj, cls);

            obj.OnExitZone(this, cls);
        }
    }
}