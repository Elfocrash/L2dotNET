using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Structures;
using L2dotNET.GameService.Tables;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Zones.Classes
{
    class hideout_zone : L2Zone
    {
        public Hideout hideout;

        public hideout_zone()
        {
            ZoneID = IdFactory.Instance.nextId();
            _enabled = true;
        }

        public override void onEnter(L2Object obj)
        {
            if (!_enabled)
                return;

            base.onEnter(obj);

            obj.OnEnterZone(this);

            if (obj is L2Player)
            {
                //    p._stats.p_regen_hp += Template._hp_regen_bonus;
                //   p._stats.p_regen_mp += Template._mp_regen_bonus;
            }
        }

        public override void onExit(L2Object obj, bool cls)
        {
            if (!_enabled)
                return;

            base.onExit(obj, cls);

            obj.OnExitZone(this, cls);

            if (obj is L2Player)
            {
                //    p._stats.p_regen_hp += Template._hp_regen_bonus;
                //   p._stats.p_regen_mp += Template._mp_regen_bonus;
            }
        }
    }
}