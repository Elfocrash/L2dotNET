
namespace L2dotNET.GameService.model.skills2.speceffects
{
    public class b_regen_mp_by_sit : TSpecEffect
    {
        public b_regen_mp_by_sit(double value)
        {
            this.value = value;
        }

        public override void OnStand(L2Player player)
        {
            if (mul)
                player.CharacterStat.URegMpMul -= value;
            else
                player.CharacterStat.SpecBonusRegMP -= value;
        }

        public override void OnSit(L2Player player)
        {
            if (mul)
                player.CharacterStat.URegMpMul += value;
            else
                player.CharacterStat.SpecBonusRegMP += value;
        }
    }
}
