using System.Collections.Generic;
using log4net;
using L2dotNET.GameService.network;

namespace L2dotNET.GameService.world
{
    public class L2WorldRegion
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(L2WorldRegion));

        public List<L2WorldRegion> _surroundingRegions;
        private int _tileX, _tileY;
        public L2WorldRegion(int pTileX, int pTileY)
        {
            _surroundingRegions = new List<L2WorldRegion>();
            _tileX = pTileX;
            _tileY = pTileY;
        }

        public void addSurroundingRegion(L2WorldRegion reg)
        {
            _surroundingRegions.Add(reg);
        }

        public IEnumerable<L2WorldRegion> getSurroundingRegions()
        {
            return null;
        }

        public string getName()
        {
            return "WorldRegion:"+_tileX + "_" + _tileY;
        }

        public SortedDictionary<int, L2Object> _objects = new SortedDictionary<int, L2Object>();

        public void realiseMe(L2Object obj, GameServerNetworkPacket pk, bool pkuse)
        {
            if (_objects.ContainsKey(obj.ObjID))
            {                
                log.Info($"{ getName() } error, object { obj.ObjID } already in here.");
                return;
            }

            _objects.Add(obj.ObjID, obj);

            foreach (L2Object ko in _objects.Values)
            {
                if (ko.ObjID == obj.ObjID)
                    continue;

                ko.addKnownObject(obj, pk, pkuse);
                obj.addKnownObject(ko, pk, pkuse);
            }
        }

        public void unrealiseMe(L2Object obj, bool pkuse)
        {
            foreach (L2Object ko in _objects.Values)
            {
                if (ko.ObjID == obj.ObjID)
                    continue;

                ko.removeKnownObject(obj, true);
            }

            lock (_objects)
            {
                _objects.Remove(obj.ObjID);
            }
        }

        public void checkZones(L2Object obj, bool main)
        {

            if (main)
                foreach (L2WorldRegion wrn in _surroundingRegions)
                    wrn.checkZones(obj, false);
        }

        public void showObjects(L2Object obj, bool main, int radius, int height, bool delLongest, bool zones)
        {
            int x = obj.X;
            int y = obj.Y;
            int z = obj.Z;
            int sqRadius = radius * radius;
            foreach (L2Object ko in _objects.Values)
            {
                if (ko.ObjID == obj.ObjID)
                    continue;

                int x1 = ko.X;
                int y1 = ko.Y;
                int z1 = ko.Z;
                double dx = x1 - x;
                double dy = y1 - y;

                if (dx * dx + dy * dy < sqRadius && (z1 - height < z || z1 + height > z))
                {
                    ko.revalidate(obj);
                    obj.revalidate(ko);
                }
                else
                {
                    ko.removeKnownObject(obj, true);
                    obj.removeKnownObject(ko, true);
                }
            }
                
            if (main)
                foreach (L2WorldRegion wrn in _surroundingRegions)
                    wrn.showObjects(obj, false, radius, height, delLongest, zones);
        }

        public void broadcastPacket(GameServerNetworkPacket pck, bool main)
        {
            foreach (L2Object o in _objects.Values)
            {
                if (o is L2Player)
                {
                    o.sendPacket(pck);
                }

                if (main)
                    foreach (L2WorldRegion wrn in _surroundingRegions)
                        wrn.broadcastPacket(pck, false);
            }
        }
    }
}
