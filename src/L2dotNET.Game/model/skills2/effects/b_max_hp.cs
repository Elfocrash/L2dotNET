using L2dotNET.GameService.network.serverpackets;

namespace L2dotNET.GameService.Model.skills2.effects
{
    class b_max_hp : TEffect
    {
        public b_max_hp()
        {
            SU_ID = StatusUpdate.MAX_HP;
        }
    }
}