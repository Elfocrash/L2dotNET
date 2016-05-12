using System;
using L2dotNET.GameService.model.stats;
using L2dotNET.GameService.network.l2send;
using L2dotNET.GameService.world;

namespace L2dotNET.GameService.model.skills2.effects
{
    class i_fatal_blow : TEffect
    {
        private int power;
        public override void build(string str)
        {
            string[] v = str.Split(' ');
            power = int.Parse(v[1]);
            unk1 = int.Parse(v[2]);
            unk2 = int.Parse(v[3]);
        }

        public override TEffectResult onStart(L2Character caster, world.L2Character target)
        {
            if (!tempSuccess)
                return nothing;

            double shieldDef = Formulas.checkShieldDef(caster, target);
            double damage = Formulas.getPhysSkillHitDamage(caster, target, power);

            //$c1 has given $c2 damage of $s3.
            caster.sendPacket(new SystemMessage(2261).AddPlayerName(caster.Name).AddString(target.Name).AddNumber(damage));
            if (target is L2Player) //$c1 has received $s3 damage from $c2.
                target.sendPacket(new SystemMessage(2262).AddPlayerName(target.Name).AddPlayerName(caster.Name).AddNumber(damage));

            target.reduceHp(caster, damage);

            return nothing;
        }

        private bool tempSuccess = false;
        private int unk1;
        private int unk2;
        public byte success(L2Character target)
        {
            tempSuccess = new Random().Next(100) <= 50;
            return tempSuccess ? (byte)1 : (byte)0;
        }
    }
}
