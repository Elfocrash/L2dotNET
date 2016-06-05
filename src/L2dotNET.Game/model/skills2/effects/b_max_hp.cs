using L2dotNET.GameService.network.l2send;

namespace L2dotNET.GameService.model.skills2.effects
{
    class b_max_hp : TEffect
    {
        public b_max_hp()
        {
            SU_ID = StatusUpdate.MAX_HP;
        }
    }
}