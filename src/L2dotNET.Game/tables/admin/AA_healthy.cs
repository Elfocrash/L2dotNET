using L2dotNET.Game.model.skills2;
using L2dotNET.Game.network.l2send;

namespace L2dotNET.Game.tables.admin
{
    class AA_healthy : _adminAlias
    {
        public AA_healthy()
        {
            cmd = "healthy";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //healthy -- восстанавливает выбранному чару хп\мп

            L2Player target;
            if (admin.CurrentTarget != null && admin.CurrentTarget is L2Player)
            {
                target = (L2Player)admin.CurrentTarget;
            }
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

            SystemMessage sm = new SystemMessage(1067); //$s2 HP has been restored by $s1.
            sm.AddPlayerName(admin.Name);
            sm.AddNumber((int)hpval);
            target.sendPacket(sm);

            sm = new SystemMessage(1069); //$s2 MP has been restored by $s1.
            sm.AddPlayerName(admin.Name);
            sm.AddNumber((int)mpval);
            target.sendPacket(sm);
        }
    }
}
