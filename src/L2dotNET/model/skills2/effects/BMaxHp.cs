using L2dotNET.Network.serverpackets;

namespace L2dotNET.model.skills2.effects
{
    class BMaxHp : Effect
    {
        public BMaxHp()
        {
            SuId = StatusUpdate.MaxHp;
        }
    }
}