using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Model.Skills2.SpecEffects
{
    public class BRegenHpByMove : SpecEffect
    {
        public BRegenHpByMove(double value)
        {
            Value = value;
        }

        public override void OnStartMoving(L2Player player)
        {
            player.CharacterStat.SpecBonusRegHp += Value;
            player.SendMessage("reg hp inc to " + player.CharacterStat.GetStat(EffectType.BRegHp));
        }

        public override void OnStopMoving(L2Player player)
        {
            player.CharacterStat.SpecBonusRegHp -= Value;
            player.SendMessage("reg hp lowered to " + player.CharacterStat.GetStat(EffectType.BRegHp));
        }
    }
}