using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets.PartyAPI
{
    class RequestOustPartyMember : GameServerNetworkRequest
    {
        private string name;

        public RequestOustPartyMember(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            name = readS();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            if (player.Party == null)
            {
                player.SendActionFailed();
                return;
            }

            if (player.Party.leader.ObjId != player.ObjId)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.FAILED_TO_EXPEL_THE_PARTY_MEMBER);
                player.SendActionFailed();
                return;
            }

            player.Party.Expel(name);
        }
    }
}