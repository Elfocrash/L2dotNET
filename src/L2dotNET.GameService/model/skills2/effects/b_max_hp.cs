using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Model.Skills2.Effects
{
    class b_max_hp : TEffect
    {
        public b_max_hp()
        {
            SU_ID = StatusUpdate.MAX_HP;
        }
    }
}