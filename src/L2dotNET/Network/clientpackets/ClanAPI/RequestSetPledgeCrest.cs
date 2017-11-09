using L2dotNET.model.player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets.ClanAPI
{
    class RequestSetPledgeCrest : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _size;
        private readonly byte[] _picture;

        public RequestSetPledgeCrest(Packet packet, GameClient client)
        {
            _client = client;
            _size = packet.ReadInt();

            if ((_size > 0) && (_size <= 256))
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

            //if (clan.Level < 3)
            //{
            //    player.SendSystemMessage(SystemMessage.SystemMessageId.ClanLvl3NeededToSetCrest);
            //    player.SendActionFailed();
            //    return;
            //}

            //if (clan.IsDissolving())
            //{
            //    player.SendSystemMessage(SystemMessage.SystemMessageId.CannotSetCrestWhileDissolutionInProgress);
            //    player.SendActionFailed();
            //    return;
            //}

            //if ((_size < 0) || (_size > 256))
            //{
            //    player.SendSystemMessage(SystemMessage.SystemMessageId.CanOnlyRegister1612Px256ColorBmpFiles);
            //    player.SendActionFailed();
            //    return;
            //}

            //if ((player.ClanPrivs & L2Clan.CpClRegisterCrest) != L2Clan.CpClRegisterCrest)
            //{
            //    player.SendSystemMessage(SystemMessage.SystemMessageId.NotAuthorizedToBestowRights);
            //    player.SendActionFailed();
            //    return;
            //}

            //clan.UpdateCrest(_size, _picture);
        }
    }
}