using L2dotNET.model.player;

namespace L2dotNET.model.skills2.speceffects
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