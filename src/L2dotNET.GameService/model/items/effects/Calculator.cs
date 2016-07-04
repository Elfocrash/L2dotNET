using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Model.Items.Effects
{
    class Calculator : ItemEffect
    {
        public Calculator()
        {
            ids = new[] { 4393 }; //Calculator
        }

        public override void UsePlayer(L2Player player, L2Item item)
        {
            player.SendPacket(new Network.Serverpackets.Calculator());
        }
    }
}