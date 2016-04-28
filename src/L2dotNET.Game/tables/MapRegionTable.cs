using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.IO;
using System;
using System.Xml;
using L2dotNET.Game.model;
using L2dotNET.Game.world;
using L2dotNET.Game.model.zones.Type;
using L2dotNET.Game.Enums;
using log4net;

namespace L2dotNET.Game.tables
{
    class MapRegionTable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MapRegionTable));
        private static volatile MapRegionTable instance;
        private static object syncRoot = new object();

        private static int REGIONS_X = 11;
        private static int REGIONS_Y = 16;

        private static int[,] _regions = new int[REGIONS_X, REGIONS_Y];

        private static int[] _castleIdArray =
        { 0, 0, 0, 0, 0, 1, 0, 2, 3, 4, 5, 0, 0, 6, 8, 7, 9, 0, 0 };

        public MapRegionTable()
        {

        }

        public static MapRegionTable Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new MapRegionTable();
                        }
                    }
                }

                return instance;
            }
        }

        public void Initialize()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"data\xml\map_region.xml");
            XmlNodeList nodes = doc.DocumentElement.SelectNodes("/list/map");
            int count = 0;
            foreach (XmlNode node in nodes)
            {
                if (node.Attributes[0].OwnerElement.Name.Equals("map"))
                {
                    XmlNamedNodeMap attrs = node.Attributes;
                    int rY = Convert.ToInt32(attrs.GetNamedItem("geoY").Value) - 10;
                    for (int rX = 0; rX < REGIONS_X; rX++)
                    {
                        _regions[rX, rY] = Convert.ToInt32(attrs.GetNamedItem("geoX_" + (rX + 16)).Value);
                        count++;
                    }
                }
            }
            log.Info($"MapRegionTable: Loaded {count} regions.");
        }

        public static int GetMapRegion(int posX, int posY)
        {
            try
            {
                return _regions[GetMapRegionX(posX), GetMapRegionY(posY)];
            }
            catch (Exception e)
            {
                log.Error("Exception in GetMapRegion: " + e);
                return 0;
            }
        }

        public static int GetMapRegionX(int posX)
        {
            return (posX >> 15) + 4;
        }

        public static int GetMapRegionY(int posY)
        {
            return (posY >> 15) + 8;
        }

        public static int GetAreaCastle(int x, int y)
        {
            switch (GetMapRegion(x, y))
            {
                case 0: // Talking Island Village
                case 5: // Town of Gludio
                case 6: // Gludin Village
                    return 1;

                case 7: // Town of Dion
                    return 2;

                case 8: // Town of Giran
                case 12: // Giran Harbor
                    return 3;

                case 1: // Elven Village
                case 2: // Dark Elven Village
                case 9: // Town of Oren
                case 17: // Floran Village
                    return 4;

                case 10: // Town of Aden
                case 11: // Hunters Village
                default: // Town of Aden
                    return 5;

                case 13: // Heine
                    return 6;

                case 15: // Town of Goddard
                    return 7;

                case 14: // Rune Township
                case 18: // Primeval Isle Wharf
                    return 8;

                case 3: // Orc Village
                case 4: // Dwarven Village
                case 16: // Town of Schuttgart
                    return 9;
            }
        }

        public string GetClosestTownName(int x, int y)
        {
            return GetClosestTownName(GetMapRegion(x, y));
        }

        public string GetClosestTownName(int townId)
        {
            switch (townId)
            {
                case 0:
                    return "Talking Island Village";

                case 1:
                    return "Elven Village";

                case 2:
                    return "Dark Elven Village";

                case 3:
                    return "Orc Village";

                case 4:
                    return "Dwarven Village";

                case 5:
                    return "Town of Gludio";

                case 6:
                    return "Gludin Village";

                case 7:
                    return "Town of Dion";

                case 8:
                    return "Town of Giran";

                case 9:
                    return "Town of Oren";

                case 10:
                    return "Town of Aden";

                case 11:
                    return "Hunters Village";

                case 12:
                    return "Giran Harbor";

                case 13:
                    return "Heine";

                case 14:
                    return "Rune Township";

                case 15:
                    return "Town of Goddard";

                case 16:
                    return "Town of Schuttgart";

                case 17:
                    return "Floran Village";

                case 18:
                    return "Primeval Isle";

                default:
                    return "Town of Aden";
            }
        }

        public Location GetTeleToLocation(L2Character _player, TeleportWhereType teleportWhere)
        {
            if (_player is L2Player)
            {
                L2Player player = (L2Player)_player;
                return GetClosestTown(player.ClassId.ClassRace, player.X, player.Y).GetSpawnLoc();
            }

            return GetClosestTown(_player.X, _player.Y).GetSpawnLoc();
        }

        public static L2TownZone GetClosestTown(ClassRace race, int x, int y)
        {
            switch (GetMapRegion(x, y))
            {
                case 0: // TI
                    return GetTown(2);

                case 1:// Elven
                    return GetTown((race == ClassRace.DARK_ELF) ? 1 : 3);

                case 2:// DE
                    return GetTown((race == ClassRace.ELF) ? 3 : 1);

                case 3: // Orc
                    return GetTown(4);

                case 4:// Dwarven
                    return GetTown(6);

                case 5:// Gludio
                    return GetTown(7);

                case 6:// Gludin
                    return GetTown(5);

                case 7: // Dion
                    return GetTown(8);

                case 8: // Giran
                case 12: // Giran Harbor
                    return GetTown(9);

                case 9: // Oren
                    return GetTown(10);

                case 10: // Aden
                    return GetTown(12);

                case 11: // HV
                    return GetTown(11);

                case 13: // Heine
                    return GetTown(15);

                case 14: // Rune
                    return GetTown(14);

                case 15: // Goddard
                    return GetTown(13);

                case 16: // Schuttgart
                    return GetTown(17);

                case 17:// Floran
                    return GetTown(16);

                case 18:// Primeval Isle
                    return GetTown(19);
            }
            return GetTown(16); // Default to floran
        }

        public static L2TownZone GetClosestTown(int x, int y)
        {
            switch (GetMapRegion(x, y))
            {
                case 0: // TI
                    return GetTown(2);

                case 1:// Elven
                    return GetTown(3);

                case 2:// DE
                    return GetTown(1);

                case 3: // Orc
                    return GetTown(4);

                case 4:// Dwarven
                    return GetTown(6);

                case 5:// Gludio
                    return GetTown(7);

                case 6:// Gludin
                    return GetTown(5);

                case 7: // Dion
                    return GetTown(8);

                case 8: // Giran
                case 12: // Giran Harbor
                    return GetTown(9);

                case 9: // Oren
                    return GetTown(10);

                case 10: // Aden
                    return GetTown(12);

                case 11: // HV
                    return GetTown(11);

                case 13: // Heine
                    return GetTown(15);

                case 14: // Rune
                    return GetTown(14);

                case 15: // Goddard
                    return GetTown(13);

                case 16: // Schuttgart
                    return GetTown(17);

                case 17:// Floran
                    return GetTown(16);

                case 18:// Primeval Isle
                    return GetTown(19);
            }
            return GetTown(16); // Default to floran
        }

        public static L2TownZone GetSecondClosestTown(int x, int y)
        {
            switch (GetMapRegion(x, y))
            {
                case 0: // TI
                case 1: // Elven
                case 2: // DE
                case 5: // Gludio
                case 6: // Gludin
                    return GetTown(5);

                case 3: // Orc
                    return GetTown(4);

                case 4: // Dwarven
                case 16: // Schuttgart
                    return GetTown(6);

                case 7: // Dion
                    return GetTown(7);

                case 8: // Giran
                case 9: // Oren
                case 10:// Aden
                case 11: // HV
                    return GetTown(11);

                case 12: // Giran Harbour
                case 13: // Heine
                case 17:// Floran
                    return GetTown(16);

                case 14: // Rune
                    return GetTown(13);

                case 15: // Goddard
                    return GetTown(12);

                case 18: // Primeval Isle
                    return GetTown(19);
            }
            return GetTown(16); // Default to floran
        }

        public static int GetClosestLocation(int x, int y)
        {
            switch (GetMapRegion(x, y))
            {
                case 0: // TI
                    return 1;

                case 1: // Elven
                    return 4;

                case 2: // DE
                    return 3;

                case 3: // Orc
                case 4: // Dwarven
                case 16:// Schuttgart
                    return 9;

                case 5: // Gludio
                case 6: // Gludin
                    return 2;

                case 7: // Dion
                    return 5;

                case 8: // Giran
                case 12: // Giran Harbor
                    return 6;

                case 9: // Oren
                    return 10;

                case 10: // Aden
                    return 13;

                case 11: // HV
                    return 11;

                case 13: // Heine
                    return 12;

                case 14: // Rune
                    return 14;

                case 15: // Goddard
                    return 15;
            }
            return 0;
        }

        public static L2TownZone GetTown(int townId)
        {
            //foreach(L2TownZone zone in )
            return null;
        }
    }

    public enum TeleportWhereType
    {
        CASTLE,
        CLAN_HALL,
        SIEGE_FLAG,
        TOWN
    }
}
