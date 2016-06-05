using L2dotNET.GameService.Model.player;

namespace L2dotNET.GameService.Model.items.effects
{
    class Calculator : ItemEffect
    {
        public Calculator()
        {
            ids = new int[] { 4393 }; //Calculator
        }

        public override void UsePlayer(L2Player player, L2Item item)
        {
            player.sendPacket(new network.serverpackets.Calculator());
        }
    }
}