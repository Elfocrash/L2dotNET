using L2dotNET.Models.player;
using L2dotNET.tables;

namespace L2dotNET.Models.zones.classes
{
    public class monster_race : L2Zone
    {
        public monster_race()
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
                // ((L2Player)obj).sendPacket(new PlaySound("S_Race", true));
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
                // ((L2Player)obj).sendMessage("leaved monster race");

                //    p._stats.p_regen_hp += Template._hp_regen_bonus;
                //   p._stats.p_regen_mp += Template._mp_regen_bonus;
            }
        }
    }
}