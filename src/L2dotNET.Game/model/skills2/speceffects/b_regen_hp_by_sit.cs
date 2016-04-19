
namespace L2dotNET.Game.model.skills2.speceffects
{
    public class b_regen_hp_by_sit : TSpecEffect
    {
        public b_regen_hp_by_sit(double value)
        {
            this.value = value;
        }

        public override void OnStand(L2Player player)
        {
            if (mul)
                player.CharacterStat.URegHpMul -= value;
            else
                player.CharacterStat.SpecBonusRegHP -= value;

            player.sendMessage("reg hp lowered");
        }

        public override void OnSit(L2Player player)
        {
            if (mul)
                player.CharacterStat.URegHpMul += value;
            else
                player.CharacterStat.SpecBonusRegHP += value;

            player.sendMessage("reg hp inc");
        }
    }
}
