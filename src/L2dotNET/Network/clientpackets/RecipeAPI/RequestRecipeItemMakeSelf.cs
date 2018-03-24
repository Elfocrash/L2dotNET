using System;
using L2dotNET.Models.player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets.RecipeAPI
{
    class RequestRecipeItemMakeSelf : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _id;

        public RequestRecipeItemMakeSelf(Packet packet, GameClient client)
        {
            _client = client;
            _id = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;
        }
    }
}