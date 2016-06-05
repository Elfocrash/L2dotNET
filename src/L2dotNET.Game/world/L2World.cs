using System;
using System.Collections.Generic;
using log4net;
using L2dotNET.GameService.model;
using L2dotNET.GameService.network;
using L2dotNET.GameService.network.l2send;

namespace L2dotNET.GameService.world
{
    public class L2World
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(L2World));

        private static volatile L2World instance;
        private static readonly object syncRoot = new object();

        public static int TILE_X_MIN = 16;
        public static int TILE_X_MAX = 26;
        public static int TILE_Y_MIN = 10;
        public static int TILE_Y_MAX = 25;

        public static int TILE_SIZE = 32768;
        public static int WORLD_X_MIN = (TILE_X_MIN - 20) * TILE_SIZE;
        public static int WORLD_X_MAX = (TILE_X_MAX - 19) * TILE_SIZE;
        public static int WORLD_Y_MIN = (TILE_Y_MIN - 18) * TILE_SIZE;
        public static int WORLD_Y_MAX = (TILE_Y_MAX - 17) * TILE_SIZE;

        private static readonly int REGION_SIZE = 4096;
        private static readonly int REGIONS_X = (WORLD_X_MAX - WORLD_X_MIN) / REGION_SIZE;
        private static readonly int REGIONS_Y = (WORLD_Y_MAX - WORLD_Y_MIN) / REGION_SIZE;
        private static readonly int REGION_X_OFFSET = Math.Abs(WORLD_X_MIN / REGION_SIZE);
        private static readonly int REGION_Y_OFFSET = Math.Abs(WORLD_Y_MIN / REGION_SIZE);

        private readonly SortedList<int, L2Player> _allPlayers = new SortedList<int, L2Player>();
        private readonly SortedList<int, L2Object> _allObjects = new SortedList<int, L2Object>();

        private static L2World world = new L2World();

        private L2WorldRegion[][] _worldRegions;

        private L2World() { }

        public static L2World Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new L2World();
                        }
                    }
                }

                return instance;
            }
        }

        public void Initialize()
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
                            if (ValidRegion(x + a, y + b))
                            {
                                _worldRegions[x + a][y + b].addSurroundingRegion(_worldRegions[x][y]);
                            }
                        }
                    }
                }
            }
            log.Info("L2World: WorldRegion grid (" + REGIONS_X + " by " + REGIONS_Y + ") is now setted up.");
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

        public void RemoveObjects(L2Object[] objects)
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

        public L2Object FindObject(int oID)
        {
            if (_allObjects.ContainsKey(oID))
                return _allObjects[oID];

            return null;
        }

        public void RemoveFromAllPlayers(L2Player cha)
        {
            lock (_allPlayers)
            {
                _allPlayers.Remove(cha.ObjID);
            }
        }

        public L2WorldRegion[][] FetAllWorldRegions()
        {
            return _worldRegions;
        }

        private bool ValidRegion(int x, int y)
        {
            return (x >= 0 && x <= REGIONS_X && y >= 0 && y <= REGIONS_Y);
        }

        public static int GetRegionX(int regionX)
        {
            return (regionX - REGION_X_OFFSET) * REGION_SIZE;
        }

        public static int GetRegionY(int regionY)
        {
            return (regionY - REGION_Y_OFFSET) * REGION_SIZE;
        }

        public L2WorldRegion GetRegion(Location point)
        {
            return GetRegion(point.X, point.Y);
        }

        public L2WorldRegion GetRegion(int x, int y)
        {
            return _worldRegions[(x - WORLD_X_MIN) / REGION_SIZE][(y - WORLD_Y_MIN) / REGION_SIZE];
        }

        public L2WorldRegion[][] GetWorldRegions()
        {
            return _worldRegions;
        }

        public void RealiseEntry(L2Object obj, GameServerNetworkPacket pk, bool pkuse)
        {
            if (_allObjects.ContainsKey(obj.ObjID))
            {
                _allObjects.Add(obj.ObjID, obj);

                if (obj is L2Player)
                    _allPlayers.Add(obj.ObjID, (L2Player)obj);

                L2WorldRegion reg = GetRegion(obj.X, obj.Y);
                obj.CurrentRegion = reg.getName();
                if (reg != null)
                {
                    reg.realiseMe(obj, pk, pkuse);
                }
                else
                    log.Warn($"l2world: realiseEntry error, object on unk territory {obj.X} {obj.Y} {obj.Z}");
            }
        }

        public void UnrealiseEntry(L2Object obj, bool pkuse)
        {
            lock (_allObjects)
            {
                _allObjects.Remove(obj.ObjID);
            }

            if (obj is L2Player)
                lock (_allPlayers)
                    _allPlayers.Remove(obj.ObjID);

            L2WorldRegion reg = GetRegion(obj.X, obj.Y);
            if (reg != null)
            {
                reg.unrealiseMe(obj, pkuse);
            }
            else
                log.Warn($"l2world: unrealiseEntry error, object on unk territory {obj.X} {obj.Y} {obj.Z}");
        }

        public void GetKnowns(L2Object obj, int range, int height, bool zones)
        {
            L2WorldRegion reg = GetRegion(obj.X, obj.Y);
            obj.CurrentRegion = reg.getName();
            if (reg != null)
            {
                reg.showObjects(obj, true, range, height, true, zones);
            }
            else
                log.Warn($"l2world: unrealiseEntry error, object on unk territory {obj.X} {obj.Y} {obj.Z}");
        }

        public void CheckToUpdate(L2Object obj, int _x, int _y, int radius, int height, bool delLongest, bool zones)
        {
            L2WorldRegion reg = GetRegion(_x, _y);
            if (reg != null)
            {
                reg.showObjects(obj, true, radius, height, delLongest, zones);
            }
            else
                log.Warn($"l2world: unrealiseEntry error, object on unk territory {obj.X} {obj.Y} {obj.Z}");
        }

        public void BroadcastToRegion(int x, int y, GameServerNetworkPacket pck)
        {
            L2WorldRegion reg = GetRegion(x, y);
            if (reg != null)
            {
                reg.broadcastPacket(pck, false);
            }
            else
                log.Warn($"l2world: broadcastRegionPacket error, object on unk territory {x} {y}");
        }

        public IList<L2Object> GetAllObjects()
        {
            return _allObjects.Values;
        }

        public L2Player GetPlayer(string _target)
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

        public L2Player GetPlayer(int objId)
        {
            lock (_allPlayers)
            {
                foreach (L2Player pl in _allPlayers.Values)
                {
                    if (pl.ObjID.Equals(objId))
                        return pl;
                }
            }

            return null;
        }

        public IEnumerable<L2Player> GetAllPlayers()
        {
            return _allPlayers.Values;
        }

        public short GetPlayerCount()
        {
            return (short)_allPlayers.Count;
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