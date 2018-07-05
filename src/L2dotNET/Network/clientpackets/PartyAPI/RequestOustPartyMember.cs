using System;
using System.Threading.Tasks;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets.PartyAPI
{
    class RequestOustPartyMember : PacketBase
    {
        private readonly GameClient _client;
        private readonly string _name;

        public RequestOustPartyMember(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _name = packet.ReadString();
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                L2Player player = _client.CurrentPlayer;

                if (player.Party == null)
                {
                    player.SendActionFailedAsync();
                    return;
                }

                if (player.Party.Leader.CharacterId != player.CharacterId)
                {
                    player.SendSystemMessage(SystemMessage.SystemMessageId.FailedToExpelThePartyMember);
                    player.SendActionFailedAsync();
                    return;
                }

                player.Party.Expel(_name);
            });
        }
    }
}