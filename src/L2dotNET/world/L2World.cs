using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using L2dotNET.Models;
using L2dotNET.Models.Player;
using L2dotNET.Models.Zones;
using NLog;

namespace L2dotNET.World
{
    public static class L2World
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        // Geodata min/max tiles
        public const int TileXMin = 16;
        public const int TileXMax = 26;
        public const int TileYMin = 10;
        public const int TileYMax = 25;

        // Map dimensions
        public const int TileSize = 32768;

        public const int WorldXMin = (TileXMin - 20) * TileSize;
        public const int WorldXMax = (TileXMax - 19) * TileSize;
        public const int WorldYMin = (TileYMin - 18) * TileSize;
        public const int WorldYMax = (TileYMax - 17) * TileSize;

        // Regions and offsets
        private const int RegionSize = 4096;
        private static readonly int RegionsX = (WorldXMax - WorldXMin) / RegionSize;
        private static readonly int RegionsY = (WorldYMax - WorldYMin) / RegionSize;
        private static readonly int RegionXOffset = Math.Abs(WorldXMin / RegionSize);
        private static readonly int RegionYOffset = Math.Abs(WorldYMin / RegionSize);

        private static readonly ConcurrentDictionary<int, L2Object> _objects = new ConcurrentDictionary<int, L2Object>();
        private static readonly ConcurrentDictionary<int, L2Player> _players = new ConcurrentDictionary<int, L2Player>();

        private static readonly L2WorldRegion[,] _worldRegions = new L2WorldRegion[RegionsX + 1, RegionsY + 1];

        public static void Initialize()
        {
            for (int i = 0; i <= RegionsX; i++)
                for (int j = 0; j <= RegionsY; j++)
                    _worldRegions[i, j] = new L2WorldRegion(i, j);

            for (int x = 0; x <= RegionsX; x++)
                for (int y = 0; y <= RegionsY; y++)
                    for (int a = -1; a <= 1; a++)
                        for (int b = -1; b <= 1; b++)
                            if (ValidRegion(x + a, y + b))
                            {
                                _worldRegions[x + a, y + b].AddSurroundingRegion(_worldRegions[x, y]);
                            }

            Log.Info($"WorldRegion grid ({RegionsX} by {RegionsY}) is now setted up.");
        }

        public static void AddObject(L2Object obj)
        {
            if (!_objects.ContainsKey(obj.ObjectId))
            {
                _objects.TryAdd(obj.ObjectId, obj);
            }
        }

        public static void RemoveObject(L2Object obj)
        {
            L2Object o;
            _objects.TryRemove(obj.ObjectId, out o);
        }

        public static List<L2Object> GetObjects()
        {
            return _objects.Values.ToList();
        }

        public static L2Object GetObject(int objectId)
        {
            if (_objects.ContainsKey(objectId))
            {
                return _objects[objectId];
            }

            return null;
        }

        public static void AddPlayer(L2Player player)
        {
            if (!_players.ContainsKey(player.ObjectId))
            {
                _players.TryAdd(player.ObjectId, player);
            }
        }

        public static void RemovePlayer(L2Player player)
        {
            L2Player o;
            _players.TryRemove(player.ObjectId, out o);
        }

        public static List<L2Player> GetPlayers()
        {
            return _players.Values.ToList();
        }

        public static L2Player GetPlayer(int objectId)
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

        public static L2WorldRegion GetRegion(Location point)
        {
            return GetRegion(point.X, point.Y);
        }

        public static L2WorldRegion GetRegion(int x, int y)
        {
            return _worldRegions[(x - WorldXMin) / RegionSize, (y - WorldYMin) / RegionSize];
        }

        public static L2WorldRegion[,] GetWorldRegions()
        {
            return _worldRegions;
        }
    }
}