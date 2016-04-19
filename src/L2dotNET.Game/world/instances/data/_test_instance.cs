
namespace L2dotNET.Game.world.instances.data
{
    class _test_instance : InstanceTemplate
    {
        public _test_instance()
        {
            ClientId = 1;
            ClientName = "Party Duel";

            reuseH = 2;
            reuseM = 30;

            ActionTime = 1 * 60;

            WholeZoneIsPvp = true;
        } 
    }
}
