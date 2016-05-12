using L2dotNET.GameService.model.stats;
using L2dotNET.GameService.network.l2send;
using L2dotNET.GameService.world;
using System;

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

            //$c1 has given $c2 damage of $s3.
            caster.sendPacket(new SystemMessage(2261).AddPlayerName(caster.Name).AddString(tar.Name).AddNumber(damage));
            if (tar is L2Player) //$c1 has received $s3 damage from $c2.
                tar.sendPacket(new SystemMessage(2262).AddPlayerName(tar.Name).AddPlayerName(caster.Name).AddNumber(damage));

            tar.reduceHp(caster, damage);

            return nothing;
        }
    }
}
