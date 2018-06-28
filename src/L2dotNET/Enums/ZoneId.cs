using System;
using System.Collections.Generic;
using L2dotNET.DataContracts.Shared.Enums;

namespace L2dotNET.Enums
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
                yield return Pvp;
                yield return Peace;
                yield return Siege;
                yield return MotherTree;
                yield return ClanHall;
                yield return NoLanding;
                yield return Water;
                yield return Jail;
                yield return MonsterTrack;
                yield return Castle;
                yield return Swamp;
                yield return NoSummonFriend;
                yield return NoStore;
                yield return Town;
                yield return Hq;
                yield return DangerArea;
                yield return CastOnArtifact;
                yield return NoRestart;
                yield return Script;
            }
        }

        public static readonly ZoneId Pvp = new ZoneId(ZoneIds.Pvp);
        public static readonly ZoneId Peace = new ZoneId(ZoneIds.Peace);
        public static readonly ZoneId Siege = new ZoneId(ZoneIds.Siege);
        public static readonly ZoneId MotherTree = new ZoneId(ZoneIds.MotherTree);
        public static readonly ZoneId ClanHall = new ZoneId(ZoneIds.ClanHall);
        public static readonly ZoneId NoLanding = new ZoneId(ZoneIds.NoLanding);
        public static readonly ZoneId Water = new ZoneId(ZoneIds.Water);
        public static readonly ZoneId Jail = new ZoneId(ZoneIds.Jail);
        public static readonly ZoneId MonsterTrack = new ZoneId(ZoneIds.MonsterTrack);
        public static readonly ZoneId Castle = new ZoneId(ZoneIds.Castle);
        public static readonly ZoneId Swamp = new ZoneId(ZoneIds.Swamp);
        public static readonly ZoneId NoSummonFriend = new ZoneId(ZoneIds.NoSummonFriend);
        public static readonly ZoneId NoStore = new ZoneId(ZoneIds.NoStore);
        public static readonly ZoneId Town = new ZoneId(ZoneIds.Town);
        public static readonly ZoneId Hq = new ZoneId(ZoneIds.Hq);
        public static readonly ZoneId DangerArea = new ZoneId(ZoneIds.DangerArea);
        public static readonly ZoneId CastOnArtifact = new ZoneId(ZoneIds.CastOnArtifact);
        public static readonly ZoneId NoRestart = new ZoneId(ZoneIds.NoRestart);
        public static readonly ZoneId Script = new ZoneId(ZoneIds.Script);
    }
}