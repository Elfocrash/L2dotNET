using System;
using System.Linq;
using System.Threading.Tasks;
using L2dotNET.DataContracts;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;
using L2dotNET.Services.Contracts;
using L2dotNET.Utility;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace L2dotNET.Network.clientpackets
{
    class CharacterDelete : PacketBase
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly ICharacterService _characterService;
        private readonly ICrudService<CharacterContract> _characterCrudService;
        private readonly GameClient _client;
        private readonly int _charSlot;

        public CharacterDelete(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _characterService = serviceProvider.GetService<ICharacterService>();
            _characterCrudService = serviceProvider.GetService<ICrudService<CharacterContract>>();
            _charSlot = packet.ReadInt();
        }

        public override async Task RunImpl()
        {
            ValidateAndDelete();

            _client.SendPacketAsync(new CharList(_client, _client.SessionKey.PlayOkId1));

        }

        private void ValidateAndDelete()
        {
            L2Player player = _client.AccountCharacters.FirstOrDefault(filter => filter.CharacterSlot == _charSlot);

            if (player == null)
            {
                Log.Warn($"{_client.Address} tried to delete Character in slot {_charSlot} but no characters exits at that slot.");
                _client.SendPacketAsync(new CharDeleteFail(CharDeleteFail.CharDeleteFailReason.DeletionFailed));
                return;
            }

            // TODO: rework that when clan system would be done
            /*if ((player.ClanId != 0) && (player.Clan != null))
            {
                if (player.Clan.LeaderId == player.ObjId)
                {
                    _client.SendPacket(new CharDeleteFail(CharDeleteFail.CharDeleteFailReason.ClanLeadersMayNotBeDeleted));
                    return;
                }
                _client.SendPacket(new CharDeleteFail(CharDeleteFail.CharDeleteFailReason.YouMayNotDeleteClanMember));
                return;
            }*/

            if (_characterService.GetDaysRequiredToDeletePlayer() == 0)
            {
                if (!_characterService.DeleteCharById(player.ObjectId))
                {
                    _client.SendPacketAsync(new CharDeleteFail(CharDeleteFail.CharDeleteFailReason.DeletionFailed));
                    return;
                }

                _client.DeleteCharacter(_charSlot);
            }
            else
            {
                player.SetCharDeleteTime();
                _characterCrudService.Update(player.ToContract());
            }

            _client.SendPacketAsync(new CharDeleteOk());
        }
    }
}