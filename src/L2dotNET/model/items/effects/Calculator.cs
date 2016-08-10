using L2dotNET.model.player;

namespace L2dotNET.model.items.effects
{
    class Calculator : ItemEffect
    {
        public Calculator()
        {
            Ids = new[] { 4393 }; //Calculator
        }

        public override void UsePlayer(L2Player player, L2Item item)
        {
            player.SendPacket(new Network.serverpackets.Calculator());
        }
    }
}