using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Stats;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills2.Effects
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

        public override TEffectResult onStart(L2Character caster, L2Character target)
        {
            if (target == null)
                return nothing;

            L2Character tar = target;
            //double shieldDef = Formulas.checkShieldDef(caster, tar);
            double damage = Formulas.getPhysSkillHitDamage(caster, tar, power);

            caster.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1_HAS_GIVEN_C2_DAMAGE_OF_S3).AddPlayerName(caster.Name).AddString(tar.Name).AddNumber(damage));
            if (tar is L2Player)
                tar.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1_HAS_RECEIVED_S3_DAMAGE_FROM_C2).AddPlayerName(tar.Name).AddPlayerName(caster.Name).AddNumber(damage));

            tar.ReduceHp(caster, damage);

            return nothing;
        }
    }
}