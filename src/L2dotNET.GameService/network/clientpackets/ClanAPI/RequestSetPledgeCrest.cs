using L2dotNET.GameService.Model.Communities;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets.ClanAPI
{
    class RequestSetPledgeCrest : GameServerNetworkRequest
    {
        private int _size;
        private byte[] _picture;

        public RequestSetPledgeCrest(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        public override void Read()
        {
            _size = ReadD();

            if ((_size > 0) && (_size <= 256))
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

            if (clan.Level < 3)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.ClanLvl3NeededToSetCrest);
                player.SendActionFailed();
                return;
            }

            if (clan.IsDissolving())
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.CannotSetCrestWhileDissolutionInProgress);
                player.SendActionFailed();
                return;
            }

            if ((_size < 0) || (_size > 256))
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.CanOnlyRegister1612Px256ColorBmpFiles);
                player.SendActionFailed();
                return;
            }

            if ((player.ClanPrivs & L2Clan.CpClRegisterCrest) != L2Clan.CpClRegisterCrest)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.NotAuthorizedToBestowRights);
                player.SendActionFailed();
                return;
            }

            clan.UpdateCrest(_size, _picture);
        }
    }
}