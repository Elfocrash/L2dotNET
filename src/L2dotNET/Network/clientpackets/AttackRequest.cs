using System;
using System.Threading.Tasks;
using L2dotNET.Models;
using L2dotNET.Models.Player;
using L2dotNET.World;
using NLog;

namespace L2dotNET.Network.clientpackets
{
    class AttackRequest : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _objectId;
        private readonly int _originX;
        private readonly int _originY;
        private readonly int _originZ;
        private readonly int _attackId;

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public AttackRequest(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _objectId = packet.ReadInt();
            _originX = packet.ReadInt();
            _originY = packet.ReadInt();
            _originZ = packet.ReadInt();
            _attackId = packet.ReadByte(); // 0 for simple click   1 for shift-click
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                L2Player player = _client.CurrentPlayer;
                L2Object obj = null;

                if (player.PBlockAct == 1)
                {
                    player.SendActionFailedAsync();
                    return;
                }

                if (_objectId == player.ObjectId)
                    obj = player;
                else
                {
                    if (L2World.GetObject(_objectId) != null)
                        obj = L2World.GetObject(_objectId);
                }

                if (obj == null)
                {
                    player.SendActionFailedAsync();
                    return;
                }

                //if (obj is L2Npc)
                //{
                //    if (((L2Npc)obj).Template._can_be_attacked == 0)
                //    {
                //        player.sendSystemMessage(144);//That is the incorrect target.
                //        player.sendActionFailed();
                //        return;
                //    }
                //}

                obj.OnForcedAttackAsync(player);
            });
        }
    }
}