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

            if (obj is L2Player)
            {
                //    p._stats.p_regen_hp += Template._hp_regen_bonus;
                //   p._stats.p_regen_mp += Template._mp_regen_bonus;
            }
        }

        public override void OnExit(L2Object obj, bool cls)
        {
            if (!Enabled)
            {
                return;
            }

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