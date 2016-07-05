using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Stats;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills2.Effects
{
    class IpAttack : Effect
    {
        public IpAttack()
        {
            Type = EffectType.IPAttack;
        }

        private int _power;
        private int _unk1;
        private int _unk2;
        private int _unk3;

        public override void Build(string str)
        {
            string[] v = str.Split(' ');
            _power = int.Parse(v[1]);
            _unk1 = int.Parse(v[2]);
            _unk2 = int.Parse(v[2]);
            _unk3 = int.Parse(v[2]);
        }

        public override EffectResult OnStart(L2Character caster, L2Character target)
        {
            if (target == null)
            {
                return Nothing;
            }

            L2Character tar = target;
            //double shieldDef = Formulas.checkShieldDef(caster, tar);
            double damage = Formulas.GetPhysSkillHitDamage(caster, tar, _power);

            caster.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1HasGivenC2DamageOfS3).AddPlayerName(caster.Name).AddString(tar.Name).AddNumber(damage));
            if (tar is L2Player)
            {
                tar.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1HasReceivedS3DamageFromC2).AddPlayerName(tar.Name).AddPlayerName(caster.Name).AddNumber(damage));
            }

            tar.ReduceHp(caster, damage);

            return Nothing;
        }
    }
}