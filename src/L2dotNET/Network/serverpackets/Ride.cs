using L2dotNET.Models.Player;

namespace L2dotNET.Network.serverpackets
{
    class Ride : GameserverPacket
    {
        private readonly int _id;
        private readonly int _bRide;
        private readonly int _rideType;
        private readonly int _npcId;
        private int _x;
        private int _y;
        private int _z;

        public Ride(L2Player player, bool mount, int npc = 0)
        {
            _id = player.ObjId;
            _bRide = mount ? 1 : 0;
            _npcId = npc + 1000000;
            _x = player.X;
            _y = player.Y;
            _z = player.Z;

            switch (npc)
            {
                case 0: // dismount
                    _rideType = 0;
                    break;
                case 12526: // Wind
                case 12527: // Star
                case 12528: // Twilight
                case 16038: // red strider of wind
                case 16039: // red strider of star
                case 16040: // red strider of dusk
                    _rideType = 1;
                    break;
                case 12621: // Wyvern
                    _rideType = 2;
                    break;
                case 16037: // Great Snow Wolf
                case 16041: // Fenrir Wolf
                case 16042: // White Fenrir Wolf
                    _rideType = 3;
                    break;
                case 13130: // Light Purple Maned Horse
                case 13146: // Tawny-Maned Lion
                case 13147: // Steam Sledge
                    _rideType = 4;
                    break;
            }

            player.MountType = _rideType;
        }

        public override void Write()
        {
            WriteByte(0x86);
            WriteInt(_id);
            WriteInt(_bRide);
            WriteInt(_rideType);
            WriteInt(_npcId);
            //writeD(x);
            //writeD(y);
            //writeD(z);
        }
    }
}