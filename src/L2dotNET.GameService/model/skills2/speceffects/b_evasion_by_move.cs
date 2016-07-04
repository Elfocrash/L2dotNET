using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Model.Skills2.SpecEffects
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

            StatusUpdate su = new StatusUpdate(player.ObjId);
            su.add(StatusUpdate.EVASION, (int)player.CharacterStat.getStat(TEffectType.b_evasion));
            player.SendPacket(su);
        }

        public override void OnStopMoving(L2Player player)
        {
            player.CharacterStat.SpecBonusEvasion -= value;

            StatusUpdate su = new StatusUpdate(player.ObjId);
            su.add(StatusUpdate.EVASION, (int)player.CharacterStat.getStat(TEffectType.b_evasion));
            player.SendPacket(su);
        }
    }
}