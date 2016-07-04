using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Model.Skills2.Effects
{
    class BCriticalRate : Effect
    {
        public BCriticalRate()
        {
            SuId = StatusUpdate.Critical;
        }
    }
}