using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Model.Skills2.Effects
{
    class BMaxHp : Effect
    {
        public BMaxHp()
        {
            SuId = StatusUpdate.MaxHp;
        }
    }
}