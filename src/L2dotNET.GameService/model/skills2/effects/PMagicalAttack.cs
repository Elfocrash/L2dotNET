using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Model.Skills2.Effects
{
    class PMagicalAttack : Effect
    {
        public PMagicalAttack()
        {
            SuId = StatusUpdate.MAtk;
            Type = EffectType.PMagicalAttack;
        }
    }
}