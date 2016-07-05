using System;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Stats;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills2.Effects
{
    internal class FatalBlow : Effect
    {
        private int _power;

        public override void Build(string str)
        {
            string[] v = str.Split(' ');
            _power = int.Parse(v[1]);
            _unk1 = int.Parse(v[2]);
            _unk2 = int.Parse(v[3]);
        }

        public override EffectResult OnStart(L2Character caster, L2Character target)
        {
            if (!_tempSuccess)
            {
                return Nothing;
            }

            //double shieldDef = Formulas.checkShieldDef(caster, target);
            double damage = Formulas.GetPhysSkillHitDamage(caster, target, _power);

            caster.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1HasGivenC2DamageOfS3).AddPlayerName(caster.Name).AddString(target.Name).AddNumber(damage));
            if (target is L2Player)
            {
                target.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1HasReceivedS3DamageFromC2).AddPlayerName(target.Name).AddPlayerName(caster.Name).AddNumber(damage));
            }

            target.ReduceHp(caster, damage);

            return Nothing;
        }

        private bool _tempSuccess;
        private int _unk1;
        private int _unk2;

        public byte Success(L2Character target)
        {
            _tempSuccess = new Random().Next(100) <= 50;
            return _tempSuccess ? (byte)1 : (byte)0;
        }
    }
}