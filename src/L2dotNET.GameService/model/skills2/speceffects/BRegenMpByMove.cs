using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Model.Skills2.SpecEffects
{
    public class BRegenMpByMove : SpecEffect
    {
        public BRegenMpByMove(double value)
        {
            Value = value;
        }

        public override void OnStartMoving(L2Player player)
        {
            player.CharacterStat.SpecBonusRegMp += Value;
        }

        public override void OnStopMoving(L2Player player)
        {
            player.CharacterStat.SpecBonusRegMp -= Value;
        }
    }
}