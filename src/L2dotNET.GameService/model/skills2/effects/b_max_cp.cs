using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Model.Skills2.Effects
{
    class b_max_cp : TEffect
    {
        public b_max_cp()
        {
            SU_ID = StatusUpdate.MAX_CP;
        }
    }
}