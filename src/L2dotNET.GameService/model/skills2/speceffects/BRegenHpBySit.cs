using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Model.Skills2.SpecEffects
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