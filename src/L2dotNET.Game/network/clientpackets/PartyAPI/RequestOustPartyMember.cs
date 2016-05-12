
namespace L2dotNET.GameService.network.l2recv
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
                player.sendSystemMessage(317);//You have failed to expel the party member.
                player.sendActionFailed();
                return;
            }

            player.Party.Expel(name);
        }
    }
}
