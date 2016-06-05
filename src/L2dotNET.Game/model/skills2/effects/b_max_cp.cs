using L2dotNET.GameService.network.serverpackets;

namespace L2dotNET.GameService.Model.skills2.effects
{
    class b_max_cp : TEffect
    {
        public b_max_cp()
        {
            SU_ID = StatusUpdate.MAX_CP;
        }
    }
}