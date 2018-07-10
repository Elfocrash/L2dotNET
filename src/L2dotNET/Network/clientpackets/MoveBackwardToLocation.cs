using System;
using System.Threading.Tasks;
using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets
{
    class MoveBackwardToLocation : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _targetX;
        private readonly int _targetY;
        private readonly int _targetZ;
        private readonly int _originX;
        private readonly int _originY;
        private readonly int _originZ;
        private readonly int _moveMovement;

        public MoveBackwardToLocation(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _targetX = packet.ReadInt();
            _targetY = packet.ReadInt();
            _targetZ = packet.ReadInt();
            _originX = packet.ReadInt();
            _originY = packet.ReadInt();
            _originZ = packet.ReadInt();
            try
            {
                _moveMovement = packet.ReadInt(); // is 0 if cursor keys are used  1 if mouse is used
            }
            catch
            {
                // ignore for now
            }
        }

        public override async Task RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            if (player.IsSittingInProgress() || player.IsSitting())
            {
                player.SendSystemMessage(SystemMessageId.CantMoveSitting);
                player.SendActionFailedAsync();
                return;
            }

            if (player.Boat != null)
            {
                player.SendMessageAsync("cant leave boat.");
                player.SendActionFailedAsync();
                return;
            }

            if (!player.CharMovement.CanMove())
            {
                player.SendActionFailedAsync();
                return;
            }

            // player.sendMessage($"can see: {GeoData.getInstance().canSeeCoord(player, _targetX, _targetY, _targetZ, true)}");

            player.Obsx = -1;

            //player.SendMessageAsync($"distance {Math.Floor(Math.Sqrt(dx * dx + dy * dy))}");
            //player.AiCharacter.StopAutoAttack();
            player.CharMovement.MoveTo(_targetX, _targetY, _targetZ);
        }
    }
}