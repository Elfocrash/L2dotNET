using System;
using System.Linq;
using System.Threading.Tasks;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;
using L2dotNET.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace L2dotNET.Network.clientpackets
{
    class CharacterDelete : PacketBase
    {
        private readonly ICharacterService CharacterService;

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly GameClient _client;
        private readonly int _charSlot;

        public CharacterDelete(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            CharacterService = serviceProvider.GetService<ICharacterService>();
            _charSlot = packet.ReadInt();
        }

        public override async Task RunImpl()
        {
            //if (!FloodProtectors.performAction(getClient(), Action.CHARACTER_SELECT))
            //{
            //  _client.SendPacket(new CharDeleteFail(CharDeleteFail.CharDeleteFailReason.DeletionFailed));
            //	return;
            //}

            await Task.Run(() =>
            {
                ValidateAndDelete();

                _client.SendPacketAsync(new CharacterSelectionInfo(_client.AccountName, _client.AccountCharacters, _client.SessionKey.PlayOkId1));
            });
        }

        private void ValidateAndDelete()
        {
            L2Player player = _client.AccountCharacters.FirstOrDefault(filter => filter.CharSlot == _charSlot);

            if (player == null)
            {
                Log.Warn($"{_client.Address} tried to delete Character in slot {_charSlot} but no characters exits at that slot.");
                _client.SendPacketAsync(new CharDeleteFail(CharDeleteFail.CharDeleteFailReason.DeletionFailed));
                return;
            }

            //if ((player.ClanId != 0) && (player.Clan != null))
            //{
            //    if (player.Clan.LeaderId == player.ObjId)
            //    {
            //        _client.SendPacket(new CharDeleteFail(CharDeleteFail.CharDeleteFailReason.ClanLeadersMayNotBeDeleted));
            //        return;
            //    }

            //    _client.SendPacket(new CharDeleteFail(CharDeleteFail.CharDeleteFailReason.YouMayNotDeleteClanMember));
            //    return;
            //}

            if (CharacterService.GetDaysRequiredToDeletePlayer() == 0)
            {
                if (!CharacterService.DeleteCharByObjId(player.ObjId))
                {
                    _client.SendPacketAsync(new CharDeleteFail(CharDeleteFail.CharDeleteFailReason.DeletionFailed));
                    return;
                }

                _client.DeleteCharacter(_charSlot);
            }
            else
            {
                player.SetCharDeleteTime();
                // TODO: Fix that
                //if (!_playerService.MarkToDeleteChar(player.ObjId, player.DeleteTime))
                //{
                //    _client.SendPacketAsync(new CharDeleteFail(CharDeleteFail.CharDeleteFailReason.DeletionFailed));
              //      return;
            //    }
            }

            _client.SendPacketAsync(new CharDeleteOk());
        }
    }
}