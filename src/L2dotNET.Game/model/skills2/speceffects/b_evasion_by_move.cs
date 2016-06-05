using L2dotNET.GameService.network.l2send;

namespace L2dotNET.GameService.model.skills2.speceffects
{
    public class b_evasion_by_move : TSpecEffect
    {
        public b_evasion_by_move(double value)
        {
            this.value = value;
        }

        public override void OnStartMoving(L2Player player)
        {
            player.CharacterStat.SpecBonusEvasion += value;

            StatusUpdate su = new StatusUpdate(player.ObjID);
            su.add(StatusUpdate.EVASION, (int)player.CharacterStat.getStat(TEffectType.b_evasion));
            player.sendPacket(su);
        }

        public override void OnStopMoving(L2Player player)
        {
            player.CharacterStat.SpecBonusEvasion -= value;

            StatusUpdate su = new StatusUpdate(player.ObjID);
            su.add(StatusUpdate.EVASION, (int)player.CharacterStat.getStat(TEffectType.b_evasion));
            player.sendPacket(su);
        }
    }
}