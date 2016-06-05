using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Model.Skills2.Effects
{
    class b_max_mp : TEffect
    {
        public b_max_mp()
        {
            SU_ID = StatusUpdate.MAX_MP;
        }
    }
}