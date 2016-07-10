using L2dotNET.GameService.Enums;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tables;
using L2dotNET.GameService.World;
using L2dotNET.Utility;

namespace L2dotNET.GameService.Model.Zones.Classes
{
    class mother_tree : L2Zone
    {
        public mother_tree()
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

            if (!(obj is L2Player))
            {
                return;
            }

            L2Player p = (L2Player)obj;
            p.SendSystemMessage((SystemMessage.SystemMessageId)Template.EnteringMessageNo);
            if (Template.AffectRace.EqualsIgnoreCase("all"))
            {
                return;
            }

            if (!Template.AffectRace.EqualsIgnoreCase("elf"))
            {
                return;
            }

            if (p.BaseClass.ClassId.ClassRace != ClassRace.Elf)
            {
                return;
            }

            //   p._stats.p_regen_hp += Template._hp_regen_bonus;
            //   p._stats.p_regen_mp += Template._mp_regen_bonus;
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
                L2Player p = (L2Player)obj;
                p.SendSystemMessage((SystemMessage.SystemMessageId)Template.LeavingMessageNo);
                if (Template.AffectRace.EqualsIgnoreCase("all"))
                {
                    return;
                }

                if (!Template.AffectRace.EqualsIgnoreCase("elf"))
                {
                    return;
                }

                if (p.BaseClass.ClassId.ClassRace != ClassRace.Elf)
                {
                    return;
                }

                //   p._stats.p_regen_hp -= Template._hp_regen_bonus;
                //   p._stats.p_regen_mp -= Template._mp_regen_bonus;
            }
        }
    }
}