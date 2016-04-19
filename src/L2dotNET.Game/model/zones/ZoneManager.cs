using System;
using System.Collections.Generic;
using L2dotNET.Game.world;

namespace L2dotNET.Game.model.zones
{
    public class ZoneManager
    {
        public SortedList<int, L2Zone> _zones = new SortedList<int, L2Zone>();

        public void addZone(L2Zone zone)
        {
            if (_zones.ContainsKey(zone.ZoneID))
                return;

            _zones.Add(zone.ZoneID, zone);
            zone.onInit();
        }

        public void remZone(L2Zone zone)
        {
            if (!_zones.ContainsKey(zone.ZoneID))
                return;

            _zones.Remove(zone.ZoneID);
            Console.WriteLine("zone "+zone.Name+" removed");
        }

        public void validate(L2Object obj, int _x, int _y, int _z)
        {
            foreach (L2Zone z in _zones.Values)
            {
                if (z.InstanceID != obj.InstanceID)
                    continue;

                if (z.Territory.isInsideZone(_x, _y, _z))
                    obj.onEnterZone(z);
                else
                    obj.onExitZone(z, true);
            }
        }
    }
}
