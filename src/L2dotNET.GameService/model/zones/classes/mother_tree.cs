using L2dotNET.GameService.Enums;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tables;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Zones.Classes
{
    class mother_tree : L2Zone
    {
        public mother_tree()
        {
            ZoneID = IdFactory.Instance.nextId();
            _enabled = true;
        }

        public override void onEnter(L2Object obj)
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
                    if (Template._affect_race.Equals("elf"))
                        if (p.BaseClass.ClassId.ClassRace != ClassRace.ELF)
                            return;

                //   p._stats.p_regen_hp += Template._hp_regen_bonus;
                //   p._stats.p_regen_mp += Template._mp_regen_bonus;
            }
        }

        public override void onExit(L2Object obj, bool cls)
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
                    if (Template._affect_race.Equals("elf"))
                        if (p.BaseClass.ClassId.ClassRace != ClassRace.ELF)
                            return;

                //   p._stats.p_regen_hp -= Template._hp_regen_bonus;
                //   p._stats.p_regen_mp -= Template._mp_regen_bonus;
            }
        }
    }
}