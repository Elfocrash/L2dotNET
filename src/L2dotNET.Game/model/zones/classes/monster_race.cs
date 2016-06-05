using L2dotNET.GameService.Model.player;
using L2dotNET.GameService.tables;

namespace L2dotNET.GameService.Model.zones.classes
{
    public class monster_race : L2Zone
    {
        public monster_race()
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
                // ((L2Player)obj).sendPacket(new PlaySound("S_Race", true));
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
                // ((L2Player)obj).sendMessage("leaved monster race");

                //    p._stats.p_regen_hp += Template._hp_regen_bonus;
                //   p._stats.p_regen_mp += Template._mp_regen_bonus;
            }
        }
    }
}