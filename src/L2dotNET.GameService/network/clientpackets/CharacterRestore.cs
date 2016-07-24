using System.Linq;
using log4net;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.Network;
using L2dotNET.Services.Contracts;
using Ninject;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class CharacterRestore : PacketBase
    {
        [Inject]
        public IPlayerService PlayerService => GameServer.Kernel.Get<IPlayerService>();

        private static readonly ILog Log = LogManager.GetLogger(typeof(CharacterDelete));

        private readonly GameClient _client;
        private readonly int _charSlot;

        public CharacterRestore(Packet packet, GameClient client)
        {
            _client = client;
            _charSlot = packet.ReadInt();
        }

        public override void RunImpl()
        {
            //if (!FloodProtectors.performAction(getClient(), Action.CHARACTER_SELECT))
            //    return;

            ValidateAndRestore();

            _client.SendPacket(new CharacterSelectionInfo(_client.AccountName, _client.AccountChars, _client.SessionId));
        }

        private void ValidateAndRestore()
        {
            L2Player player = _client.AccountChars.FirstOrDefault(filter => filter.CharSlot == _charSlot);

            if (player == null)
            {
                Log.Warn($"{_client.Address} tried to restore Character in slot {_charSlot} but no characters exits at that slot.");
                return;
            }

            PlayerService.MarkToRestoreChar(player.ObjId);
            player.DeleteTime = 0;
        }
    }
}