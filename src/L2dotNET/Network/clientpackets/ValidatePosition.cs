using System;
using log4net;
using L2dotNET.Models.Player;
using L2dotNET.Models.Zones;
using L2dotNET.World;

namespace L2dotNET.Network.clientpackets
{
    class ValidatePosition : PacketBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ValidatePosition));

        private const int Synctype = 1;

        private readonly GameClient _client;
        private readonly int _x;
        private readonly int _y;
        private readonly int _z;
        private readonly int _heading;
        private readonly int _data;

        public ValidatePosition(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _x = packet.ReadInt();
            _y = packet.ReadInt();
            _z = packet.ReadInt();
            _heading = packet.ReadInt();
            _data = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;
            L2WorldRegion prevReg = player.Region;

            int realX = player.X;
            int realY = player.Y;
            int realZ = player.Z;

            int dx = _x - realX;
            int dy = _y - realY;
            int dz = _z - realZ;
            double diffSq = (dx * dx) + (dy * dy);

            if (diffSq < 360000)
            {
                if (!player.IsMoving())
                {
                    if (diffSq < 2500)
                    {
                        player.X = realX;
                        player.Y = realY;
                        player.Z = _z;
                    }
                    else
                    {
                        player.X = _x;
                        player.Y = _y;
                        player.Z = _z;
                    }
                }
            }
            else
            {
                player.X = realX;
                player.Y = realY;
                player.Z = _z;
                player.Heading = _heading;
            }
            L2WorldRegion NewRegion = L2World.Instance.GetRegion(new Location(player.X, player.Y, player.Z));
            if (prevReg != NewRegion)
            {
                player.SetRegion(NewRegion);
                player.SetupKnows();

                //Add objects from surrounding regions into knows, this is a hack to prevent 
                //objects from popping into view as soon as you enter a new region
                //TODO: Proper region transition
                player.SetupKnows(NewRegion);
                
            }
            //Log.Info($"Current client position: X:{_x}, Y:{_y}, Z:{_z}"); //debug
            player.BroadcastUserInfo();
            player.ValidateVisibleObjects(_x, _y, true);
        }
    }
}