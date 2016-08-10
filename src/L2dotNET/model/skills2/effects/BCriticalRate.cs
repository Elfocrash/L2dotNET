using L2dotNET.Network.serverpackets;

namespace L2dotNET.model.skills2.effects
{
    class BCriticalRate : Effect
    {
        public BCriticalRate()
        {
            SuId = StatusUpdate.Critical;
        }
    }
}