using L2dotNET.GameService.tables;

namespace L2dotNET.GameService.model.zones.classes
{
    class water : L2Zone
    {
        public water()
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
        }

        public override void onExit(world.L2Object obj, bool cls)
        {
            if (!_enabled)
                return;

            base.onExit(obj, cls);

            obj.onExitZone(this, cls);
        }
    }
}