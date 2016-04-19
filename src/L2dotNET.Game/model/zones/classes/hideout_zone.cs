using L2dotNET.Game.model.player.basic;
using L2dotNET.Game.tables;

namespace L2dotNET.Game.model.zones.classes
{
    class hideout_zone : L2Zone
    {
        public structures.Hideout hideout;
        public hideout_zone()
        {
            ZoneID = IdFactory.getInstance().nextId();
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
                

            //    p._stats.p_regen_hp += Template._hp_regen_bonus;
             //   p._stats.p_regen_mp += Template._mp_regen_bonus;
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
                

                //    p._stats.p_regen_hp += Template._hp_regen_bonus;
                //   p._stats.p_regen_mp += Template._mp_regen_bonus;
            }
        }
    }
}
