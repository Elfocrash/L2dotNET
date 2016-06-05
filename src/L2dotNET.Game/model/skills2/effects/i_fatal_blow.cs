using System;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Stats;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills2.Effects
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

        public override TEffectResult onStart(L2Character caster, L2Character target)
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