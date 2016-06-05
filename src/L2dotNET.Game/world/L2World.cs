using System;
using System.Collections.Generic;
using L2dotNET.GameService.network.l2send;
using L2dotNET.GameService.network;
using System.Linq;
using L2dotNET.GameService.model;
using log4net;
using L2dotNET.GameService.model.npcs;
using L2dotNET.GameService.tables;
using System.Collections.ObjectModel;
using L2dotNET.Models;

namespace L2dotNET.GameService.world
{
    public class L2World
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(L2World));

        private static volatile L2World instance;
        private static object syncRoot = new object();

        // Geodata min/max tiles
        public static int TILE_X_MIN = 16;
        public static int TILE_X_MAX = 26;
        public static int TILE_Y_MIN = 10;
        public static int TILE_Y_MAX = 25;

        // Map dimensions
        public static int TILE_SIZE = 32768;
        public static int WORLD_X_MIN = (TILE_X_MIN - 20) * TILE_SIZE;
        public static int WORLD_X_MAX = (TILE_X_MAX - 19) * TILE_SIZE;
        public static int WORLD_Y_MIN = (TILE_Y_MIN - 18) * TILE_SIZE;
        public static int WORLD_Y_MAX = (TILE_Y_MAX - 17) * TILE_SIZE;

        // Regions and offsets
        private static int REGION_SIZE = 4096;
        private static int REGIONS_X = (WORLD_X_MAX - WORLD_X_MIN) / REGION_SIZE;
        private static int REGIONS_Y = (WORLD_Y_MAX - WORLD_Y_MIN) / REGION_SIZE;
        private static int REGION_X_OFFSET = Math.Abs(WORLD_X_MIN / REGION_SIZE);
        private static int REGION_Y_OFFSET = Math.Abs(WORLD_Y_MIN / REGION_SIZE);

        private Dictionary<int, L2Object> _objects = new Dictionary<int, L2Object>();
        private Dictionary<int, L2Player> _players = new Dictionary<int, L2Player>();

        private L2WorldRegion[,] _worldRegions = new L2WorldRegion[REGIONS_X + 1,REGIONS_Y + 1];
	
        private L2World()
        {

        }

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
            for (int i = 0; i <= REGIONS_X; i++)
            {
                for (int j = 0; j <= REGIONS_Y; j++)
                    _worldRegions[i,j] = new L2WorldRegion(i, j);
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
                                _worldRegions[x + a,y + b].AddSurroundingRegion(_worldRegions[x,y]);
                        }
                    }
                }
            }
            log.Info("L2World: WorldRegion grid (" + REGIONS_X + " by " + REGIONS_Y + ") is now setted up.");
        }

        public void AddObject(L2Object obj)
        {
            if(!_objects.ContainsKey(obj.ObjID))
                _objects.Add(obj.ObjID, obj);
        }

        public void RemoveObject(L2Object obj)
        {
            _objects.Remove(obj.ObjID);
        }

        public List<L2Object> getObjects()
        {
            return _objects.Values.ToList();
        }

        public L2Object GetObject(int objectId)
        {
            return _objects[objectId];
        }

        public void AddPlayer(L2Player cha)
        {
            if(!_players.ContainsKey(cha.ObjID))
                _players.Add(cha.ObjID, cha);
        }

        public void removePlayer(L2Player cha)
        {
            _players.Remove(cha.ObjID);
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

        public static int getRegionX(int regionX)
        {
            return (regionX - REGION_X_OFFSET) * REGION_SIZE;
        }

        public static int getRegionY(int regionY)
        {
            return (regionY - REGION_Y_OFFSET) * REGION_SIZE;
        }

        private static bool validRegion(int x, int y)
        {
            return (x >= 0 && x <= REGIONS_X && y >= 0 && y <= REGIONS_Y);
        }

        public L2WorldRegion GetRegion(Location point)
        {
            return GetRegion(point.X, point.Y);
        }

        public L2WorldRegion GetRegion(int x, int y)
        {
            return _worldRegions[(x - WORLD_X_MIN) / REGION_SIZE,(y - WORLD_Y_MIN) / REGION_SIZE];
        }

        public L2WorldRegion[,] GetWorldRegions()
        {
            return _worldRegions;
        }

        public void deleteVisibleNpcSpawns()
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
