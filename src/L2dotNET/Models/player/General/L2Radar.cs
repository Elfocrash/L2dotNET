using System.Collections.Generic;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.model.player.General
{
    public class L2Radar
    {
        private readonly L2Player _player;
        private readonly List<RadarMarker> _markers;

        public L2Radar(L2Player player)
        {
            _player = player;
            _markers = new List<RadarMarker>();
        }

        public void AddMarker(int x, int y, int z)
        {
            RadarMarker newMarker = new RadarMarker(x, y, z);

            _markers.Add(newMarker);
            _player.SendPacket(new RadarControl(2, 2, x, y, z));
            _player.SendPacket(new RadarControl(0, 1, x, y, z));
        }

        public void RemoveMarker(int x, int y, int z)
        {
            RadarMarker newMarker = new RadarMarker(x, y, z);

            _markers.Remove(newMarker);
            _player.SendPacket(new RadarControl(1, 1, x, y, z));
        }

        public void RemoveAllMarkers()
        {
            _markers.ForEach(tempMarker => _player.SendPacket(new RadarControl(2, 2, tempMarker._x, tempMarker._y, tempMarker._z)));
            _markers.Clear();
        }

        public void loadMarkers()
        {
            _player.SendPacket(new RadarControl(2, 2, _player.X, _player.Y, _player.Z));
            _markers.ForEach(tempMarker => _player.SendPacket(new RadarControl(0, 1, tempMarker._x, tempMarker._y, tempMarker._z)));
        }
    }
}