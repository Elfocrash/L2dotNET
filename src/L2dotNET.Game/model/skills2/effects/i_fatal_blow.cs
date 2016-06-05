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

            caster.sendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1_HAS_GIVEN_C2_DAMAGE_OF_S3).AddPlayerName(caster.Name).AddString(target.Name).AddNumber(damage));
            if (target is L2Player)
                target.sendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1_HAS_RECEIVED_S3_DAMAGE_FROM_C2).AddPlayerName(target.Name).AddPlayerName(caster.Name).AddNumber(damage));

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