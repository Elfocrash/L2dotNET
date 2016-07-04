using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets.PartyAPI
{
    class RequestOustPartyMember : GameServerNetworkRequest
    {
        private string _name;

        public RequestOustPartyMember(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        public override void Read()
        {
            _name = ReadS();
        }

        public override void Run()
        {
            L2Player player = Client.CurrentPlayer;

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