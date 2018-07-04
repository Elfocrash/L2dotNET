using System;
using System.Threading.Tasks;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets
{
    class RequestRestart : PacketBase
    {
        private readonly GameClient _client;

        public RequestRestart(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                L2Player player = _client.CurrentPlayer;

                if (player == null)
                    return;

                if (player.PBlockAct == 1)
                {
                    player.SendActionFailedAsync();
                    return;
                }

                if (player.isInCombat())
                {
                    player.SendSystemMessage(SystemMessage.SystemMessageId.CantRestartWhileFighting);
                    player.SendActionFailedAsync();
                    return;
                }

                player.Online = 0;
                player.DeleteMe();
                player.SendPacketAsync(new RestartResponse());

                CharacterSelectionInfo csl = new CharacterSelectionInfo(_client.AccountName, _client.AccountCharacters, _client.SessionKey.PlayOkId1)
                {
                    CharId = player.ObjId
                };
                player.SendPacketAsync(csl);
            });
        }
    }
}