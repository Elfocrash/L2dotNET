using L2dotNET.GameService.Config;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets.PartyAPI
{
    class RequestOustPartyMember : PacketBase
    {
        private string _name;
        private readonly GameClient _client;

        public RequestOustPartyMember(Packet packet, GameClient client)
        {
            _client = client;
            _name = packet.ReadString();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            if (player.Party == null)
            {
                player.SendActionFailed();
                return;
            }

            if (player.Party.Leader.ObjId != player.ObjId)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.FailedToExpelThePartyMember);
                player.SendActionFailed();
                return;
            }

            player.Party.Expel(_name);
        }
    }
}