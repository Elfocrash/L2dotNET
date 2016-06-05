using L2dotNET.GameService.model.stats;
using L2dotNET.GameService.network.l2send;
using L2dotNET.GameService.world;

namespace L2dotNET.GameService.model.skills2.effects
{
    class i_p_attack : TEffect
    {
        public i_p_attack()
        {
            type = TEffectType.i_p_attack;
        }

        private int power;
        private int unk1;
        private int unk2;
        private int unk3;

        public override void build(string str)
        {
            string[] v = str.Split(' ');
            power = int.Parse(v[1]);
            unk1 = int.Parse(v[2]);
            unk2 = int.Parse(v[2]);
            unk3 = int.Parse(v[2]);
        }

        public override TEffectResult onStart(L2Character caster, world.L2Character target)
        {
            if (!(target is L2Character))
                return nothing;

            L2Character tar = (L2Character)target;
            double shieldDef = Formulas.checkShieldDef(caster, tar);
            double damage = Formulas.getPhysSkillHitDamage(caster, tar, power);

            caster.sendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1_HAS_GIVEN_C2_DAMAGE_OF_S3).AddPlayerName(caster.Name).AddString(tar.Name).AddNumber(damage));
            if (tar is L2Player)
                tar.sendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1_HAS_RECEIVED_S3_DAMAGE_FROM_C2).AddPlayerName(tar.Name).AddPlayerName(caster.Name).AddNumber(damage));

            tar.reduceHp(caster, damage);

            return nothing;
        }
    }
}