using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Model.Skills2.SpecEffects
{
    public class BEvasionByMove : SpecEffect
    {
        public BEvasionByMove(double value)
        {
            Value = value;
        }

        public override void OnStartMoving(L2Player player)
        {
            player.CharacterStat.SpecBonusEvasion += Value;

            StatusUpdate su = new StatusUpdate(player.ObjId);
            su.Add(StatusUpdate.Evasion, (int)player.CharacterStat.GetStat(EffectType.BEvasion));
            player.SendPacket(su);
        }

        public override void OnStopMoving(L2Player player)
        {
            player.CharacterStat.SpecBonusEvasion -= Value;

            StatusUpdate su = new StatusUpdate(player.ObjId);
            su.Add(StatusUpdate.Evasion, (int)player.CharacterStat.GetStat(EffectType.BEvasion));
            player.SendPacket(su);
        }
    }
}