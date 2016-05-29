using System.Collections.Generic;
using System.Timers;
using L2dotNET.GameService.model.skills2;
using L2dotNET.GameService.model.zones;
using L2dotNET.GameService.model.zones.classes;
using L2dotNET.GameService.network.l2send;
using L2dotNET.GameService.network;

namespace L2dotNET.GameService.world
{
    public abstract class L2Object
    {
        public int ObjID;
        public SortedList<int, L2Object> knownObjects = new SortedList<int, L2Object>();
        public virtual byte Level { get; set; } = 1;
        public virtual double CurHP { get; set; }
        public virtual double CurMP { get; set; }
        public virtual double CurCP { get; set; }
        public virtual bool Dead { get; set; } = false;
        public virtual int X { get; set; }
        public virtual int Y { get; set; }
        public virtual int Z { get; set; }
        public virtual int DestX { get; set; }
        public virtual int DestY { get; set; }
        public virtual int DestZ { get; set; }
        public virtual int Heading { get; set; }
        public virtual int TeamID { get; set; }
        public virtual bool Visible { get; set; } = true;
        public virtual string CurrentRegion { get; set; }
        public byte ObjectSummonType = 0;

        public virtual void onAction(L2Player player) {}
        public virtual void onActionShift(L2Player player) { onAction(player); }
        public virtual void onForcedAttack(L2Player player) {}
        public virtual void sendPacket(GameServerNetworkPacket pk) { }
        public virtual void addAbnormal(TSkill skill, L2Character caster, bool permanent, bool unlim) { }
        public virtual void onRemObject(L2Object obj) { }
        public virtual void onAddObject(L2Object obj, GameServerNetworkPacket pk, string msg = null) { }
        public virtual void broadcastUserInfo() { }
        public virtual void NotifyAction(L2Player player) { }
        public virtual void StartAI() { }

        public virtual void onSpawn() 
        {
            broadcastUserInfo();
        }

        public virtual void broadcastPacket(GameServerNetworkPacket pk, bool excludeYourself)
        {
            if (!excludeYourself)
                sendPacket(pk);

            foreach (L2Object o in knownObjects.Values)
            {
                if (o is L2Player)
                    o.sendPacket(pk);
            }
        }

        public virtual void broadcastPacket(GameServerNetworkPacket pk)
        {
            broadcastPacket(pk, false);
        }

        public virtual void reduceHp(L2Character attacker, double damage) { }

        public void deleteMe()
        {
            foreach (L2Object o in knownObjects.Values)
            {
                if(o is L2Player)
                    o.sendPacket(new DeleteObject(ObjID));
            }

            StopRegeneration();

            L2World.Instance.RemoveObject(this);
        }

        public void clearKnowns(bool deleteMe, params int[] exclude)
        {
            foreach (L2Object o in knownObjects.Values)
            {
                o.onClearing(this, deleteMe);

                if (deleteMe && this is L2Player)
                {
                      sendPacket(new DeleteObject(o.ObjID));
                }
            }

            knownObjects.Clear();
        }

        public void getKnowns(int range, int height, bool zones)
        {
            L2World.Instance.getObjects();// GetKnowns(this, range, height, zones);
        }

        private void onClearing(L2Object target, bool deleteMe)
        {
            lock (knownObjects)
            {
                knownObjects.Remove(target.ObjID);
            }

            if (deleteMe && target is L2Player)
            {
                target.sendPacket(new DeleteObject(ObjID));
            }
        }
        
        public void setVisible(bool val)
        {
            Visible = val;
            foreach (L2Object o in knownObjects.Values)
            {
                o.canView(this);
            }
        }

        private void canView(L2Object target)
        {
            foreach (L2Object o in knownObjects.Values)
            {
                o.onClearing(this, true);
            }
        }

        public void addKnownObject(L2Object obj, GameServerNetworkPacket pk, bool pkuse)
        {
            if (knownObjects.ContainsKey(obj.ObjID))
                return;

            knownObjects.Add(obj.ObjID, obj);

            if (!obj.Visible)
                return;

            if(pkuse)
                onAddObject(obj, pk);
        }

        public void updateVisibleStatus()
        {
            foreach (L2Object o in knownObjects.Values)
            {
                if (o.Visible)
                    onAddObject(o, null);
            }
        }

        public void removeKnownObject(L2Object obj, bool update)
        {
            if(knownObjects.ContainsKey(obj.ObjID))
            {
                onRemObject(obj);

                lock (knownObjects)
                {
                    knownObjects.Remove(obj.ObjID);
                }
            }
        }

        public void revalidate(L2Object obj)
        {
            if (!knownObjects.ContainsKey(obj.ObjID))
            {
                knownObjects.Add(obj.ObjID, obj);

                if(obj.Visible)
                    onAddObject(obj, null);
            }
        }

        public bool isInsideRadius(L2Object o, int radius, bool checkZ, bool strictCheck)
        {
            return isInsideRadius(o.X, o.Y, o.Z, radius, checkZ, strictCheck);
        }

        public bool isInsideRadius(int x, int y, int radius, bool strictCheck)
        {
            return isInsideRadius(x, y, 0, radius, false, strictCheck);
        }

        public bool isInsideRadius(int x, int y, int z, int radius, bool checkZ, bool strictCheck)
        {
            double dx = x - X;
            double dy = y - Y;
            double dz = z - Z;

            if (strictCheck)
            {
                if (checkZ)
                    return (dx * dx + dy * dy + dz * dz) < radius * radius;
                else
                    return (dx * dx + dy * dy) < radius * radius;
            }
            else
            {
                if (checkZ)
                    return (dx * dx + dy * dy + dz * dz) <= radius * radius;
                else
                    return (dx * dx + dy * dy) <= radius * radius;
            }
        }

        public SortedList<int, L2Zone> _activeZones = new SortedList<int, L2Zone>();
        private bool 
            _isInsidePeaceZone = false,
            _isInsidePvpZone = false, 
            _isInsideWaterZone = false, 
            _isInsideSSQZone = false,
            _isInsideSiegeZone = false,
            _isInsideSomeDungeon = false;

        public bool isInDanger = false;

        public bool isInSiege()
        {
            return _isInsideSiegeZone;
        }

        public bool isInDungeon()
        {
            return _isInsideSomeDungeon;
        }

        public int lastCode = -1;
        private bool ForceSetPvp = false;
        public bool _isInCombat = false;

        public void setForcedPvpZone(bool val)
        {
            ForceSetPvp = val;
            validateZoneCompass();
            validateBattleZones();
        }

        public virtual void validateZoneCompass()
        {
            if (ForceSetPvp)
            {
                if (lastCode != ExSetCompassZoneCode.PVPZONE)
                {
                    lastCode = ExSetCompassZoneCode.PVPZONE;
                    sendPacket(new ExSetCompassZoneCode(ExSetCompassZoneCode.PVPZONE));
                    return;
                }
            }

            int code = 0;
            if (_isInsidePvpZone)
            {
                code = ExSetCompassZoneCode.PVPZONE;
            }
            else
            {
                if (_isInsidePeaceZone)
                    code = ExSetCompassZoneCode.PEACEZONE;
                else
                    code = ExSetCompassZoneCode.GENERALZONE; //обычн зона
            }

            if (code != 0)
            {
                if (lastCode != -1 && lastCode != code)
                {
                    lastCode = code;
                    sendPacket(new ExSetCompassZoneCode(code));
                }
                else
                {
                    lastCode = code;
                    sendPacket(new ExSetCompassZoneCode(code));
                }
            }
        }

        public void onEnterZone(L2Zone z)
        {
            if (_activeZones.ContainsKey(z.ZoneID))
                return;

            if (this is L2Player)
            {
                ((L2Player)this).sendMessage("entered zone "+z.Name);
            }

            _activeZones.Add(z.ZoneID, z);
            z.onEnter(this);

            revalidateZone(z);
            validateZoneCompass();
        }

        public void onExitZone(L2Zone z, bool cls)
        {
            if (!_activeZones.ContainsKey(z.ZoneID))
                return;

            lock (_activeZones)
            {
                _activeZones.Remove(z.ZoneID);
            }
            
            z.onExit(this, cls);

            revalidateZone(z);
            validateZoneCompass();
        }

        private void revalidateZone(L2Zone z)
        {
            if (z is peace_zone)
            {
                validatePeaceZones();
            }
            else if (z is battle_zone)
            {
                validateBattleZones();
            }
            else if (z is water)
            {
                validateWaterZones();
            }
        }
 
        public bool isInBattle()
        {
            return _isInsidePvpZone;
        }

        public bool isInPeace()
        {
            if (_isInsidePvpZone)
                return false;

            return _isInsidePeaceZone;
        }

        public bool isInWater()
        {
            return _isInsideWaterZone;
        }

        public bool isInCombat()
        {
            return _isInCombat;
        }

        public void validatePeaceZones()
        {
            bool found = false, old = _isInsidePeaceZone;
            foreach (L2Zone z in _activeZones.Values)
            {
                if (z is peace_zone)
                {
                    _isInsidePeaceZone = true;
                    found = true;
                    break;
                }
            }

            if (!found)
                _isInsidePeaceZone = false;

            if (!old && _isInsidePeaceZone)
            {
                if (this is L2Player)
                {
                    ((L2Player)this).sendSystemMessage(SystemMessage.SystemMessageId.ENTER_PEACEFUL_ZONE);
                }
            }
            else if (old && !_isInsidePeaceZone)
            {
                if (this is L2Player)
                {
                    ((L2Player)this).sendSystemMessage(SystemMessage.SystemMessageId.EXIT_PEACEFUL_ZONE);
                }
            }
        }

        public void validateBattleZones()
        {
            bool found = false, old = _isInsidePvpZone;
            if (!ForceSetPvp)
            {
                foreach (L2Zone z in _activeZones.Values)
                {
                    if (z is battle_zone)
                    {
                        _isInsidePvpZone = true;
                        found = true;
                        break;
                    }
                }
            }
            else
            {
                old = false;
                _isInsidePvpZone = true;
                found = true;
            }

            if (!found)
                _isInsidePvpZone = false;

            if (!old && _isInsidePvpZone)
            {
                if (this is L2Player)
                {
                    ((L2Player)this).sendSystemMessage(SystemMessage.SystemMessageId.ENTERED_COMBAT_ZONE);
                }
            }
            else if (old && !_isInsidePvpZone)
            {
                if (this is L2Player)
                {
                    ((L2Player)this).sendSystemMessage(SystemMessage.SystemMessageId.LEFT_COMBAT_ZONE);
                }
            }
        }

        public void validateWaterZones()
        {
            //bool found = false;
            //foreach (L2Zone z in _activeZones.Values)
            //{
            //    if (z is water)
            //    {
            //        _isInsideWaterZone = true;
            //        found = true;
            //        break;
            //    }
            //}

            //if(!found)

            _isInsideWaterZone = Z > -4779 && Z < -3779;

            if (this is L2Player)
            {
                ((L2Player)this).waterTimer();
            }
        }

        public void validateVisibleObjects(int x, int y, bool zones)
        {
            int range = 4000;
            int height = 1600;

            if (isInSiege())
            {
                range = 2600;
                height = 1000;
            }

            //L2World.Instance.CheckToUpdate(this, x, y, range, height, true, zones);
        }

        public Timer RegenerationMethod_1s, RegenUpdate;
        public int RegenUpdateInterval = 3000;

        public virtual void StartRegeneration()
        {
            if (RegenerationMethod_1s == null)
            {
                RegenerationMethod_1s = new Timer();
                RegenerationMethod_1s.Interval = 1000;
                RegenerationMethod_1s.Elapsed += new ElapsedEventHandler(RegenTaskDone);
            }

            if (RegenUpdate == null)
            {
                RegenUpdate = new Timer();
                RegenUpdate.Interval = RegenUpdateInterval;
                RegenUpdate.Elapsed += new ElapsedEventHandler(RegenUpdateTaskDone);
            }

            RegenerationMethod_1s.Enabled = true;
            RegenUpdate.Enabled = true;
        }

        public virtual void RegenTaskDone(object sender, ElapsedEventArgs e) { }
        public virtual void RegenUpdateTaskDone(object sender, ElapsedEventArgs e) { }

        public void StopRegeneration()
        {
            if (RegenerationMethod_1s != null)
                RegenerationMethod_1s.Enabled = false;

            if (RegenUpdate != null)
                RegenUpdate.Enabled = false;
        }

        public virtual double Radius
        {
            get { return 11; }
        }

        public virtual double Height
        {
            get { return 22; }
        }

        public virtual string asString()
        {
            return "L2Object:" + ObjID;
        }
    }


}
