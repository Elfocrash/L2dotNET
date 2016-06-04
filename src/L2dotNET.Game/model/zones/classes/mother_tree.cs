using L2dotNET.GameService.Enums;
using L2dotNET.GameService.network.l2send;
using L2dotNET.GameService.tables;

namespace L2dotNET.GameService.model.zones.classes
{
    class mother_tree : L2Zone
    {
        public mother_tree()
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
                L2Player p = (L2Player)obj;
                p.sendSystemMessage((SystemMessage.SystemMessageId)Template._entering_message_no);
                if (!Template._affect_race.Equals("all"))
                {
                    if (Template._affect_race.Equals("elf"))
                        if (p.BaseClass.ClassId.ClassRace != ClassRace.ELF)
                            return;
                }

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
                L2Player p = (L2Player)obj;
                p.sendSystemMessage((SystemMessage.SystemMessageId)Template._leaving_message_no); 
                if (!Template._affect_race.Equals("all"))
                {
                    if (Template._affect_race.Equals("elf"))
                        if (p.BaseClass.ClassId.ClassRace != ClassRace.ELF)
                            return;
                }

             //   p._stats.p_regen_hp -= Template._hp_regen_bonus;
             //   p._stats.p_regen_mp -= Template._mp_regen_bonus;
            }
        }
    }
}
