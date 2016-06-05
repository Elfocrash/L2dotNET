using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Model.Skills2.SpecEffects
{
    public class b_regen_mp_by_move : TSpecEffect
    {
        public b_regen_mp_by_move(double value)
        {
            this.value = value;
        }

        public override void OnStartMoving(L2Player player)
        {
            player.CharacterStat.SpecBonusRegMP += value;
        }

        public override void OnStopMoving(L2Player player)
        {
            player.CharacterStat.SpecBonusRegMP -= value;
        }
    }
}