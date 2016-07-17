using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Model.Skills2.SpecEffects
{
    public class BRegenMpBySit : SpecEffect
    {
        public BRegenMpBySit(double value)
        {
            Value = value;
        }

        public override void OnStand(L2Player player)
        {
            if (Mul)
                player.CharacterStat.URegMpMul -= Value;
            else
                player.CharacterStat.SpecBonusRegMp -= Value;
        }

        public override void OnSit(L2Player player)
        {
            if (Mul)
                player.CharacterStat.URegMpMul += Value;
            else
                player.CharacterStat.SpecBonusRegMp += Value;
        }
    }
}