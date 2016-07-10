using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using L2dotNET.GameService.Model.Player;
using L2dotNET.Models;

namespace L2dotNET.GameService.World
{
    public class L2World
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(L2World));

        private static volatile L2World _instance;
        private static readonly object SyncRoot = new object();

        // Geodata min/max tiles
        public static int TileXMin = 16;
        public static int TileXMax = 26;
        public static int TileYMin = 10;
        public static int TileYMax = 25;

        // Map dimensions
        public static int TileSize = 32768;
        public static int WorldXMin = (TileXMin - 20) * TileSize;
        public static int WorldXMax = (TileXMax - 19) * TileSize;
        public static int WorldYMin = (TileYMin - 18) * TileSize;
        public static int WorldYMax = (TileYMax - 17) * TileSize;

        // Regions and offsets
        private const int RegionSize = 4096;
        private static readonly int RegionsX = (WorldXMax - WorldXMin) / RegionSize;
        private static readonly int RegionsY = (WorldYMax - WorldYMin) / RegionSize;
        private static readonly int RegionXOffset = Math.Abs(WorldXMin / RegionSize);
        private static readonly int RegionYOffset = Math.Abs(WorldYMin / RegionSize);

        private readonly Dictionary<int, L2Object> _objects = new Dictionary<int, L2Object>();
        private readonly Dictionary<int, L2Player> _players = new Dictionary<int, L2Player>();

        private readonly L2WorldRegion[,] _worldRegions = new L2WorldRegion[RegionsX + 1, RegionsY + 1];

        private L2World() { }

        public static L2World Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                lock (SyncRoot)
                {
                    if (_instance == null)
                    {
                        _instance = new L2World();
                    }
                }

                return _instance;
            }
        }

        public void Initialize()
        {
            for (int i = 0; i <= RegionsX; i++)
                for (int j = 0; j <= RegionsY; j++)
                {
                    _worldRegions[i, j] = new L2WorldRegion(i, j);
                }

            for (int x = 0; x <= RegionsX; x++)
                for (int y = 0; y <= RegionsY; y++)
                    for (int a = -1; a <= 1; a++)
                        for (int b = -1; b <= 1; b++)
                            if (ValidRegion(x + a, y + b))
                            {
                                _worldRegions[x + a, y + b].AddSurroundingRegion(_worldRegions[x, y]);
                            }

            Log.Info("L2World: WorldRegion grid (" + RegionsX + " by " + RegionsY + ") is now setted up.");
        }

        public void AddObject(L2Object obj)
        {
            if (!_objects.ContainsKey(obj.ObjId))
            {
                _objects.Add(obj.ObjId, obj);
            }
        }

        public void RemoveObject(L2Object obj)
        {
            _objects.Remove(obj.ObjId);
        }

        public List<L2Object> GetObjects()
        {
            return _objects.Values.ToList();
        }

        public L2Object GetObject(int objectId)
        {
            return _objects[objectId];
        }

        public void AddPlayer(L2Player cha)
        {
            if (!_players.ContainsKey(cha.ObjId))
            {
                _players.Add(cha.ObjId, cha);
            }
        }

        public void RemovePlayer(L2Player cha)
        {
            _players.Remove(cha.ObjId);
        }

        public List<L2Player> GetPlayers()
        {
            return _players.Values.ToList();
        }

        //public L2Player GetPlayer(string name)
        //{
        //    return _players.get(CharNameTable.getInstance().getPlayerObjectId(name));
        //}

        public L2Player GetPlayer(int objectId)
        {
            return _players[objectId];
        }

        public static int GetRegionX(int regionX)
        {
            return (regionX - RegionXOffset) * RegionSize;
        }

        public static int GetRegionY(int regionY)
        {
            return (regionY - RegionYOffset) * RegionSize;
        }

        private static bool ValidRegion(int x, int y)
        {
            return (x >= 0) && (x <= RegionsX) && (y >= 0) && (y <= RegionsY);
        }

        public L2WorldRegion GetRegion(Location point)
        {
            return GetRegion(point.X, point.Y);
        }

        public L2WorldRegion GetRegion(int x, int y)
        {
            return _worldRegions[(x - WorldXMin) / RegionSize, (y - WorldYMin) / RegionSize];
        }

        public L2WorldRegion[,] GetWorldRegions()
        {
            return _worldRegions;
        }

        public void DeleteVisibleNpcSpawns()
        {
            //_log.info("Deleting all visible NPCs.");
            //       for (int i = 0; i <= REGIONS_X; i++)
            //       {
            //           for (int j = 0; j <= REGIONS_Y; j++)
            //           {
            //               foreach (L2Object obj in _worldRegions[i,j].GetObjects())
            //               {
            //               if (obj is L2Npc)
            //{
            //                   ((L2Npc)obj).deleteMe();

            //                   L2Spawn spawn = ((L2Npc)obj).getSpawn();
            //                   if (spawn != null)
            //                   {
            //                       //spawn.setRespawnState(false);
            //                       //SpawnTable.getInstance().deleteSpawn(spawn, false);
            //                   }
            //               }
            //           }
            //       }
            //   }
            //   _log.info("All visibles NPCs are now deleted.");
        }
    }
}