
namespace L2dotNET.Game.model.items.effects
{
    class Calculator : ItemEffect
    {
        public Calculator()
        {
            ids = new int[] { 
                4393 //Calculator
            };
        }

        public override void UsePlayer(L2Player player, L2Item item)
        {
            player.sendPacket(new L2dotNET.Game.network.l2send.Calculator());
        }
    }
}
