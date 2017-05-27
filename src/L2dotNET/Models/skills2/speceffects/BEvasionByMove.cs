using L2dotNET.model.player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.model.skills2.speceffects
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