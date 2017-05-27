using L2dotNET.model.player;
using L2dotNET.model.structures;
using L2dotNET.tables;
using L2dotNET.world;

namespace L2dotNET.model.zones.classes
{
    class hideout_zone : L2Zone
    {
        public Hideout hideout;

        public hideout_zone()
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

            if (obj is L2Player)
            {
                //    p._stats.p_regen_hp += Template._hp_regen_bonus;
                //   p._stats.p_regen_mp += Template._mp_regen_bonus;
            }
        }

        public override void OnExit(L2Object obj, bool cls)
        {
            if (!Enabled)
                return;

            base.OnExit(obj, cls);

            obj.OnExitZone(this, cls);

            if (obj is L2Player)
            {
                //    p._stats.p_regen_hp += Template._hp_regen_bonus;
                //   p._stats.p_regen_mp += Template._mp_regen_bonus;
            }
        }
    }
}