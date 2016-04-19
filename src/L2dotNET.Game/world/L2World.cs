using System;
using System.Collections.Generic;
using L2dotNET.Game.logger;
using L2dotNET.Game.world.instances;
using L2dotNET.Game.network.l2send;
using L2dotNET.Game.network;

namespace L2dotNET.Game.world
{
    public class L2World
    {
        public static int GRACIA_MAX_X = -166168;
        public static int GRACIA_MAX_Z = 6105;
        public static int GRACIA_MIN_Z = -895;

        /*
        * biteshift, defines number of regions
        * note, shifting by 15 will result in regions corresponding to map tiles
        * shifting by 12 divides one tile to 8x8 regions
        */
        public static int SHIFT_BY = 12;

        public static int MAP_MIN_X = -327680;
        public static int MAP_MAX_X = 229376;
        public static int MAP_MIN_Y = -262144;
        public static int MAP_MAX_Y = 294912;

        public static int OFFSET_X = Math.Abs(MAP_MIN_X >> SHIFT_BY);
        public static int OFFSET_Y = Math.Abs(MAP_MIN_Y >> SHIFT_BY);

        private static int REGIONS_X = (MAP_MAX_X >> SHIFT_BY) + OFFSET_X;
        private static int REGIONS_Y = (MAP_MAX_Y >> SHIFT_BY) + OFFSET_Y;

        private SortedList<int, L2Player> _allPlayers = new SortedList<int, L2Player>();
        private SortedList<int, L2Object> _allObjects = new SortedList<int, L2Object>();
        private SortedList<int, L2Instance> _instances = new SortedList<int, L2Instance>();

        private static L2World world = new L2World();

        private L2WorldRegion[][] _worldRegions;

        private L2World()
        {
            initRegions();
        }

        public static L2World getInstance()
        {
            return world;
        }

        public void removeObjects(List<L2Object> list)
        {
            lock (_allObjects)
            {
                foreach (L2Object o in list)
                {
                    _allObjects.Remove(o.ObjID);

                    if (o is L2Player)
                    {
                        lock (_allPlayers)
                        {
                            _allPlayers.Remove(o.ObjID);
                        }
                    }
                }
            }
        }

        public void removeObjects(L2Object[] objects)
        {
            lock (_allObjects)
            {
                foreach (L2Object o in objects)
                {
                    _allObjects.Remove(o.ObjID);

                    if (o is L2Player)
                    {
                        lock (_allPlayers)
                        {
                            _allPlayers.Remove(o.ObjID);
                        }

                    }
                }
            }
        }

        public L2Object findObject(int oID)
        {
            if (_allObjects.ContainsKey(oID))
                return _allObjects[oID];

            return null;
        }

        public void removeFromAllPlayers(L2Player cha)
        {
            lock (_allPlayers)
            {
                _allPlayers.Remove(cha.ObjID);
            }
        }

        public L2WorldRegion[][] getAllWorldRegions()
        {
            return _worldRegions;
        }

        private bool validRegion(int x, int y)
        {
            return (x >= 0 && x <= REGIONS_X && y >= 0 && y <= REGIONS_Y);
        }

        private void initRegions()
        {
            _worldRegions = new L2WorldRegion[REGIONS_X + 1][];

            for (int i = 0; i <= REGIONS_X; i++)
            {
                _worldRegions[i] = new L2WorldRegion[REGIONS_Y + 1];
                for (int j = 0; j <= REGIONS_Y; j++)
                {
                    _worldRegions[i][j] = new L2WorldRegion(i, j);
                }
            }

            for (int x = 0; x <= REGIONS_X; x++)
            {
                for (int y = 0; y <= REGIONS_Y; y++)
                {
                    for (int a = -1; a <= 1; a++)
                    {
                        for (int b = -1; b <= 1; b++)
                        {
                            if (validRegion(x + a, y + b))
                            {
                                _worldRegions[x + a][y + b].addSurroundingRegion(_worldRegions[x][y]);
                            }
                        }
                    }
                }
            }

            CLogger.info("l2world: " + REGIONS_X * REGIONS_Y + " regions done.");
        }

        public L2WorldRegion getRegion(int x, int y)
        {
            L2WorldRegion reg = _worldRegions[(x >> SHIFT_BY) + OFFSET_X][(y >> SHIFT_BY) + OFFSET_Y];
            return reg;
        }

        public void realiseEntry(L2Object obj, GameServerNetworkPacket pk, bool pkuse)
        {
            _allObjects.Add(obj.ObjID, obj);

            if (obj is L2Player)
                _allPlayers.Add(obj.ObjID, (L2Player)obj);

            L2WorldRegion reg = getRegion(obj.X, obj.Y);
            obj.CurrentRegion = reg.getName();
            if (reg != null)
            {
                reg.realiseMe(obj, pk, pkuse);
            }
            else
                CLogger.warning("l2world: realiseEntry error, object on unk territory " + obj.X + " " + obj.Y + " " + obj.Z);
        }

        public void unrealiseEntry(L2Object obj, bool pkuse)
        {
            lock (_allObjects)
            {
                _allObjects.Remove(obj.ObjID);
            }

            if (obj is L2Player)
                lock (_allPlayers)
                    _allPlayers.Remove(obj.ObjID);

            L2WorldRegion reg = getRegion(obj.X, obj.Y);
            if (reg != null)
            {
                reg.unrealiseMe(obj, pkuse);
            }
            else
                CLogger.warning("l2world: unrealiseEntry error, object on unk territory " + obj.X + " " + obj.Y + " " + obj.Z);
        }

        public void getKnowns(L2Object obj, int range, int height, bool zones)
        {
            L2WorldRegion reg = getRegion(obj.X, obj.Y);
            obj.CurrentRegion = reg.getName();
            if (reg != null)
            {
                reg.showObjects(obj, true, range, height, true, zones);
            }
            else
                CLogger.warning("l2world: unrealiseEntry error, object on unk territory " + obj.X + " " + obj.Y + " " + obj.Z);
        }

        public void checkToUpdate(L2Object obj, int _x, int _y, int radius, int height, bool delLongest, bool zones)
        {
            L2WorldRegion reg = getRegion(_x, _y);
            if (reg != null)
            {
                reg.showObjects(obj, true, radius, height, delLongest, zones);
            }
            else
                CLogger.warning("l2world: unrealiseEntry error, object on unk territory " + obj.X + " " + obj.Y + " " + obj.Z);
        }

        public void broadcastToRegion(int instanceId, int x, int y, GameServerNetworkPacket pck)
        {
            L2WorldRegion reg = getRegion(x, y);
            if (reg != null)
            {
                reg.broadcastPacket(instanceId, pck, false);
            }
            else
                CLogger.warning("l2world: broadcastRegionPacket error, object on unk territory " + x + " " + y);
        }

        public IList<L2Object> getAllObjects()
        {
            return _allObjects.Values;
        }

        public L2Player getPlayer(string _target)
        {
            lock (_allPlayers)
            {
                foreach (L2Player pl in _allPlayers.Values)
                {
                    if (pl.Name.Equals(_target))
                        return pl;
                }
            }

            return null;
        }

        public IEnumerable<L2Player> getAllPlayers()
        {
            return _allPlayers.Values;
        }

        public short getPlayerCount()
        {
            return (short)_allPlayers.Count;
        }

        public L2Instance getInstance(int id)
        {
            if (_instances.ContainsKey(id))
                return _instances[id];

            return null;
        }

        public void registerInstance(L2Instance instance)
        {
            _instances.Add(instance.ServerID, instance);
        }

        public void closeInstance(L2Instance instance)
        {
            if (_instances.ContainsKey(instance.ServerID))
                lock (_instances)
                    _instances.Remove(instance.ServerID);
        }

        public void KickAccount(string account)
        {
            foreach (L2Player pl in _allPlayers.Values)
            {
                if (pl.Gameclient.AccountName == account)
                {
                    pl.sendPacket(new LeaveWorld());
                    pl.Termination();
                    break;
                }
            }
        }
    }
}
