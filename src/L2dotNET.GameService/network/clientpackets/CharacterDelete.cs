using System.Linq;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.Network;
using L2dotNET.Services.Contracts;
using Ninject;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class CharacterDelete : PacketBase
    {
        [Inject]
        public IPlayerService PlayerService => GameServer.Kernel.Get<IPlayerService>();
        private GameClient _client;
        public CharacterDelete(Packet packet, GameClient client)
        {
            _client = client;
            _charSlot = packet.ReadInt();
        }

        private int _charSlot;

        public override void RunImpl()
        {
            //if (!FloodProtectors.performAction(getClient(), Action.CHARACTER_SELECT))
            //{
            //	getClient().sendPacket(new CharDeleteFail(CharDeleteFail.CharDeleteFailReason.DELETION_FAILED));
            //	return;
            //}

            L2Player player = _client.AccountChars.FirstOrDefault(filter => filter.CharSlot == _charSlot);

            if (player == null)
            {
                _client.SendPacket(new CharDeleteFail(CharDeleteFail.CharDeleteFailReason.DeletionFailed));
                return;
            }

            if ((player.ClanId != 0) && (player.Clan != null))
            {
                if (player.Clan.LeaderId == player.ObjId)
                {
                    _client.SendPacket(new CharDeleteFail(CharDeleteFail.CharDeleteFailReason.ClanLeadersMayNotBeDeleted));
                    return;
                }

                _client.SendPacket(new CharDeleteFail(CharDeleteFail.CharDeleteFailReason.YouMayNotDeleteClanMember));
                return;
            }

            if (Config.Config.Instance.GameplayConfig.Server.Client.DeleteCharAfterDays == 0)
            {
                if (!PlayerService.DeleteCharByObjId(player.ObjId))
                {
                    _client.SendPacket(new CharDeleteFail(CharDeleteFail.CharDeleteFailReason.DeletionFailed));
                    return;
                }

                _client.RemoveAccountCharAndResetSlotIndex(_charSlot);
            }
            else
            {
                if (!PlayerService.MarkToDeleteChar(player.ObjId))
                {
                    _client.SendPacket(new CharDeleteFail(CharDeleteFail.CharDeleteFailReason.DeletionFailed));
                    return;
                }
            }

            _client.SendPacket(new CharDeleteOk());
            _client.SendPacket(new CharacterSelectionInfo(_client.AccountName, _client.AccountChars, _client.SessionId));
        }
    }
}