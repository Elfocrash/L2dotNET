using L2dotNET.GameService.network.l2send;

namespace L2dotNET.GameService.model.skills2.effects
{
    class b_critical_rate : TEffect
    {
        public b_critical_rate()
        {
            SU_ID = StatusUpdate.CRITICAL;
        }
    }
}