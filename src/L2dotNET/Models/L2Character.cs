using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using L2dotNET.Enums;
using L2dotNET.Models.Items;
using L2dotNET.Models.Player;
using L2dotNET.Models.Player.General;
using L2dotNET.Models.Stats;
using L2dotNET.Models.Stats.Funcs;
using L2dotNET.Models.Status;
using L2dotNET.Network.serverpackets;
using L2dotNET.Templates;
using L2dotNET.Tools;
using L2dotNET.World;
using Calculator = L2dotNET.Models.Stats.Calculator;

namespace L2dotNET.Models
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

        #region AbhornalMask
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
        #endregion

        public int Int => CharacterStat.Int;

        public int Str => CharacterStat.Str;

        public int Con => CharacterStat.Con;

        public int Men => CharacterStat.Men;

        public int Dex => CharacterStat.Dex;

        public int Wit => CharacterStat.Wit;

        public int MaxHp => CharacterStat.MaxHp;

        public int MaxMp => CharacterStat.MaxMp;

        public int MaxCp => CharacterStat.MaxCp;

        protected byte zoneValidateCounter = 4;

        public CharStatus CharStatus { get; set; }

        public virtual void UpdateAbnormalEffect() { }

        public virtual void UpdateAbnormalExEffect() { }

        public virtual void UpdateAbnormalEventEffect() { }

        public CharacterMovement CharMovement { get; }

        public CharacterStat CharacterStat { get; set; }

        public override int X
        {
            get => CharMovement.X;
            set => CharMovement.X = value;
        }

        public override int Y
        {
            get => CharMovement.Y;
            set => CharMovement.Y = value;
        }

        public override int Z
        {
            get => CharMovement.Z;
            set => CharMovement.Z = value;
        }

        public L2Character(int objectId, CharTemplate template) : base(objectId)
        {
            Template = template;
            CharacterStat = new CharacterStat(this);
            CharMovement = new CharacterMovement(this);
            InitializeCharacterStatus();
            AddFuncsToNewCharacter();
        }

        public virtual CharStatus GetStatus()
        {
            return CharStatus;
        }

        public virtual void InitializeCharacterStatus()
        {
            CharStatus = new CharStatus(this);
        }

        public virtual async Task SetTargetAsync(L2Character obj)
        {
            await Task.Run(() =>
            {
                if (obj != null && !obj.Visible)
                    obj = null;

                Target = obj;
            });
        }
        /*
        public async Task AddStatFuncsAsync(IEnumerable<StatFunction> funcs)
        {
            List<Stat> modifiedStats = new List<Stat>();
            foreach (var func in funcs)
            {
                modifiedStats.Add(func.Stat);
                AddStatFunc(func);
            }
            await BroadcastModifiedStatsAsync(modifiedStats);
        }*/

        public void RemoveStatsByOwner(object owner)
        {
            throw new NotImplementedException();
        }

        public virtual void AddFuncsToNewCharacter()
        {
            CharacterStat.AddStatFunction(FuncPAtkMod.Instance);
            CharacterStat.AddStatFunction(FuncMAtkMod.Instance);
            CharacterStat.AddStatFunction(FuncPDefMod.Instance);
            CharacterStat.AddStatFunction(FuncMDefMod.Instance);

            CharacterStat.AddStatFunction(FuncMaxHpMul.Instance);
            CharacterStat.AddStatFunction(FuncMaxMpMul.Instance);

            CharacterStat.AddStatFunction(FuncAtkAccuracy.Instance);
            CharacterStat.AddStatFunction(FuncAtkEvasion.Instance);

            CharacterStat.AddStatFunction(FuncPAtkSpeed.Instance);
            CharacterStat.AddStatFunction(FuncMAtkSpeed.Instance);

            CharacterStat.AddStatFunction(FuncMoveSpeed.Instance);

            CharacterStat.AddStatFunction(FuncAtkCritical.Instance);
            CharacterStat.AddStatFunction(FuncMAtkCritical.Instance);
        }

        public double GetLevelMod()
        {
            return (100.0 - 11 + Level) / 100.0;
        }
        /*
        public async Task BroadcastModifiedStatsAsync(List<Stat> stats)
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

                    statusUpdate.Add(StatusUpdate.AtkSpd, CharacterStat.PAttackSpeed);
                }
                else if (stat == Models.Stats.Stats.MagicAttackSpeed)
                {
                    if (statusUpdate == null)
                        statusUpdate = new StatusUpdate(this);

                    statusUpdate.Add(StatusUpdate.CastSpd, CharacterStat.MAttackSpeed);
                }
                else if (stat == Models.Stats.Stats.MaxHp)
                {
                    if (statusUpdate == null)
                        statusUpdate = new StatusUpdate(this);

                    statusUpdate.Add(StatusUpdate.MaxHp, CharacterStat.MaxHp);
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
                        await BroadcastPacketAsync(statusUpdate);
                }
            }
            else if(statusUpdate != null)
                await BroadcastPacketAsync(statusUpdate);
        }
        */
        public override async Task OnForcedAttackAsync(L2Player player)
        {
            await player.SendActionFailedAsync();
        }

        public override async Task OnSpawnAsync(bool notifyOthers = true)
        {
            await base.OnSpawnAsync(notifyOthers);
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
                zoneValidateCounter = 4;
            else
            {
                zoneValidateCounter--;
                if (zoneValidateCounter < 0)
                    zoneValidateCounter = 4;
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

        public virtual async Task SendMessageAsync(string p) { await Task.FromResult(1); }

        public virtual async Task SendActionFailedAsync()
        {
            await Task.FromResult(1);
        }

        public virtual async Task SendSystemMessage(SystemMessage.SystemMessageId msgId) { await Task.FromResult(1); }

        public virtual void OnPickUp(L2Item item) { }
        
        public int ClientPosX,
                   ClientPosY,
                   ClientPosZ,
                   ClientHeading;

        public virtual async Task TeleportAsync(int x, int y, int z)
        {
            SetTargetAsync(null);
            //clearKnowns(true);
            X = x;
            Y = y;
            Z = z;

            if (!(this is L2Player))
                return;

            await BroadcastPacketAsync(new TeleportToLocation(ObjectId, x, y, z, Heading));
        }

        private Timer _waterTimer;
        private DateTime _waterTimeDamage;

        public async Task WaterTimer()
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
                    await SendPacketAsync(new SetupGauge(ObjectId, SetupGauge.SgColor.Cyan, breath * 1000));
            }
            else
            {
                if ((_waterTimer == null) || !_waterTimer.Enabled)
                    return;

                _waterTimer.Enabled = false;

                if (this is L2Player)
                    await SendPacketAsync(new SetupGauge(ObjectId, SetupGauge.SgColor.Cyan, 1));
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
            //    DoDieAsync(null, true);
            //    return;
            //}

            //if (this is L2Player)
            //    SendPacket(new SystemMessage((SystemMessage.SystemMessageId)msgId).AddNumber(damage));
        }

        //public override void ReduceHp(L2Character attacker, double damage)
        //{
        //    if (Dead)
        //        return;

        //    //if ((this is L2Player && attacker is L2Player))
        //    //{
        //    //    if (CurCp > 0)
        //    //    {
        //    //        CurCp -= damage;

        //    //        if (CurCp < 0)
        //    //        {
        //    //            damage = CurCp * -1;
        //    //            CurCp = 0;
        //    //        }
        //    //    }
        //    //}

        //    CharStatus.ReduceHp(damage,attacker);

        //    StatusUpdate statusUpdate = new StatusUpdate(this);
        //    statusUpdate.Add(StatusUpdate.CurHp, (int)CharStatus.CurrentHp);
        //    // statusUpdate.Add(StatusUpdate.CurCp, (int)CurCp);
        //    BroadcastPacket(statusUpdate);

        //    if (CharStatus.CurrentHp <= 0)
        //    {
        //        DoDieAsync(attacker);
        //        return;
        //    }
        //}

        public virtual async Task DoDieAsync(L2Character killer)
        {
            lock (this)
            {
                if (Dead)
                    return;

                CharStatus.SetCurrentHp(0);

                Dead = true;
            }

            Target = null;
            await CharMovement.NotifyStopMove(true);

            if (IsAttacking())
                AbortAttack();

            CharStatus.StopHpMpRegeneration();

            await BroadcastStatusUpdateAsync();

            await BroadcastPacketAsync(new Die(this));
        }

        public virtual async Task DeleteByForceAsync()
        {
            await BroadcastPacketAsync(new DeleteObject(ObjectId));
            L2World.RemoveObject(this);
        }

        public virtual L2Item ActiveWeapon => null;

        public virtual L2Item SecondaryWeapon => null;

        public virtual L2Item ActiveArmor => null;

        public L2Character TargetToHit { get; set; }

        public L2Character Target { get; set; }

        public virtual async Task DoAttackAsync(L2Character target)
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
                await CharMovement.MoveTo(target.X, target.Y, target.Z);
                return;
            }

            if ((reqMp > 0) && (reqMp > CharStatus.CurrentMp))
            {
                await SendMessageAsync($"no mp {CharStatus.CurrentMp} {reqMp}");
                return;
            }

            Attack atk = new Attack(this, target, ss, 5);

            if (dual)
            {
                Hit1 = GenHitSimple(true, ss);
                atk.AddHit(target.ObjectId, (int)Hit1.Damage, Hit1.Miss, Hit1.Crit, Hit1.ShieldDef > 0);

                Hit2 = GenHitSimple(true, ss);
                atk.AddHit(target.ObjectId, (int)Hit2.Damage, Hit2.Miss, Hit2.Crit, Hit2.ShieldDef > 0);
            }
            else
            {
                Hit1 = GenHitSimple(false, ss);
                atk.AddHit(target.ObjectId, (int)Hit1.Damage, Hit1.Miss, Hit1.Crit, Hit1.ShieldDef > 0);
            }

            Target = target;

            if (AttackToHit == null)
            {
                AttackToHit = new Timer();
                AttackToHit.Elapsed += AttackDoHitAsync;
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

            await BroadcastPacketAsync(atk);
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

        public virtual async void AttackDoHitAsync(object sender, ElapsedEventArgs e)
        {
            if (Target != null)
            {
                if (!Hit1.Miss)
                {
                    Target.CharStatus.ReduceHp(Hit1.Damage, this);

                    if (Target is L2Player)
                        await Target.SendPacketAsync(new SystemMessage(SystemMessage.SystemMessageId.C1HasReceivedS3DamageFromC2).AddName(Target).AddName(this).AddNumber(Hit1.Damage));
                }
                else
                {
                    if (Target is L2Player)
                    {
                        await Target.SendPacketAsync(new SystemMessage(SystemMessage.SystemMessageId.C1HasEvadedC2Attack).AddName(Target).AddName(this));
                    }
                }
            }

            AttackToHit.Enabled = false;
        }

        public virtual async void AttackDoHit2Nd(object sender, ElapsedEventArgs e)
        {
            if (Target != null)
            {
                if (!Hit2.Miss)
                {
                    Target.CharStatus.ReduceHp(Hit2.Damage, this);
                    if (Target is L2Player)
                        await Target.SendPacketAsync(new SystemMessage(SystemMessage.SystemMessageId.C1HasReceivedS3DamageFromC2).AddName(Target).AddName(this).AddNumber(Hit2.Damage));
                }
                else
                {
                    if (Target is L2Player)
                    {
                        await Target.SendPacketAsync(new SystemMessage(SystemMessage.SystemMessageId.C1HasEvadedC2Attack).AddName(Target).AddName(this));
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

        public async Task BroadcastSoulshotUseAsync(int itemId)
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

            await BroadcastPacketAsync(new MagicSkillUse(this, this, skillId, 1, 0));
            await SendSystemMessage(SystemMessage.SystemMessageId.EnabledSoulshot);
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
        
        public virtual async Task BroadcastStatusUpdateAsync()
        {
            if (!CharStatus.StatusListener.Any())
                return;

            //will look into this later
            //if (!needHpUpdate(352))
            //    return;

            StatusUpdate su = new StatusUpdate(this);
            su.Add(StatusUpdate.CurHp, (int)CharStatus.CurrentHp);

            foreach (var temp in CharStatus.StatusListener)
            {
                if(temp.ObjectId != ObjectId)
                    await temp.SendPacketAsync(su);
            }
        }
        
        public virtual L2Character[] GetPartyCharacters()
        {
            return new[] { this };
        }

        

        


        

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

        public override string AsString()
        {
            return $"L2Character: {ObjectId}";
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
    }
}