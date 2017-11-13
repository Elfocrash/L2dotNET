using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using L2dotNET.Enums;
using L2dotNET.model.items;
using L2dotNET.model.player;
using L2dotNET.model.skills;
using L2dotNET.Models.Stats;
using L2dotNET.Models.Stats.Funcs;
using L2dotNET.Models.Status;
using L2dotNET.Network.serverpackets;
using L2dotNET.templates;
using L2dotNET.tools;
using Calculator = L2dotNET.Models.Stats.Calculator;

namespace L2dotNET.world
{
    public class L2Character : L2Object
    {
        public virtual CharTemplate Template { get; set; }
        public virtual string Name { get; set; }
        public virtual string Title { get; set; }
        public int SpawnX { get; set; }
        public int SpawnY { get; set; }
        public int SpawnZ { get; set; }
        private readonly byte[] _zones = new byte[ZoneId.GetZoneCount()];

        public byte IsRunning { get; set; } = 1;

        public int AbnormalBitMask;
        public int AbnormalBitMaskEx;
        public int AbnormalBitMaskEvent;

        public const int AbnormalMaskBleed = 0x000001;

        public const int AbnormalMaskExInvincible = 0x000001;
        public const int AbnormalMaskExAirStun = 0x000002;
        public const int AbnormalMaskExAirRoot = 0x000004;
        public const int AbnormalMaskExBagSword = 0x000008;
        public const int AbnormalMaskExAfroYellow = 0x000010;
        public const int AbnormalMaskExAfroPink = 0x000020;
        public const int AbnormalMaskExAfroBlack = 0x000040;
        //unk x80
        public const int AbnormalMaskExStigmaShillien = 0x000100;
        public const int AbnormalMaskExStakatoRoot = 0x000200;
        public const int AbnormalMaskExFreezing = 0x000400;
        public const int AbnormalMaskExVesper = 0x000800;

        public const int AbnormalMaskEventIceHand = 0x000008;
        public const int AbnormalMaskEventHeadphone = 0x000010;
        public const int AbnormalMaskEventCrown1 = 0x000020;
        public const int AbnormalMaskEventCrown2 = 0x000040;
        public const int AbnormalMaskEventCrown3 = 0x000080;
        
        public int Int => Stats.Int;

        public int Str => Stats.Str;

        public int Con => Stats.Con;

        public int Men => Stats.Men;

        public int Dex => Stats.Dex;

        public int Wit => Stats.Wit;

        protected byte ZoneValidateCounter = 4;

        public CharStatus Status { get; set; }

        public virtual void UpdateAbnormalEffect() { }

        public virtual void UpdateAbnormalExEffect() { }

        public virtual void UpdateAbnormalEventEffect() { }

        private Timer _updatePositionTime = new Timer(90);

        public Calculator[] Calculators { get; set; }

        public CharacterStat Stats { get; set; }

        public L2Character(int objectId, CharTemplate template) : base(objectId)
        {
            Template = template;
            Stats = new CharacterStat(this);
            InitializeCharacterStatus();
            Calculators = new Calculator[Models.Stats.Stats.Values.Count()];
            AddFuncsToNewCharacter();
            _updatePositionTime.Elapsed += UpdatePositionTask;
        }

        public virtual CharStatus GetStatus()
        {
            return Status;
        }

        public virtual void InitializeCharacterStatus()
        {
            Status = new CharStatus(this);
        }

        public virtual void SetTarget(L2Character obj)
        {
            if (obj != null && !obj.Visible)
                obj = null;

            Target = obj;
        }

        public void AddStatFunc(Func func)
        {
            if (func == null)
                return;

            var statId = Array.IndexOf(Models.Stats.Stats.Values.ToArray(), func.Stat);

            lock (Calculators)
            {
                if (Calculators[statId] == null)
                    Calculators[statId] = new Calculator();

                Calculators[statId].AddFunc(func);
            }
        }

        public void AddStatFuncs(IEnumerable<Func> funcs)
        {
            List<Stat> modifiedStats = new List<Stat>();
            foreach (var func in funcs)
            {
                modifiedStats.Add(func.Stat);
                AddStatFunc(func);
            }
            BroadcastModifiedStats(modifiedStats);
        }

        public void RemoveStatsByOwner(object owner)
        {
            throw new NotImplementedException();
        }

        public virtual void AddFuncsToNewCharacter()
        {
            AddStatFunc(new FuncPAtkMod());
            AddStatFunc(new FuncMAtkMod());
            AddStatFunc(new FuncPDefMod());
            AddStatFunc(new FuncMDefMod());

            AddStatFunc(new FuncMaxHpMul());
            AddStatFunc(new FuncMaxMpMul());

            AddStatFunc(new FuncAtkAccuracy());
            AddStatFunc(new FuncAtkEvasion());

            AddStatFunc(new FuncPAtkSpeed());
            AddStatFunc(new FuncMAtkSpeed());

            AddStatFunc(new FuncMoveSpeed());

            AddStatFunc(new FuncAtkCritical());
            AddStatFunc(new FuncMAtkCritical());
        }

        public double GetLevelMod()
        {
            return (100.0 - 11 + Level) / 100.0;
        }

        public void BroadcastModifiedStats(List<Stat> stats)
        {
            if (stats == null || !stats.Any())
                return;

            bool broadcastFull = false;
            StatusUpdate statusUpdate = null;

            foreach (var stat in stats)
            {
                if (stat == Models.Stats.Stats.PowerAttackSpeed)
                {
                    if(statusUpdate == null)
                        statusUpdate = new StatusUpdate(this);

                    statusUpdate.Add(StatusUpdate.AtkSpd, Stats.PAttackSpeed);
                }
                else if (stat == Models.Stats.Stats.MagicAttackSpeed)
                {
                    if (statusUpdate == null)
                        statusUpdate = new StatusUpdate(this);

                    statusUpdate.Add(StatusUpdate.CastSpd, Stats.MAttackSpeed);
                }
                else if (stat == Models.Stats.Stats.MaxHp)
                {
                    if (statusUpdate == null)
                        statusUpdate = new StatusUpdate(this);

                    statusUpdate.Add(StatusUpdate.MaxHp, Stats.MaxHp);
                }else if (stat == Models.Stats.Stats.RunSpeed)
                    broadcastFull = true;
            }

            if (this is L2Player player)
            {
                if (broadcastFull)
                    player.UpdateAndBroadcastStatus(2);
                else
                {
                    player.UpdateAndBroadcastStatus(1);
                    if(statusUpdate != null)
                        BroadcastPacket(statusUpdate);
                }
            }
            else if(statusUpdate != null)
                BroadcastPacket(statusUpdate);
        }

        public override void OnForcedAttack(L2Player player)
        {
            bool newtarget = false;
            if (player.Target == null)
            {
                player.Target = this;
                newtarget = true;
            }
            else
            {
                if (player.Target.ObjId != ObjId)
                {
                    player.Target = this;
                    newtarget = true;
                }
            }

            if (newtarget)
                player.SendPacket(new MyTargetSelected(ObjId, 0));
            else
                player.SendActionFailed();
        }

        public override void OnSpawn(bool notifyOthers = true)
        {
            base.OnSpawn(notifyOthers);
            RevalidateZone(true);
        }

        public virtual void DeleteMe()
        {
            //foreach (L2Player o in KnownObjects.Values.OfType<L2Player>())
            //    o.SendPacket(new DeleteObject(ObjId));

        }

        public void RevalidateZone(bool force)
        {
            if (Region == null)
                return;

            if (force)
                ZoneValidateCounter = 4;
            else
            {
                ZoneValidateCounter--;
                if (ZoneValidateCounter < 0)
                    ZoneValidateCounter = 4;
                else
                    return;
            }

            Region.RevalidateZones(this);
        }

        public override void SetRegion(L2WorldRegion newRegion)
        {
            // confirm revalidation of old region's zones
            if (Region != null)
            {
                if (newRegion != null)
                    Region.RevalidateZones(this);
                else
                    Region.RemoveFromZones(this);
            }

            base.SetRegion(newRegion);
        }

        public void SetInsisdeZone(ZoneId zone, bool state)
        {
            if (state)
                _zones[(int)zone.Id]++;
            else
                _zones[(int)zone.Id]--;
        }

        public virtual void SendMessage(string p) { }

        public virtual void SendActionFailed() { }

        public virtual void SendSystemMessage(SystemMessage.SystemMessageId msgId) { }

        public virtual void OnPickUp(L2Item item) { }

        public int BuffMax = Config.Config.Instance.GameplayConfig.PlayerConfig.Buff.MaxBuffsAmount;
        
        public int ClientPosX,
                   ClientPosY,
                   ClientPosZ,
                   ClientHeading;

        public virtual void Teleport(int x, int y, int z)
        {
            SetTarget(null);
            //clearKnowns(true);
            X = x;
            Y = y;
            Z = z;

            if (!(this is L2Player))
                return;

            BroadcastPacket(new TeleportToLocation(ObjId, x, y, z, Heading));
        }

        private Timer _waterTimer;
        private DateTime _waterTimeDamage;

        public void WaterTimer()
        {
            if (IsInWater())
            {
                bool next = false;
                if ((_waterTimer == null) || !_waterTimer.Enabled)
                {
                    _waterTimer = new Timer();
                    _waterTimer.Elapsed += WaterActionTime;
                    _waterTimer.Interval = 3000;
                    next = true;
                }

                if (!next)
                    return;

                int breath = 100;
                _waterTimeDamage = DateTime.Now.AddSeconds(breath);
                _waterTimer.Enabled = true;

                if (this is L2Player)
                    SendPacket(new SetupGauge(ObjId, SetupGauge.SgColor.Cyan, breath * 1000));
            }
            else
            {
                if ((_waterTimer == null) || !_waterTimer.Enabled)
                    return;

                _waterTimer.Enabled = false;

                if (this is L2Player)
                    SendPacket(new SetupGauge(ObjId, SetupGauge.SgColor.Cyan, 1));
            }

            //if (!isInWater())
            //{
            //    if (_waterTimer == null)
            //        return;

            //    if (_waterTimer.Enabled)
            //    {
            //        _waterTimer.Stop();
            //        _waterTimer.Enabled = false;
            //        _waterTimer = null;

            //        if (this is L2Player)
            //        {
            //            sendPacket(new SetupGauge(ObjID, SetupGauge.SG_color.cyan, 1));
            //        }
            //    }

            //    return;
            //}
            //else
            //{
            //    if (_waterTimer == null)
            //    {
            //        int breath = (int)CharacterStat.getStat(TEffectType.b_breath);
            //        _waterTimeDamage = DateTime.Now.AddSeconds(breath);
            //        _waterTimer = new System.Timers.Timer(breath * 1000);
            //        _waterTimer.Elapsed += new ElapsedEventHandler(waterActionTime);
            //        _waterTimer.Enabled = true;
            //        _waterTimer.Interval = 3000;

            //        return;
            //    }
            //}
        }

        private void WaterActionTime(object sender, ElapsedEventArgs e)
        {
            TimeSpan ts = _waterTimeDamage - DateTime.Now;
            if (!(ts.TotalMilliseconds < 0))
                return;

            if (this is L2Player)
                ReduceHpArea(200, 297);
        }

        public void ReduceHpArea(int damage, int msgId)
        {
            //if (Dead)
            //    return;

            //CurHp -= damage;

            //StatusUpdate su = new StatusUpdate(ObjId);
            //su.Add(StatusUpdate.CurHp, (int)CurHp);
            //su.Add(StatusUpdate.MaxHp, (int)CharacterStat.GetStat(EffectType.BMaxHp));
            //BroadcastPacket(su);

            //if (CurHp <= 0)
            //{
            //    Dead = true;
            //    CurHp = 0;
            //    DoDie(null, true);
            //    return;
            //}

            //if (this is L2Player)
            //    SendPacket(new SystemMessage((SystemMessage.SystemMessageId)msgId).AddNumber(damage));
        }

        public override void ReduceHp(L2Character attacker, double damage)
        {
            if (Dead)
                return;

            //if ((this is L2Player && attacker is L2Player))
            //{
            //    if (CurCp > 0)
            //    {
            //        CurCp -= damage;

            //        if (CurCp < 0)
            //        {
            //            damage = CurCp * -1;
            //            CurCp = 0;
            //        }
            //    }
            //}

            CurHp -= damage;

            StatusUpdate su = new StatusUpdate(this);
            su.Add(StatusUpdate.CurHp, (int)CurHp);
           // su.Add(StatusUpdate.CurCp, (int)CurCp);
            BroadcastPacket(su);

            if (CurHp <= 0)
            {
                CurHp = 0;
                DoDie(attacker);
                return;
            }
        }

        public virtual void DoDie(L2Character killer)
        {
            Dead = true;
            if (IsAttacking())
                AbortAttack();

            Status.StopHpMpRegeneration();

            CurHp = 0;
            StatusUpdate su = new StatusUpdate(this);
            su.Add(StatusUpdate.CurHp, 0);
            BroadcastPacket(su);

            BroadcastPacket(new Die(this));
        }

        public virtual void DeleteByForce()
        {
            BroadcastPacket(new DeleteObject(ObjId));
            L2World.Instance.RemoveObject(this);
        }

        public virtual L2Item ActiveWeapon => null;

        public virtual L2Item SecondaryWeapon => null;

        public virtual L2Item ActiveArmor => null;

        public L2Character TargetToHit { get; set; }

        public L2Character Target { get; set; }

        public virtual void DoAttack(L2Character target)
        {
            if (target == null)
            {
                return;
            }

            if (target.Dead)
            {
                return;
            }

            if ((AttackToHit != null) && AttackToHit.Enabled)
                return;

            if ((AttackToEnd != null) && AttackToEnd.Enabled)
                return;

            double dist = 60,
                   reqMp = 0;

            L2Item weapon = ActiveWeapon;
            double timeAtk = 100;//attackspeed
            bool dual = false,
                 ranged = false,
                 ss = false;
            if (weapon != null) { }
            else
                timeAtk = (1362 * 345) / timeAtk;

            if (!Calcs.CheckIfInRange((int)dist, this, target, true))
            {
                TryMoveTo(target.X, target.Y, target.Z);
                return;
            }

            if ((reqMp > 0) && (reqMp > CurMp))
            {
                SendMessage($"no mp {CurMp} {reqMp}");
                return;
            }

            Attack atk = new Attack(this, target, ss, 5);

            if (dual)
            {
                Hit1 = GenHitSimple(true, ss);
                atk.AddHit(target.ObjId, (int)Hit1.Damage, Hit1.Miss, Hit1.Crit, Hit1.ShieldDef > 0);

                Hit2 = GenHitSimple(true, ss);
                atk.AddHit(target.ObjId, (int)Hit2.Damage, Hit2.Miss, Hit2.Crit, Hit2.ShieldDef > 0);
            }
            else
            {
                Hit1 = GenHitSimple(false, ss);
                atk.AddHit(target.ObjId, (int)Hit1.Damage, Hit1.Miss, Hit1.Crit, Hit1.ShieldDef > 0);
            }

            Target = target;

            if (AttackToHit == null)
            {
                AttackToHit = new Timer();
                AttackToHit.Elapsed += AttackDoHit;
            }

            double timeToHit = ranged ? timeAtk * 0.5 : timeAtk * 0.6;
            AttackToHit.Interval = timeToHit;
            AttackToHit.Enabled = true;

            if (dual)
            {
                if (AttackToHitBonus == null)
                {
                    AttackToHitBonus = new Timer();
                    AttackToHitBonus.Elapsed += AttackDoHit2Nd;
                }

                AttackToHitBonus.Interval = timeAtk * 0.78;
                AttackToHitBonus.Enabled = true;
            }

            if (AttackToEnd == null)
            {
                AttackToEnd = new Timer();
                AttackToEnd.Elapsed += AttackDoEnd;
            }

            AttackToEnd.Interval = timeAtk;
            AttackToEnd.Enabled = true;

            BroadcastPacket(atk);
        }

        public class Hit
        {
            public bool Miss;
            public double ShieldDef;
            public bool Crit;
            public double Damage;
        }

        public Hit Hit1,
                   Hit2;

        public Hit GenHitSimple(bool dual, bool ss)
        {
            Hit h = new Hit
            {
                Miss = false
            };
            if (h.Miss)
                return h;

            h.ShieldDef = 0;
            h.Crit = false;
            h.Damage = 100;
            if (dual)
                h.Damage *= .5;
            if (ss)
                h.Damage *= 2;
            if (h.Crit)
                h.Damage *= 2;

            return h;
        }

        public virtual void AttackDoHit(object sender, ElapsedEventArgs e)
        {
            if (Target != null)
            {
                if (!Hit1.Miss)
                {
                    Target.ReduceHp(this, Hit1.Damage);

                    if (Target is L2Player)
                        Target.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1HasReceivedS3DamageFromC2).AddName(Target).AddName(this).AddNumber(Hit1.Damage));
                }
                else
                {
                    if (Target is L2Player)
                    {
                        Target.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1HasEvadedC2Attack).AddName(Target).AddName(this));
                    }
                }
            }

            AttackToHit.Enabled = false;
        }

        public virtual void AttackDoHit2Nd(object sender, ElapsedEventArgs e)
        {
            if (Target != null)
            {
                if (!Hit2.Miss)
                {
                    Target.ReduceHp(this, Hit2.Damage);
                    if (Target is L2Player)
                        Target.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1HasReceivedS3DamageFromC2).AddName(Target).AddName(this).AddNumber(Hit2.Damage));
                }
                else
                {
                    if (Target is L2Player)
                    {
                        Target.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1HasEvadedC2Attack).AddName(Target).AddName(this));
                    }
                }
            }

            AttackToHitBonus.Enabled = false;
        }

        public virtual void AttackDoEnd(object sender, ElapsedEventArgs e)
        {
            AttackToEnd.Enabled = false;

            //L2Item weapon = Inventory.getWeapon();
            //if (weapon != null)
            //{
            //    if (weapon.Soulshot)
            //        weapon.Soulshot = false;

            //    foreach (int sid in weapon.Template.getSoulshots())
            //        if (autoSoulshots.Contains(sid))
            //        {
            //            if (Inventory.getItemCount(sid) < weapon.Template.SoulshotCount)
            //            {
            //                sendPacket(new SystemMessage(1435).addItemName(sid));//Due to insufficient $s1, the automatic use function has been deactivated.

            //                lock (autoSoulshots)
            //                {
            //                    autoSoulshots.Remove(sid);
            //                    sendPacket(new ExAutoSoulShot(sid, 0));
            //                }
            //            }
            //            else
            //            {
            //                Inventory.destroyItem(sid, weapon.Template.SoulshotCount, false, true);
            //                weapon.Soulshot = true;
            //                broadcastSoulshotUse(sid);
            //            }

            //            break;
            //        }
            //}

            // if (Target != null)
            //    doAttack((L2Character)Target);
        }

        public void BroadcastSoulshotUse(int itemId)
        {
            int skillId = 0;
            switch (itemId)
            {
                case 1835:
                case 5789:
                    skillId = 2039;
                    break;
                case 1463:
                    skillId = 2150;
                    break;
                case 1464:
                    skillId = 2151;
                    break;
                case 1465:
                    skillId = 2152;
                    break;
                case 1466:
                    skillId = 2153;
                    break;
                case 1467:
                    skillId = 2154;
                    break;
                case 22082:
                    skillId = 26060;
                    break;
                case 22083:
                    skillId = 26061;
                    break;
                case 22084:
                    skillId = 26062;
                    break;
                case 22085:
                    skillId = 26063;
                    break;
                case 22086:
                    skillId = 26064;
                    break;
            }

            if (skillId <= 0)
                return;

            BroadcastPacket(new MagicSkillUse(this, this, skillId, 1, 0));
            SendSystemMessage(SystemMessage.SystemMessageId.EnabledSoulshot);
        }

        public virtual void AbortAttack()
        {
            if ((AttackToHit != null) && AttackToHit.Enabled)
                AttackToHit.Enabled = false;

            if ((AttackToHitBonus != null) && AttackToHitBonus.Enabled)
                AttackToHitBonus.Enabled = false;

            if ((AttackToEnd != null) && AttackToEnd.Enabled)
                AttackToEnd.Enabled = false;

            //  hit1 = null;
            //  hit2 = null;
        }

        public Timer AttackToHit,
                     AttackToHitBonus,
                     AttackToEnd;

        public int PBlockSpell = 0,
                   PBlockSkill = 0;
        public int PBlockAct = 0;

        public virtual bool CantMove()
        {
            if (PBlockAct == 1)
                return true;

            if ((AbnormalBitMaskEx & AbnormalMaskExFreezing) == AbnormalMaskExFreezing)
                return true;

            return false;
        }

        public void TryMoveTo(int x, int y, int z)
        {
            TargetToHit = null;
            if (CantMove())
            {
                SendActionFailed();
                return;
            }

            DestX = x;
            DestY = y;
            DestZ = z;

            MoveTo(x, y, z);
        }

        public void TryMoveToAndHit(int x, int y, int z,L2Character target)
        {
            TargetToHit = target;
            if (CantMove())
            {
                SendActionFailed();
                return;
            }

            DestX = x;
            DestY = y;
            DestZ = z;

            MoveToAndHit(x, y, z);
        }

        public void Status_FreezeMe(bool status, bool update)
        {
            if (status)
                AbnormalBitMaskEx |= AbnormalMaskExFreezing;
            else
                AbnormalBitMaskEx &= ~AbnormalMaskExFreezing;

            if (update)
                UpdateAbnormalExEffect();
        }

        public virtual void OnOldTargetSelection(L2Object target) { }

        public virtual void OnNewTargetSelection(L2Object target) { }
        
        public virtual void BroadcastStatusUpdate()
        {
            if (!Status.StatusListener.Any())
                return;

            //will look into this later
            //if (!needHpUpdate(352))
            //    return;

            StatusUpdate su = new StatusUpdate(this);
            su.Add(StatusUpdate.CurHp, (int)CurHp);

            foreach (var temp in Status.StatusListener)
                temp?.SendPacket(su);

        }
        
        public virtual L2Character[] GetPartyCharacters()
        {
            return new[] { this };
        }

        public bool IsMoving()
        {
            return (_updatePositionTime != null) && _updatePositionTime.Enabled;
        }

        public void MoveTo(int x, int y, int z)
        {
            TargetToHit = null;

            if (IsAttacking())
                AbortAttack();

            if (_updatePositionTime.Enabled) // новый маршрут, но старый не закончен
                NotifyStopMove(false);


            DestX = x;
            DestY = y;
            DestZ = z;

            double dx = x - X,
                   dy = y - Y;
            //dz = (z - Z);
            double distance = getPlanDistanceSq(x, y);

            double spy = dy / distance,
                   spx = dx / distance;

            double speed = 130; //TODO: Human Figher Speed Based, need get characters run speed

            //TODO: check possible divisions by zero
            _ticksToMove = (int)Math.Ceiling((10 * distance) / speed); //Client Response time = 1000ms, XYZ server check = 100ms (distance * 10 to get better precision)
            _ticksToMoveCompleted = 0;
            _xSpeedTicks = (DestX - X) / (float)_ticksToMove;
            _ySpeedTicks = (DestY - Y) / (float)_ticksToMove;

            Heading = (int)((Math.Atan2(-spx, -spy) * 10430.378) + short.MaxValue);

            BroadcastPacket(new CharMoveToLocation(this));

            _updatePositionTime.Enabled = true;
        }


        public void MoveToAndHit(int x, int y, int z)
        {
            if (IsAttacking())
                AbortAttack();

            if (_updatePositionTime.Enabled) // новый маршрут, но старый не закончен
                NotifyStopMove(false);            

            DestX = x;
            DestY = y;
            DestZ = z;

            double dx = x - X,
                   dy = y - Y;
            //dz = (z - Z);
            double distance = getPlanDistanceSq(x, y);

            double spy = dy / distance,
                   spx = dx / distance;

            double speed = 130; //TODO: Human Figher Speed Based, need get characters run speed

            //TODO: check possible divisions by zero
            _ticksToMove = (int)Math.Ceiling((10 * distance) / speed); //Client Response time = 1000ms, XYZ server check = 100ms (distance * 10 to get better precision)
            _ticksToMoveCompleted = 0;
            _xSpeedTicks = (DestX - X) / (float)_ticksToMove;
            _ySpeedTicks = (DestY - Y) / (float)_ticksToMove;

            Heading = (int)((Math.Atan2(-spx, -spy) * 10430.378) + short.MaxValue);

            BroadcastPacket(new CharMoveToLocation(this));

            _updatePositionTime.Enabled = true;
        }


        private void UpdatePositionTask(object sender, ElapsedEventArgs e)
        {
            ValidateWaterZones();

            if ((DestX == X) && (DestY == Y) && (DestZ == Z))
            {
                NotifyArrived();
                return;
            }

            if (_ticksToMove > _ticksToMoveCompleted)
            {
                _ticksToMoveCompleted++;
                X += (int)_xSpeedTicks;
                Y += (int)_ySpeedTicks;
            }
            else
            {
                X = DestX;
                Y = DestY;
                Z = DestZ;
                NotifyArrived();
            }
        }

        public virtual void NotifyStopMove(bool broadcast, bool update = false)
        {
            if (_updatePositionTime.Enabled)
                _updatePositionTime.Enabled = false;

            if (update)
                BroadcastPacket(new StopMove(this));

            DestX = 0;
            DestY = 0;
            DestZ = 0;
            _xSpeedTicks = 0;
            _ySpeedTicks = 0;
            _ticksToMove = 0;
            _ticksToMoveCompleted = 0;
        }

        public virtual void NotifyArrived()
        {
            if (TargetToHit != null)
                this.DoAttack(TargetToHit);

            if (_updatePositionTime.Enabled)
                _updatePositionTime.Enabled = false;

            DestX = 0;
            DestY = 0;
            DestZ = 0;
            _xSpeedTicks = 0;
            _ySpeedTicks = 0;
            _ticksToMove = 0;      
        }


        private int _ticksToMove,
                    _ticksToMoveCompleted;
        private float _xSpeedTicks;
        private float _ySpeedTicks;

        public bool IsInFrontOfTarget()
        {
            return false;
        }

        public bool IsInFrontOfTarget(int x, int y, int heading)
        {
            return false;
        }

        public bool IsBehindTarget()
        {
            return false;
        }

        public virtual bool IsAttacking()
        {
            return (AttackToEnd != null) && AttackToEnd.Enabled;
        }

        public virtual int MaxHp => Stats.MaxHp;
        public virtual int MaxMp => Stats.MaxMp;

        public override double CurHp => Status.CurrentHp;

        public override double CurMp => Status.CurrentMp;

        public override string AsString()
        {
            return $"L2Character: {ObjId}";
        }

        public virtual L2Item GetWeaponItem()
        {
            return null;
        }

        private List<long> _muted0;
        private List<long> _muted1;
        private List<long> _muted2;

        public void Mute(int type, long hashId, bool start)
        {
            List<long> list = null;
            switch (type)
            {
                case 0:
                    if (_muted0 == null)
                        _muted0 = new List<long>();

                    list = _muted0;
                    break;
                case 1:
                    if (_muted1 == null)
                        _muted1 = new List<long>();

                    list = _muted1;
                    break;
                case 2:
                    if (_muted2 == null)
                        _muted2 = new List<long>();

                    list = _muted2;
                    break;
            }

            if (start)
            {
                if ((list != null) && !list.Contains(hashId))
                    list.Add(hashId);
            }
            else
                list?.Remove(hashId);
        }

        public bool MutedPhysically => (_muted0 != null) && (_muted0.Count > 0);

        public bool MutedMagically => (_muted1 != null) && (_muted1.Count > 0);

        public bool MutedSpecial => (_muted2 != null) && (_muted2.Count > 0);

        /// <summary>
        /// Return the squared plan distance between the current position of the L2Character and the given x, y, z.
        /// (check only x and y, not z)
        /// </summary>
        /// <param name="x">X position of the target</param>
        /// <param name="y">Y position of the target</param>
        /// <returns>return the squared plan distance</returns>
        public double getPlanDistanceSq(int x, int y)
        {
            return Math.Sqrt(Math.Pow(x - X, 2) + Math.Pow(y - Y, 2));
        }
    }
}