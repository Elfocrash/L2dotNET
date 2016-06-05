using L2dotNET.GameService.network.serverpackets;

namespace L2dotNET.GameService.Model.skills2.effects
{
    class b_critical_rate : TEffect
    {
        public b_critical_rate()
        {
            SU_ID = StatusUpdate.CRITICAL;
        }
    }
}