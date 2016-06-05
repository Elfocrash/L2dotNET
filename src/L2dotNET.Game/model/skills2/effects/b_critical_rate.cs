using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Model.Skills2.Effects
{
    class b_critical_rate : TEffect
    {
        public b_critical_rate()
        {
            SU_ID = StatusUpdate.CRITICAL;
        }
    }
}