using System;
using System.Collections.Generic;

namespace L2dotNET.GameService.Enums
{
    public class ZoneId
    {
        public ZoneIds Id { get; }

        private ZoneId(ZoneIds zoneId)
        {
            Id = zoneId;
        }

        public static int GetZoneCount()
        {
            return Enum.GetNames(typeof(ZoneIds)).Length;
        }

        public static IEnumerable<ZoneId> Values
        {
            get
            {
                yield return PVP;
                yield return PEACE;
                yield return SIEGE;
                yield return MOTHER_TREE;
                yield return CLAN_HALL;
                yield return NO_LANDING;
                yield return WATER;
                yield return JAIL;
                yield return MONSTER_TRACK;
                yield return CASTLE;
                yield return SWAMP;
                yield return NO_SUMMON_FRIEND;
                yield return NO_STORE;
                yield return TOWN;
                yield return HQ;
                yield return DANGER_AREA;
                yield return CAST_ON_ARTIFACT;
                yield return NO_RESTART;
                yield return SCRIPT;
            }
        }

        public static readonly ZoneId PVP = new ZoneId(ZoneIds.PVP);
        public static readonly ZoneId PEACE = new ZoneId(ZoneIds.PEACE);
        public static readonly ZoneId SIEGE = new ZoneId(ZoneIds.SIEGE);
        public static readonly ZoneId MOTHER_TREE = new ZoneId(ZoneIds.MOTHER_TREE);
        public static readonly ZoneId CLAN_HALL = new ZoneId(ZoneIds.CLAN_HALL);
        public static readonly ZoneId NO_LANDING = new ZoneId(ZoneIds.NO_LANDING);
        public static readonly ZoneId WATER = new ZoneId(ZoneIds.WATER);
        public static readonly ZoneId JAIL = new ZoneId(ZoneIds.JAIL);
        public static readonly ZoneId MONSTER_TRACK = new ZoneId(ZoneIds.MONSTER_TRACK);
        public static readonly ZoneId CASTLE = new ZoneId(ZoneIds.CASTLE);
        public static readonly ZoneId SWAMP = new ZoneId(ZoneIds.SWAMP);
        public static readonly ZoneId NO_SUMMON_FRIEND = new ZoneId(ZoneIds.NO_SUMMON_FRIEND);
        public static readonly ZoneId NO_STORE = new ZoneId(ZoneIds.NO_STORE);
        public static readonly ZoneId TOWN = new ZoneId(ZoneIds.TOWN);
        public static readonly ZoneId HQ = new ZoneId(ZoneIds.HQ);
        public static readonly ZoneId DANGER_AREA = new ZoneId(ZoneIds.DANGER_AREA);
        public static readonly ZoneId CAST_ON_ARTIFACT = new ZoneId(ZoneIds.CAST_ON_ARTIFACT);
        public static readonly ZoneId NO_RESTART = new ZoneId(ZoneIds.NO_RESTART);
        public static readonly ZoneId SCRIPT = new ZoneId(ZoneIds.SCRIPT);
    }
}