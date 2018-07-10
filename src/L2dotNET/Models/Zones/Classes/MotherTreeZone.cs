using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;
using L2dotNET.Tables;
using L2dotNET.Utility;

namespace L2dotNET.Models.Zones.Classes
{
    class MotherTreeZone : L2Zone
    {
        public MotherTreeZone(IdFactory idFactory) : base(idFactory)
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

            if (!(obj is L2Player))
                return;

            L2Player p = (L2Player)obj;
            p.SendSystemMessage((SystemMessageId)Template.EnteringMessageNo);
            if (Template.AffectRace.EqualsIgnoreCase("all"))
                return;

            if (!Template.AffectRace.EqualsIgnoreCase("elf"))
                return;

            if (p.BaseClass.ClassId.ClassRace != ClassRace.Elf)
                return;

            //   p._stats.p_regen_hp += Template._hp_regen_bonus;
            //   p._stats.p_regen_mp += Template._mp_regen_bonus;
        }

        public override void OnExit(L2Object obj, bool cls)
        {
            if (!Enabled)
                return;

            base.OnExit(obj, cls);

            obj.OnExitZoneAsync(this, cls);

            if (!(obj is L2Player))
                return;

            L2Player p = (L2Player)obj;
            p.SendSystemMessage((SystemMessageId)Template.LeavingMessageNo);
            if (Template.AffectRace.EqualsIgnoreCase("all"))
                return;

            if (!Template.AffectRace.EqualsIgnoreCase("elf"))
                return;

            if (p.BaseClass.ClassId.ClassRace != ClassRace.Elf)
                return;

            //   p._stats.p_regen_hp -= Template._hp_regen_bonus;
            //   p._stats.p_regen_mp -= Template._mp_regen_bonus;
        }
    }
}