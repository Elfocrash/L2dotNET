using L2dotNET.Models.Player;
using L2dotNET.Tables;

namespace L2dotNET.Models.Zones.Classes
{
    public class MonsterRaceZone : L2Zone
    {
        public MonsterRaceZone(IdFactory idFactory) : base(idFactory)
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

            obj.OnExitZoneAsync(this, cls);

            if (obj is L2Player)
            {
                // ((L2Player)obj).sendMessage("leaved monster race");

                //    p._stats.p_regen_hp += Template._hp_regen_bonus;
                //   p._stats.p_regen_mp += Template._mp_regen_bonus;
            }
        }
    }
}