﻿using System;
using System.Linq;
using System.Threading.Tasks;
using L2dotNET.Logging.Abstraction;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;
using L2dotNET.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace L2dotNET.Network.clientpackets
{
    class CharacterDelete : PacketBase
    {
        private readonly IPlayerService _playerService;

        private static readonly ILog Log = LogProvider.GetCurrentClassLogger();

        private readonly GameClient _client;
        private readonly int _charSlot;

        public CharacterDelete(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _playerService = serviceProvider.GetService<IPlayerService>();
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

                _client.SendPacketAsync(new CharacterSelectionInfo(_client.AccountName, _client.AccountChars, _client.SessionKey.PlayOkId1));
            });
        }

        private void ValidateAndDelete()
        {
            L2Player player = _client.AccountChars.FirstOrDefault(filter => filter.CharSlot == _charSlot);

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

            if (_playerService.GetDaysRequiredToDeletePlayer() == 0)
            {
                if (!_playerService.DeleteCharByObjId(player.ObjId))
                {
                    _client.SendPacketAsync(new CharDeleteFail(CharDeleteFail.CharDeleteFailReason.DeletionFailed));
                    return;
                }

                _client.RemoveAccountCharAndResetSlotIndex(_charSlot);
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