using System;
using System.Threading.Tasks;
using L2dotNET.World;
using L2dotNET.Models;
using L2dotNET.Models.Player;
using L2dotNET.Utility;
using NLog;

namespace L2dotNET.Network.clientpackets
{
    class RequestAction : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _objectId;
        private readonly int _x;
        private readonly int _y;
        private readonly int _z;
        private readonly int _actionId;

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public RequestAction(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _objectId = packet.ReadInt();
            _x = packet.ReadInt();
            _y = packet.ReadInt();
            _z = packet.ReadInt();
            _actionId = packet.ReadByte(); // Action identifier : 0-Simple click, 1-Shift click
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                L2Player player = _client.CurrentPlayer;
                L2Object obj = null;

                if (_objectId == player.ObjId)
                    obj = player;
                else
                {
                    if (L2World.GetObject(_objectId) != null)
                        obj = L2World.GetObject(_objectId);
                }
                //fixed nullreference exception when obj is null
                Log.Debug($"Action Requested with { Utilz.GetTypeLower(obj).ToString() }  of ID : { _objectId.ToString()}");

                if(obj==null)
                {
                    Log.Debug("Action Requested Failed");
                    player.SendActionFailedAsync();
                    return;
                }

                switch (_actionId)
                {
                    case 0:
                        obj.OnActionAsync(player);
                        break;
                    case 1:
                        obj.OnActionShiftAsync(player);
                        break;
                    default:
                        player.SendActionFailedAsync();
                        break;
                }
            });
        }
    }
}