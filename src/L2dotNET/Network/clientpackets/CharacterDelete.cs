using System.Linq;
using log4net;
using L2dotNET.model.player;
using L2dotNET.Network.serverpackets;
using L2dotNET.Services.Contracts;
using Ninject;

namespace L2dotNET.Network.clientpackets
{
    class CharacterDelete : PacketBase
    {
        [Inject]
        public IPlayerService PlayerService => GameServer.Kernel.Get<IPlayerService>();

        private static readonly ILog Log = LogManager.GetLogger(typeof(CharacterDelete));

        private readonly GameClient _client;
        private readonly int _charSlot;

        public CharacterDelete(Packet packet, GameClient client)
        {
            _client = client;
            _charSlot = packet.ReadInt();
        }

        public override void RunImpl()
        {
            //if (!FloodProtectors.performAction(getClient(), Action.CHARACTER_SELECT))
            //{
            //  _client.SendPacket(new CharDeleteFail(CharDeleteFail.CharDeleteFailReason.DeletionFailed));
            //	return;
            //}

            ValidateAndDelete();

            _client.SendPacket(new CharacterSelectionInfo(_client.AccountName, _client.AccountChars, _client.SessionId));
        }

        private void ValidateAndDelete()
        {
            L2Player player = _client.AccountChars.FirstOrDefault(filter => filter.CharSlot == _charSlot);

            if (player == null)
            {
                Log.Warn($"{_client.Address} tried to delete Character in slot {_charSlot} but no characters exits at that slot.");
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
                player.SetCharDeleteTime();
                if (!PlayerService.MarkToDeleteChar(player.ObjId, player.DeleteTime))
                {
                    _client.SendPacket(new CharDeleteFail(CharDeleteFail.CharDeleteFailReason.DeletionFailed));
                    return;
                }
            }

            _client.SendPacket(new CharDeleteOk());
        }
    }
}