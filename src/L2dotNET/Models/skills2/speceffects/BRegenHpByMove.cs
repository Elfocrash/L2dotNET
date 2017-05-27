using L2dotNET.model.player;

namespace L2dotNET.model.skills2.speceffects
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
            player.SendMessage($"reg hp inc to {player.CharacterStat.GetStat(EffectType.BRegHp)}");
        }

        public override void OnStopMoving(L2Player player)
        {
            player.CharacterStat.SpecBonusRegHp -= Value;
            player.SendMessage($"reg hp lowered to {player.CharacterStat.GetStat(EffectType.BRegHp)}");
        }
    }
}