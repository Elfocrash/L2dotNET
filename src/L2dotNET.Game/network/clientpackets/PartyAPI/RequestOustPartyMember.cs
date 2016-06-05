using L2dotNET.GameService.Model.player;
using L2dotNET.GameService.network.serverpackets;

namespace L2dotNET.GameService.network.clientpackets.PartyAPI
{
    class RequestOustPartyMember : GameServerNetworkRequest
    {
        private string name;

        public RequestOustPartyMember(GameClient client, byte[] data)
        {
            base.makeme(client, data);
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
                player.sendActionFailed();
                return;
            }

            if (player.Party.leader.ObjID != player.ObjID)
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.FAILED_TO_EXPEL_THE_PARTY_MEMBER);
                player.sendActionFailed();
                return;
            }

            player.Party.Expel(name);
        }
    }
}