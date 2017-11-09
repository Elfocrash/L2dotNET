using L2dotNET.model.player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets.ClanAPI
{
    class RequestExSetPledgeCrestLarge : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _size;
        private readonly byte[] _picture;

        public RequestExSetPledgeCrestLarge(Packet packet, GameClient client)
        {
            packet.MoveOffset(2);
            _client = client;
            _size = packet.ReadInt();

            if ((_size > 0) && (_size <= 2176))
                _picture = packet.ReadByteArrayAlt(_size);
        }

        public override void RunImpl()
        {
            //L2Player player = _client.CurrentPlayer;

            //if (player.ClanId == 0)
            //{
            //    player.SendActionFailed();
            //    return;
            //}

            //L2Clan clan = player.Clan;

            //if ((clan.HideoutId == 0) || (clan.CastleId == 0))
            //{
            //    player.SendMessage("You need to own clan hall or castle to assign this emblem.");
            //    player.SendActionFailed();
            //    return;
            //}

            //if ((_size < 0) || (_size > 2176))
            //{
            //    player.SendSystemMessage(SystemMessage.SystemMessageId.LengthCrestDoesNotMeetStandardRequirements);
            //    player.SendActionFailed();
            //    return;
            //}

            //if ((player.ClanPrivs & L2Clan.CpClRegisterCrest) != L2Clan.CpClRegisterCrest)
            //{
            //    player.SendSystemMessage(SystemMessage.SystemMessageId.NotAuthorizedToBestowRights);
            //    player.SendActionFailed();
            //    return;
            //}

            //clan.UpdateCrestLarge(_size, _picture);
        }
    }
}