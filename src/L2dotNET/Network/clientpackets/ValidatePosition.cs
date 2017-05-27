using log4net;
using L2dotNET.model.player;

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

        public ValidatePosition(Packet packet, GameClient client)
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
            //string prevReg = player.CurrentRegion;

            //int realX = player.X;
            //int realY = player.Y;
            ////int realZ = player.Z;

            //int dx = _x - realX;
            //int dy = _y - realY;
            ////int dz = _z - realZ;
            //double diffSq = (dx * dx) + (dy * dy);

            //if (diffSq < 360000)
            //{
            //    if (!player.IsMoving())
            //    {
            //        if (diffSq < 2500)
            //        {
            //            player.X = realX;
            //            player.Y = realY;
            //            player.Z = _z;
            //        }
            //        else
            //        {
            //            player.X = _x;
            //            player.Y = _y;
            //            player.Z = _z;
            //        }
            //    }
            //}
            //else
            //{
            //    player.X = _x;
            //    player.Y = _y;
            //    player.Z = _z;
            //    player.Heading = _heading;
            //}

            //Log.Info($"Current client position: X:{_x}, Y:{_y}, Z:{_z}"); //debug
            player.BroadcastUserInfo();

            //player.validateVisibleObjects(_x, _y, true);
        }
    }
}