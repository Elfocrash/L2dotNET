namespace L2dotNET.Models.player.General
{
    public class RadarMarker
    {
        public int _type,
                   _x,
                   _y,
                   _z;

        public RadarMarker(int type, int x, int y, int z)
        {
            _type = type;
            _x = x;
            _y = y;
            _z = z;
        }

        public RadarMarker(int x, int y, int z)
        {
            _type = 1;
            _x = x;
            _y = y;
            _z = z;
        }
    }
}