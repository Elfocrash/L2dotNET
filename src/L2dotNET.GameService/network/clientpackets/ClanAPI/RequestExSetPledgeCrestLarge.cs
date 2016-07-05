using L2dotNET.GameService.Model.Communities;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets.ClanAPI
{
    class RequestExSetPledgeCrestLarge : GameServerNetworkRequest
    {
        private int _size;
        private byte[] _picture;

        public RequestExSetPledgeCrestLarge(GameClient client, byte[] data)
        {
            Makeme(client, data, 2);
        }

        public override void Read()
        {
            _size = ReadD();

            if ((_size > 0) && (_size <= 2176))
            {
                _picture = ReadB(_size);
            }
        }

        public override void Run()
        {
            L2Player player = GetClient().CurrentPlayer;

            if (player.ClanId == 0)
            {
                player.SendActionFailed();
                return;
            }

            L2Clan clan = player.Clan;

            if ((clan.HideoutId == 0) || (clan.CastleId == 0))
            {
                player.SendMessage("You need to own clan hall or castle to assign this emblem.");
                player.SendActionFailed();
                return;
            }

            if ((_size < 0) || (_size > 2176))
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.LengthCrestDoesNotMeetStandardRequirements);
                player.SendActionFailed();
                return;
            }

            if ((player.ClanPrivs & L2Clan.CpClRegisterCrest) != L2Clan.CpClRegisterCrest)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.NotAuthorizedToBestowRights);
                player.SendActionFailed();
                return;
            }

            clan.UpdateCrestLarge(_size, _picture);
        }
    }
}