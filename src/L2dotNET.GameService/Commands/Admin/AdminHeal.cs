using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Commands.Admin
{
    class AdminHeal : AAdminCommand
    {
        public AdminHeal()
        {
            Cmd = "heal";
        }

        protected internal override void Use(L2Player admin, string alias)
        {
            //healthy -- восстанавливает выбранному чару хп\мп

            L2Player target;
            if (admin.CurrentTarget is L2Player)
                target = (L2Player)admin.CurrentTarget;
            else
                target = admin;

            double hpval = target.CharacterStat.getStat(TEffectType.b_max_hp) - target.CurHP;
            double mpval = target.CharacterStat.getStat(TEffectType.b_max_mp) - target.CurMP;
            target.CurHP += hpval;
            target.CurMP += mpval;

            StatusUpdate su = new StatusUpdate(target.ObjID);
            su.add(StatusUpdate.CUR_HP, (int)target.CurHP);
            su.add(StatusUpdate.CUR_MP, (int)target.CurMP);
            target.sendPacket(su);

            SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.S2_HP_RESTORED_BY_S1);
            sm.AddPlayerName(admin.Name);
            sm.AddNumber((int)hpval);
            target.sendPacket(sm);

            sm = new SystemMessage(SystemMessage.SystemMessageId.S2_MP_RESTORED_BY_S1);
            sm.AddPlayerName(admin.Name);
            sm.AddNumber((int)mpval);
            target.sendPacket(sm);
        }
    }
}