using L2dotNET.model.player;

namespace L2dotNET.model.skills2.speceffects
{
    public class BRegenHpBySit : SpecEffect
    {
        public BRegenHpBySit(double value)
        {
            Value = value;
        }

        public override void OnStand(L2Player player)
        {
            if (Mul)
                player.CharacterStat.URegHpMul -= Value;
            else
                player.CharacterStat.SpecBonusRegHp -= Value;

            player.SendMessage("reg hp lowered");
        }

        public override void OnSit(L2Player player)
        {
            if (Mul)
                player.CharacterStat.URegHpMul += Value;
            else
                player.CharacterStat.SpecBonusRegHp += Value;

            player.SendMessage("reg hp inc");
        }
    }
}