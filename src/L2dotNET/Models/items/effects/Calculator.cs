using L2dotNET.Models.Player;

namespace L2dotNET.Models.Items.Effects
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