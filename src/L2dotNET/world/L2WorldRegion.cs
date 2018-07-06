﻿using System.Collections.Generic;
using System.Linq;
using L2dotNET.Models;
using L2dotNET.Models.Player;
using L2dotNET.Models.Zones;
using NLog;

namespace L2dotNET.World
{
    public class L2WorldRegion
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly Dictionary<int, L2Object> _objects = new Dictionary<int, L2Object>();

        private readonly List<L2WorldRegion> _surroundingRegions = new List<L2WorldRegion>();
        private readonly List<L2ZoneType> _zones = new List<L2ZoneType>();

        private readonly int _tileX;
        private readonly int _tileY;

        private bool _active;
        private int _playersCount;

        public L2WorldRegion(int x, int y)
        {
            _tileX = x;
            _tileY = y;
        }

        public string GetName()
        {
            return $"WorldRegion:{_tileX}_{_tileY}";
        }

        public List<L2Object> GetObjects()
        {
            return _objects.Values.ToList();
        }

        public void AddSurroundingRegion(L2WorldRegion region)
        {
            _surroundingRegions.Add(region);
        }

        public List<L2WorldRegion> GetSurroundingRegions()
        {
            return _surroundingRegions;
        }

        public List<L2ZoneType> GetZones()
        {
            return _zones;
        }

        public void AddZone(L2ZoneType zone)
        {
            _zones.Add(zone);
        }

        public void RemoveZone(L2ZoneType zone)
        {
            _zones.Remove(zone);
        }

        public void RevalidateZones(L2Character character)
        {
            //if (character.isTeleporting())
            //    return;

            _zones.ForEach(zone => zone.RevalidateInZone(character));
        }

        public void RemoveFromZones(L2Character character)
        {
            _zones.ForEach(zone => zone.RemoveCharacter(character));
        }

        public bool ContainsZone(int zoneId)
        {
            return _zones.Any(z => z.Id == zoneId);
        }

        //     public bool checkEffectRangeInsidePeaceZone(L2Skill skill, final int x, final int y, final int z)
        //     {
        //         int range = skill.getEffectRange();
        //         int up = y + range;
        //         int down = y - range;
        //         int left = x + range;
        //         int right = x - range;

        //         foreach (L2ZoneType e in _zones)
        //         {
        ////             if ((e is L2TownZone && ((L2TownZone)e).isPeaceZone()) || e instanceof L2DerbyTrackZone || e instanceof L2PeaceZone)
        ////{
        //             //if (e.isInsideZone(x, up, z))
        //             //    return false;

        //             //if (e.isInsideZone(x, down, z))
        //             //    return false;

        //             //if (e.isInsideZone(left, y, z))
        //             //    return false;

        //             //if (e.isInsideZone(right, y, z))
        //             //    return false;

        //             //if (e.isInsideZone(x, y, z))
        //             //    return false;
        //         //}
        //     }
        //   return true;
        //  }

        //public void onDeath(L2Character character)
        //{
        //    _zones.stream().filter(z->z.isCharacterInZone(character)).forEach(z->z.onDieInside(character));
        //}

        //public void onRevive(L2Character character)
        //{
        //    _zones.stream().filter(z->z.isCharacterInZone(character)).forEach(z->z.onReviveInside(character));
        //}

        public bool IsActive()
        {
            return _active;
        }

        public int GetPlayersCount()
        {
            return _playersCount;
        }

        /**
	     * Check if neighbors (including self) aren't inhabited.
	     * @return true if the above condition is met.
	     */

        public bool IsEmptyNeighborhood()
        {
            return _surroundingRegions.All(neighbor => neighbor.GetPlayersCount() == 0);
        }

        /**
	     * This function turns this region's AI on or off.
	     * @param value : if true, activate hp/mp regen and random animation. If false, clean aggro/attack list, set objects on IDLE and drop their AI tasks.
	     */

        public void SetActive(bool value)
        {
            _active = value;

            //if (!value)
            //{
            //    foreach (L2Object o in _objects.Values)
            //    {
            //        if (o is L2Attackable)
            //        {
            //            L2Attackable mob = (L2Attackable)o;

            //            // Set target to null and cancel Attack or Cast
            //            mob.setTarget(null);

            //            // Stop movement
            //            mob.stopMove(null);

            //            // Stop all active skills effects in progress on the L2Character
            //            mob.stopAllEffects();

            //            mob.getAggroList().clear();
            //            mob.getAttackByList().clear();

            //            // stop the ai tasks
            //            if (mob.hasAI())
            //            {
            //                mob.getAI().setIntention(CtrlIntention.IDLE);
            //                mob.getAI().stopAITask();
            //            }
            //        }
            //    }

            //else
            //{
            //        for (L2Object o : _objects.values())
            //        {
            //            if (o instanceof L2Attackable)
            //      ((L2Attackable)o).getStatus().startHpMpRegeneration();
            //     else if (o instanceof L2Npc)
            //      ((L2Npc)o).startRandomAnimationTimer();
            //    }
            //}
        }

        public void AddVisibleObject(L2Object obj)
        {
            if (obj == null)
                return;

            if (!_objects.ContainsKey(obj.ObjectId))
                _objects.Add(obj.ObjectId, obj);

            if (obj is L2Player)
                _playersCount += 1;
        }

        public void RemoveVisibleObject(L2Object obj)
        {
            if (obj == null)
                return;

            _objects.Remove(obj.ObjectId);

            if (obj is L2Player)
                _playersCount -= 1;
        }
    }
}