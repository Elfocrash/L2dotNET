using L2dotNET.Models.player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets.PartyAPI
{
    class RequestOustPartyMember : PacketBase
    {
        private readonly GameClient _client;
        private readonly string _name;

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