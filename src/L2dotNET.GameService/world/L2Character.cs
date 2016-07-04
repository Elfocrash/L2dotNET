using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using L2dotNET.GameService.Enums;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Playable;
using L2dotNET.GameService.Model.Playable.PetAI;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.GameService.Model.Stats;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tools;

namespace L2dotNET.GameService.World
{
    public class L2Character : L2Object
    {
        public SortedList<int, TSkill> Skills = new SortedList<int, TSkill>();

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

        public virtual void UpdateAbnormalEffect() { }

        public virtual void UpdateAbnormalExEffect() { }

        public virtual void UpdateAbnormalEventEffect() { }

        public StandartAiTemplate AiCharacter = new StandartAiTemplate();

        public override void OnAction(L2Player player)
        {
            bool newtarget = false;
            if (player.CurrentTarget == null)
            {
                player.CurrentTarget = this;
                newtarget = true;
            }
            else
            {
                if (player.CurrentTarget.ObjId != ObjId)
                {
                    player.CurrentTarget = this;
                    newtarget = true;
                }
            }

            if (newtarget)
                player.SendPacket(new MyTargetSelected(ObjId, 0));
        }

        public override void OnForcedAttack(L2Player player)
        {
            bool newtarget = false;
            if (player.CurrentTarget == null)
            {
                player.CurrentTarget = this;
                newtarget = true;
            }
            else
            {
                if (player.CurrentTarget.ObjId != ObjId)
                {
                    player.CurrentTarget = this;
                    newtarget = true;
                }
            }

            if (newtarget)
                player.SendPacket(new MyTargetSelected(ObjId, 0));
            else
                player.SendActionFailed();
        }

        public override void SetRegion(L2WorldRegion newRegion)
        {
            // confirm revalidation of old region's zones
            if (Region != null)
                if (newRegion != null)
                    Region.RevalidateZones(this);
                else
                    Region.RemoveFromZones(this);

            base.SetRegion(newRegion);
        }

        public void AddSkill(int id, int lvl, bool updDb, bool update)
        {
            TSkill skill = TSkillTable.Instance.Get(id, lvl);
            if (skill != null)
                AddSkill(skill, updDb, update);
        }

        public virtual void AddSkill(TSkill newsk, bool updDb, bool update)
        {
            lock (Skills)
            {
                if (Skills.ContainsKey(newsk.skill_id))
                {
                    if (newsk.OpType == TSkillOperational.P)
                        RemoveStats(Skills[newsk.skill_id], this);

                    Skills.Remove(newsk.skill_id);
                }
            
                Skills.Add(newsk.skill_id, newsk);
            }

            if (newsk.OpType == TSkillOperational.P)
                AddStats(newsk, this);
        }

        public void SetInsisdeZone(ZoneId zone, bool state)
        {
            if (state)
                _zones[(int)zone.Id]++;
            else
                _zones[(int)zone.Id]--;
        }

        public virtual void RemoveSkill(int id, bool updDb, bool update)
        {
            lock (Skills)
            {
                if (Skills.ContainsKey(id))
                {
                    if (Skills[id].OpType == TSkillOperational.P)
                        RemoveStats(Skills[id], this);

                    Skills.Remove(id);
                }
            }
        }

        private void RemoveStats(TSkill skill, L2Character caster)
        {
            if (skill.effects.Count > 0)
            {
                TEffectResult result = CharacterStat.Stop(skill.effects, caster);
                if (this is L2Player)
                {
                    if (result.TotalUI == 1)
                        BroadcastUserInfo();
                    else
                    {
                        if (result.sus != null)
                        {
                            StatusUpdate su = new StatusUpdate(ObjId);
                            foreach (int stat in result.sus.Keys)
                                su.add(stat, (int)result.sus[stat]);

                            BroadcastPacket(su, false);

                            if (this is L2Player && (result.HpMpCp == 1) && (((L2Player)this).Party != null))
                                ((L2Player)this).Party.broadcastToMembers(new PartySmallWindowUpdate((L2Player)this));
                        }
                    }
                }
                else
                {
                    if (result.TotalUI == 1)
                        BroadcastUserInfo();
                }
            }
        }

        public void RemoveStats(L2Item item)
        {
            if (item.Template.stats.Count > 0)
            {
                TEffectResult result = CharacterStat.Stop(item.Template.stats, this);
                if (this is L2Player)
                {
                    if (result.TotalUI == 1)
                        BroadcastUserInfo();
                    else
                    {
                        if (result.sus != null)
                        {
                            StatusUpdate su = new StatusUpdate(ObjId);
                            foreach (int stat in result.sus.Keys)
                                su.add(stat, (int)result.sus[stat]);

                            BroadcastPacket(su, false);

                            if (this is L2Player && (result.HpMpCp == 1) && (((L2Player)this).Party != null))
                                ((L2Player)this).Party.broadcastToMembers(new PartySmallWindowUpdate((L2Player)this));
                        }
                    }
                }
                else
                {
                    if (result.TotalUI == 1)
                        BroadcastUserInfo();
                }
            }
        }

        public void RemoveStat(TEffect effect)
        {
            List<TEffect> ts = new List<TEffect>();
            ts.Add(effect);
            TEffectResult result = CharacterStat.Stop(ts, this);
            if (this is L2Player)
            {
                if (result.TotalUI == 1)
                    BroadcastUserInfo();
                else
                {
                    if (result.sus != null)
                    {
                        StatusUpdate su = new StatusUpdate(ObjId);
                        foreach (int stat in result.sus.Keys)
                            su.add(stat, (int)result.sus[stat]);

                        BroadcastPacket(su, false);

                        if (this is L2Player && (result.HpMpCp == 1) && (((L2Player)this).Party != null))
                            ((L2Player)this).Party.broadcastToMembers(new PartySmallWindowUpdate((L2Player)this));
                    }
                }
            }
            else
            {
                if (result.TotalUI == 1)
                    BroadcastUserInfo();
            }
        }

        private void AddStats(TSkill skill, L2Character caster)
        {
            if (skill.effects == null)
                return;

            if (skill.effects.Count > 0)
            {
                TEffectResult result = CharacterStat.Apply(skill.effects, caster);
                if (this is L2Player)
                {
                    if (result.TotalUI == 1)
                        BroadcastUserInfo();
                    else
                    {
                        if (result.sus != null)
                        {
                            StatusUpdate su = new StatusUpdate(ObjId);
                            foreach (int stat in result.sus.Keys)
                                su.add(stat, (int)result.sus[stat]);

                            BroadcastPacket(su, false);

                            if (this is L2Player && (result.HpMpCp == 1) && (((L2Player)this).Party != null))
                                ((L2Player)this).Party.broadcastToMembers(new PartySmallWindowUpdate((L2Player)this));
                        }
                    }
                }
                else
                {
                    if (result.TotalUI == 1)
                        BroadcastUserInfo();
                }
            }
        }

        public void AddStats(L2Item item)
        {
            if (item.Template.stats.Count > 0)
            {
                TEffectResult result = CharacterStat.Apply(item.Template.stats, this);
                if (this is L2Player)
                {
                    if (result.TotalUI == 1)
                        BroadcastUserInfo();
                    else
                    {
                        if (result.sus != null)
                        {
                            StatusUpdate su = new StatusUpdate(ObjId);
                            foreach (int stat in result.sus.Keys)
                                su.add(stat, (int)result.sus[stat]);

                            BroadcastPacket(su, false);

                            if (this is L2Player && (result.HpMpCp == 1) && (((L2Player)this).Party != null))
                                ((L2Player)this).Party.broadcastToMembers(new PartySmallWindowUpdate((L2Player)this));
                        }
                    }
                }
                else
                {
                    if (result.TotalUI == 1)
                        BroadcastUserInfo();
                }
            }
        }

        public void AddStat(TEffect effect)
        {
            List<TEffect> ts = new List<TEffect>();
            ts.Add(effect);
            TEffectResult result = CharacterStat.Apply(ts, this);
            if (this is L2Player)
            {
                if (result.TotalUI == 1)
                    BroadcastUserInfo();
                else
                {
                    if (result.sus != null)
                    {
                        StatusUpdate su = new StatusUpdate(ObjId);
                        foreach (int stat in result.sus.Keys)
                            su.add(stat, (int)result.sus[stat]);

                        BroadcastPacket(su, false);

                        if (this is L2Player && (result.HpMpCp == 1) && (((L2Player)this).Party != null))
                            ((L2Player)this).Party.broadcastToMembers(new PartySmallWindowUpdate((L2Player)this));
                    }
                }
            }
            else
            {
                if (result.TotalUI == 1)
                    BroadcastUserInfo();
            }
        }

        public void StopEffect(TSkill skill, L2Character caster)
        {
            AbnormalEffect ex = Effects.FirstOrDefault(e => e.id == skill.skill_id);

            if (ex != null)
                lock (Effects)
                {
                    ex.forcedStop(true, true);
                    Effects.Remove(ex);
                }
        }

        public void OnAveEnd(AbnormalEffect ave, bool msg, bool icon, L2Character caster)
        {
            int olda = AbnormalBitMask;
            if (ave.skill.abnormal_visual_effect != -1)
                AbnormalBitMask &= ~ave.skill.abnormal_visual_effect;

            bool uis = (AbnormalBitMask != olda);

            RemoveStats(ave.skill, caster);

            if (msg)
                SendPacket(new SystemMessage(SystemMessage.SystemMessageId.EFFECT_S1_DISAPPEARED).AddSkillName(ave.id, ave.lvl));

            if (uis)
                BroadcastUserInfo();

            if (icon)
                UpdateMagicEffectIcons();
        }

        public virtual void UpdateMagicEffectIcons() { }

        public virtual void UpdateSkillList() { }

        public virtual void SendMessage(string p) { }

        public virtual void SendActionFailed() { }

        public virtual void SendSystemMessage(SystemMessage.SystemMessageId msgId) { }

        public virtual void OnPickUp(L2Item item) { }

        public int BuffMax = Config.Config.Instance.GameplayConfig.Player.Buff.MaxBuffsAmount;
        public LinkedList<AbnormalEffect> Effects = new LinkedList<AbnormalEffect>();

        public override void AddAbnormal(TSkill skill, L2Character caster, bool permanent, bool unlim)
        {
            if (!permanent)
                if (skill.debuff == 1)
                {
                    //const bool success = true; // TODO _stats.calcDebuffSuccess(skill, caster);

                    //if (!success)
                    //{
                    //    caster.sendPacket(new SystemMessage(SystemMessage.SystemMessageId.S1_RESISTED_YOUR_S2).AddString(Name).AddSkillName(skill.skill_id, skill.level));

                    //    sendPacket(new SystemMessage(SystemMessage.SystemMessageId.RESISTED_S1_MAGIC).AddString(caster.Name));
                    //    return;
                    //}
                }

            if (skill.abnormal_time == 0)
            {
                OnAveStart(skill, caster);
                return;
            }

            bool cnew = true;

            List<AbnormalEffect> nulled = new List<AbnormalEffect>();
            foreach (AbnormalEffect ave in Effects.Where(ave => ave.active != 0))
            {
                if ((ave.skill.skill_id == skill.skill_id) && (ave.skill.level >= skill.level))
                {
                    cnew = false;
                    break;
                }

                if (((ave.skill.abnormal_type != null) && (skill.abnormal_type != null)) && ave.skill.abnormal_type.Equals(skill.abnormal_type))
                {
                    if (ave.skill.effect_point > skill.effect_point)
                    {
                        cnew = false;
                        break;
                    }

                    nulled.Add(ave);
                }
            }

            lock (Effects)
            {
                foreach (AbnormalEffect ei in nulled)
                {
                    ei.forcedStop(false, false);
                    Effects.Remove(ei);
                }
            }

            nulled.Clear();

            if (!cnew)
                return;

            AbnormalEffect ic = new AbnormalEffect();
            ic.id = skill.skill_id;
            ic.lvl = skill.level;
            ic.time = unlim ? -2 : skill.abnormal_time;
            ic.active = 1;
            ic._owner = this;
            ic.skill = skill;
            ic.timer();

            if (Effects.Count >= BuffMax)
            {
                int id = 1;
                foreach (AbnormalEffect ave in Effects)
                {
                    if (id == 1)
                    {
                        ave.forcedStop(false, false);
                        break;
                    }

                    id++;
                }

                lock (Effects)
                {
                    Effects.RemoveFirst();
                }
            }

            Effects.AddLast(ic);
            OnAveStart(skill, caster);

            {
                SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.YOU_FEEL_S1_EFFECT);
                sm.AddSkillName(ic.id, ic.lvl);
                SendPacket(sm);
            }

            UpdateMagicEffectIcons();
        }

        public void AddAbnormalSpa(int skillId, bool unlim)
        {
            bool addNew = true;
            int lvlnext = 1;
            List<AbnormalEffect> nulled = new List<AbnormalEffect>();
            foreach (AbnormalEffect ave in Effects.Where(ave => (ave.active != 0) && (ave.skill.skill_id == skillId)))
            {
                addNew = false;
                if (ave.skill.level <= 10)
                {
                    addNew = true;
                    lvlnext = ave.skill.level + 1;
                    nulled.Add(ave);
                    break;
                }
            }

            if (nulled.Count > 0)
            {
                lock (Effects)
                {
                    foreach (AbnormalEffect ei in nulled)
                    {
                        ei.forcedStop(false, false);
                        Effects.Remove(ei);
                    }
                }

                nulled.Clear();
            }

            if (!addNew)
                return;

            TSkill newsk = TSkillTable.Instance.Get(skillId, lvlnext);
            AbnormalEffect ic = new AbnormalEffect();
            ic.id = newsk.skill_id;
            ic.lvl = newsk.level;
            ic.time = unlim ? -2 : newsk.abnormal_time;
            ic.active = 1;
            ic._owner = this;
            ic.skill = newsk;
            ic.timer();

            if (Effects.Count >= BuffMax)
            {
                int id = 1;
                foreach (AbnormalEffect ave in Effects)
                {
                    if (id == 1)
                    {
                        ave.forcedStop(false, false);
                        break;
                    }

                    id++;
                }

                lock (Effects)
                {
                    Effects.RemoveFirst();
                }
            }

            Effects.AddLast(ic);
            OnAveStart(newsk, null);

            UpdateMagicEffectIcons();
        }

        public void OnAveStart(TSkill skill, L2Character caster)
        {
            int olda = AbnormalBitMask;
            if (skill.abnormal_visual_effect != -1)
                AbnormalBitMask |= skill.abnormal_visual_effect;

            bool uis = (AbnormalBitMask != olda);

            AddStats(skill, caster);

            if (uis)
                BroadcastUserInfo();
        }

        public virtual void AddEffects(L2Character caster, TSkill skill, SortedList<int, L2Object> objects)
        {
            foreach (L2Object target in objects.Values)
                target.AddAbnormal(skill, caster, false, false);
        }

        public virtual void AddEffect(L2Character caster, TSkill skill, bool permanent, bool unlim)
        {
            AddAbnormal(skill, caster, permanent, unlim);
        }

        public int ClientPosX,
                   ClientPosY,
                   ClientPosZ,
                   ClientHeading;

        public virtual void Teleport(int x, int y, int z)
        {
            ChangeTarget();
            //clearKnowns(true);
            X = x;
            Y = y;
            Z = z;
            if (this is L2Player)
            {
                if (((L2Player)this).Summon != null)
                {
                    L2Player pl = (L2Player)this;
                    pl.Summon.Teleport(x, y, z);
                    pl.Summon.isTeleporting = true;
                }

                BroadcastPacket(new TeleportToLocation(ObjId, x, y, z, Heading));
            }
        }

        public int GetSkillLevel(int id)
        {
            if (Skills != null)
                lock (Skills)
                {
                    if (Skills.ContainsKey(id))
                        return Skills[id].level;
                }

            return -1;
        }

        private Timer _waterTimer;
        private DateTime _waterTimeDamage;
        //private bool lastInsideWater = false;

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

                if (next)
                {
                    int breath = (int)CharacterStat.getStat(TEffectType.b_breath);
                    _waterTimeDamage = DateTime.Now.AddSeconds(breath);
                    _waterTimer.Enabled = true;

                    if (this is L2Player)
                        SendPacket(new SetupGauge(ObjId, SetupGauge.SG_color.cyan, breath * 1000));
                }
            }
            else
            {
                if ((_waterTimer != null) && _waterTimer.Enabled)
                {
                    _waterTimer.Enabled = false;

                    if (this is L2Player)
                        SendPacket(new SetupGauge(ObjId, SetupGauge.SG_color.cyan, 1));
                }
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
            if (ts.TotalMilliseconds < 0) //дыхалка кончилась. лупим по ушам
                if (this is L2Player)
                    ReduceHpArea(200, 297);
        }

        public void ReduceHpArea(int damage, int msgId)
        {
            if (Dead)
                return;

            CurHp -= damage;

            StatusUpdate su = new StatusUpdate(ObjId);
            su.add(StatusUpdate.CUR_HP, (int)CurHp);
            su.add(StatusUpdate.MAX_HP, (int)CharacterStat.getStat(TEffectType.b_max_hp));
            BroadcastPacket(su);

            if (CurHp <= 0)
            {
                Dead = true;
                CurHp = 0;
                DoDie(null, true);
                return;
            }

            if (this is L2Player)
                SendPacket(new SystemMessage((SystemMessage.SystemMessageId)msgId).AddNumber(damage));
        }

        public override void ReduceHp(L2Character attacker, double damage)
        {
            if (Dead)
                return;

            if ((this is L2Player && attacker is L2Player) || attacker is L2Summon)
                if (CurCp > 0)
                {
                    CurCp -= damage;

                    if (CurCp < 0)
                    {
                        damage = CurCp * -1;
                        CurCp = 0;
                    }
                }

            CurHp -= damage;

            StatusUpdate su = new StatusUpdate(ObjId);
            su.add(StatusUpdate.CUR_HP, (int)CurHp);
            su.add(StatusUpdate.CUR_CP, (int)CurCp);
            BroadcastPacket(su);

            if (CurHp <= 0)
            {
                CurHp = 0;
                DoDie(attacker, false);
                return;
            }

            AiCharacter.NotifyOnHit(attacker, damage);
        }

        public virtual void DoDie(L2Character killer, bool bytrigger)
        {
            Dead = true;
            StopRegeneration();
            if (IsAttacking())
                AbortAttack();

            if (IsCastingNow())
                AbortCast();

            CurHp = 0;
            StatusUpdate su = new StatusUpdate(ObjId);
            su.add(StatusUpdate.CUR_HP, 0);
            BroadcastPacket(su);

            SendMessage("You died from " + killer.Name);
            killer.SendMessage("You killed " + Name);
            BroadcastPacket(new Die(this));

            UpdateMagicEffectIcons();

            killer.AiCharacter.NotifyOnKill(this);
            AiCharacter.NotifyOnDie(killer);

            AiCharacter.Disable();
        }

        public virtual void DeleteByForce()
        {
            if (AiCharacter != null)
                AiCharacter.Disable();

            StopRegeneration();

            foreach (AbnormalEffect a in Effects)
                a._timer.Enabled = false;

            Effects.Clear();

            BroadcastPacket(new DeleteObject(ObjId));
            L2World.Instance.RemoveObject(this);
        }

        public virtual L2Item ActiveWeapon
        {
            get { return null; }
            set { }
        }

        public virtual L2Item SecondaryWeapon
        {
            get { return null; }
        }

        public virtual L2Item ActiveArmor
        {
            get { return null; }
            set { }
        }

        public L2Character CurrentTarget;

        public virtual void DoAttack(L2Character target)
        {
            if (target == null)
            {
                AiCharacter.NotifyTargetNull();
                return;
            }

            if (target.Dead)
            {
                AiCharacter.NotifyTargetDead();
                return;
            }

            if ((AttackToHit != null) && AttackToHit.Enabled)
                return;

            if ((AttackToEnd != null) && AttackToEnd.Enabled)
                return;

            double dist = 60,
                   reqMp = 0;

            L2Item weapon = ActiveWeapon;
            double timeAtk = CharacterStat.getStat(TEffectType.b_attack_spd);
            bool dual = false,
                 ranged = false,
                 ss = false;
            if (weapon != null)
            {
                ss = weapon.Soulshot;
                switch (weapon.Template.WeaponType)
                {
                    case ItemTemplate.L2ItemWeaponType.bow:
                        timeAtk = (1500 * 345 / timeAtk);
                        ranged = true;
                        break;
                    case ItemTemplate.L2ItemWeaponType.crossbow:
                        timeAtk = (1200 * 345 / timeAtk);
                        ranged = true;
                        break;
                    case ItemTemplate.L2ItemWeaponType.dualdagger:
                    case ItemTemplate.L2ItemWeaponType.dual:
                        timeAtk = (1362 * 345 / timeAtk);
                        dual = true;
                        break;
                    default:
                        timeAtk = (1362 * 345 / timeAtk);
                        break;
                }

                if ((weapon.Template.WeaponType == ItemTemplate.L2ItemWeaponType.crossbow) || (weapon.Template.WeaponType == ItemTemplate.L2ItemWeaponType.bow))
                {
                    dist += 740;
                    reqMp = weapon.Template.MpConsume;
                }
            }
            else
            {
                timeAtk = (1362 * 345 / timeAtk);
            }

            if (!Calcs.checkIfInRange((int)dist, this, target, true))
            {
                TryMoveTo(target.X, target.Y, target.Z);
                return;
            }

            if ((reqMp > 0) && (reqMp > CurMp))
            {
                SendMessage("no mp " + CurMp + " " + reqMp);
                AiCharacter.NotifyMpEnd(target);
                return;
            }

            Attack atk = new Attack(this, target, ss, 5);

            if (dual)
            {
                Hit1 = GenHitSimple(true, ss);
                atk.addHit(target.ObjId, (int)Hit1.Damage, Hit1.Miss, Hit1.Crit, Hit1.ShieldDef > 0);

                Hit2 = GenHitSimple(true, ss);
                atk.addHit(target.ObjId, (int)Hit2.Damage, Hit2.Miss, Hit2.Crit, Hit2.ShieldDef > 0);
            }
            else
            {
                Hit1 = GenHitSimple(false, ss);
                atk.addHit(target.ObjId, (int)Hit1.Damage, Hit1.Miss, Hit1.Crit, Hit1.ShieldDef > 0);
            }

            CurrentTarget = target;

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
            Hit h = new Hit {Miss = Formulas.checkMissed(this, CurrentTarget)};
            if (!h.Miss)
            {
                h.ShieldDef = Formulas.checkShieldDef(this, CurrentTarget);
                h.Crit = Formulas.checkCrit(this, CurrentTarget);
                h.Damage = Formulas.getPhysHitDamage(this, CurrentTarget, 0);
                if (dual)
                    h.Damage *= .5;
                if (ss)
                    h.Damage *= 2;
                if (h.Crit)
                    h.Damage *= 2;
            }

            return h;
        }

        public virtual void AttackDoHit(object sender, ElapsedEventArgs e)
        {
            if (CurrentTarget != null)
                if (!Hit1.Miss)
                {
                    CurrentTarget.ReduceHp(this, Hit1.Damage);

                    if (CurrentTarget is L2Player)
                        CurrentTarget.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1_HAS_RECEIVED_S3_DAMAGE_FROM_C2).AddName(CurrentTarget).AddName(this).AddNumber(Hit1.Damage));
                }
                else
                {
                    if (CurrentTarget is L2Player)
                    {
                        CurrentTarget.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1_HAS_EVADED_C2_ATTACK).AddName(CurrentTarget).AddName(this));
                        ((L2Player)CurrentTarget).AiCharacter.NotifyEvaded(this);
                    }
                }

            AttackToHit.Enabled = false;
        }

        public virtual void AttackDoHit2Nd(object sender, ElapsedEventArgs e)
        {
            if (CurrentTarget != null)
                if (!Hit2.Miss)
                {
                    CurrentTarget.ReduceHp(this, Hit2.Damage);
                    if (CurrentTarget is L2Player)
                        CurrentTarget.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1_HAS_RECEIVED_S3_DAMAGE_FROM_C2).AddName(CurrentTarget).AddName(this).AddNumber(Hit2.Damage));
                }
                else
                {
                    if (CurrentTarget is L2Player)
                    {
                        CurrentTarget.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1_HAS_EVADED_C2_ATTACK).AddName(CurrentTarget).AddName(this));
                        ((L2Player)CurrentTarget).AiCharacter.NotifyEvaded(this);
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

            // if (CurrentTarget != null)
            //    doAttack((L2Character)CurrentTarget);
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

            if (skillId > 0)
            {
                BroadcastPacket(new MagicSkillUse(this, this, skillId, 1, 0));
                SendSystemMessage(SystemMessage.SystemMessageId.ENABLED_SOULSHOT);
            }
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

            if (IsCastingNow())
                return true;

            return false;
        }

        public void TryMoveTo(int x, int y, int z)
        {
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

        public void Status_FreezeMe(bool status, bool update)
        {
            if (status)
                AbnormalBitMaskEx |= AbnormalMaskExFreezing;
            else
                AbnormalBitMaskEx &= ~AbnormalMaskExFreezing;

            if (update)
                UpdateAbnormalExEffect();
        }

        public CStats CharacterStat;

        public void CStatsInit()
        {
            if (CharacterStat == null)
                CharacterStat = new CStats(this);
        }

        public void ChangeTarget(L2Character target = null)
        {
            if (target == null)
            {
                BroadcastPacket(new TargetUnselected(this));
                CurrentTarget = null;
            }
            else
            {
                if (CurrentTarget != null)
                    if (CurrentTarget.ObjId != target.ObjId)
                    {
                        BroadcastPacket(new TargetUnselected(this));
                    }
                    else
                    {
                        OnOldTargetSelection(target);
                        return;
                    }

                CurrentTarget = target;

                BroadcastPacket(new TargetSelected(ObjId, target));
                OnNewTargetSelection(target);
            }
        }

        public virtual void OnOldTargetSelection(L2Object target) { }

        public virtual void OnNewTargetSelection(L2Object target) { }

        public override void RegenUpdateTaskDone(object sender, ElapsedEventArgs e)
        {
            bool hp = CurHp < CharacterStat.getStat(TEffectType.b_max_hp),
                 mp = CurMp < CharacterStat.getStat(TEffectType.b_max_mp),
                 cp = false;

            if (this is L2Player)
                cp = CurCp < CharacterStat.getStat(TEffectType.b_max_cp);

            if (hp || mp || cp)
            {
                StatusUpdate su = new StatusUpdate(ObjId);
                if (hp)
                    su.add(StatusUpdate.CUR_HP, (int)CurHp);
                if (mp)
                    su.add(StatusUpdate.CUR_MP, (int)CurMp);
                if (cp)
                    su.add(StatusUpdate.CUR_CP, (int)CurCp);

                BroadcastPacket(su);

                if (this is L2Summon)
                    if (((L2Summon)this).Owner != null)
                        ((L2Summon)this).Owner.SendPacket(new PetStatusUpdate((L2Summon)this));

                if (this is L2Player)
                    if (((L2Player)this).Party != null)
                        ((L2Player)this).Party.broadcastToMembers(new PartySmallWindowUpdate((L2Player)this));
            }
        }

        public override void RegenTaskDone(object sender, ElapsedEventArgs e)
        {
            double maxhp = CharacterStat.getStat(TEffectType.b_max_hp);
            if (CurHp < maxhp)
            {
                CurHp += CharacterStat.getStat(TEffectType.b_reg_hp);
                if (CurHp > maxhp)
                    CurHp = maxhp;
            }

            double maxmp = CharacterStat.getStat(TEffectType.b_max_mp);
            if (CurMp < maxmp)
            {
                CurMp += CharacterStat.getStat(TEffectType.b_reg_mp);
                if (CurMp > maxmp)
                    CurMp = maxmp;
            }

            if (this is L2Player)
            {
                double maxcp = CharacterStat.getStat(TEffectType.b_max_cp);
                if (CurCp < maxcp)
                {
                    CurCp += CharacterStat.getStat(TEffectType.b_reg_cp);
                    if (CurCp > maxcp)
                        CurCp = maxcp;
                }
            }
        }

        public virtual bool IsCastingNow()
        {
            return (CastTime != null) && CastTime.Enabled;
        }

        public virtual void AbortCast()
        {
            if (CastTime != null)
                CastTime.Enabled = false;

            BroadcastPacket(new MagicSkillCanceld(ObjId));
            CurrentCast = null;
        }

        public TSkill CurrentCast;
        public Timer CastTime;

        public SortedList<int, L2SkillCoolTime> Reuse = new SortedList<int, L2SkillCoolTime>();

        public int CastSkill(TSkill skill)
        {
            if (IsCastingNow())
                return 1;

            L2Object target = skill.getTargetCastId(this);

            if (target == null)
                return 2;

            if (skill.cast_range != -1)
            {
                double dis = Calcs.calculateDistance(this, target, true);
                if (dis > skill.cast_range)
                    return 3;
            }

            if (skill.reuse_delay > 0)
                if (Reuse.ContainsKey(skill.skill_id))
                {
                    TimeSpan ts = Reuse[skill.skill_id].stopTime - DateTime.Now;

                    if (ts.TotalMilliseconds > 0)
                        return 4;
                }

            if ((skill.mp_consume1 > 0) || (skill.mp_consume2 > 0))
                if (CurMp < skill.mp_consume1 + skill.mp_consume2)
                    return 5;

            if (skill.hp_consume > 0)
                if (CurHp < skill.hp_consume)
                    return 6;

            if (skill.effects.Count > 0)
            {
                bool fail = skill.effects.Any(ef => !ef.canUse(this));

                if (fail)
                    return 7;
            }

            if (skill.reuse_delay > 0)
            {
                L2SkillCoolTime reuse = new L2SkillCoolTime();
                reuse.id = skill.skill_id;
                reuse.lvl = skill.level;
                reuse.total = (int)skill.reuse_delay;
                reuse.delay = reuse.total;
                reuse._owner = this;
                reuse.timer();
                Reuse.Add(reuse.id, reuse);
            }

            {
                //SystemMessage sm = new SystemMessage(46); //You use $s1.
                //sm.addSkillName(skill.ClientID, skill.Level);
                //sendPacket(sm);
                //TODO nearby objects notify
            }

            if (skill.hp_consume > 0)
            {
                CurHp -= skill.hp_consume;

                StatusUpdate su = new StatusUpdate(ObjId);
                su.add(StatusUpdate.CUR_HP, (int)CurHp);
                BroadcastPacket(su);
            }

            if (skill.mp_consume1 > 0)
            {
                CurMp -= skill.mp_consume1;

                StatusUpdate su = new StatusUpdate(ObjId);
                su.add(StatusUpdate.CUR_MP, (int)CurMp);
                BroadcastPacket(su);
            }

            int hitTime = skill.skill_hit_time;

            int hitT = hitTime > 0 ? (int)(hitTime * 0.95) : 0;
            CurrentCast = skill;

            BroadcastPacket(new MagicSkillUse(this, target, skill, hitTime == 0 ? 20 : hitTime));

            if (hitTime > 50)
            {
                if (CastTime == null)
                {
                    CastTime = new Timer();
                    CastTime.Elapsed += CastEnd;
                }

                CastTime.Interval = hitT;
                CastTime.Enabled = true;
            }
            else
                CastEnd();

            return -1;
        }

        private void CastEnd(object sender = null, ElapsedEventArgs e = null)
        {
            if (CurrentCast.mp_consume2 > 0)
            {
                if (CurMp < CurrentCast.mp_consume2)
                {
                    CurrentCast = null;
                    CastTime.Enabled = false;
                    return;
                }

                CurMp -= CurrentCast.mp_consume2;

                StatusUpdate su = new StatusUpdate(ObjId);
                su.add(StatusUpdate.CUR_MP, (int)CurMp);
                BroadcastPacket(su);
            }

            if (CurrentCast.cast_range != -1)
            {
                bool block = false;
                if (CurrentTarget != null)
                {
                    double dis = Calcs.calculateDistance(this, CurrentTarget, true);
                    if (dis > CurrentCast.effective_range)
                        block = true;
                }
                else
                    block = true;

                if (block)
                {
                    CurrentCast = null;
                    CastTime.Enabled = false;
                    return;
                }
            }

            SortedList<int, L2Object> arr = CurrentCast.getAffectedTargets(this);
            List<int> broadcast = new List<int>();
            broadcast.AddRange(arr.Keys);

            BroadcastPacket(new MagicSkillLaunched(this, broadcast, CurrentCast.skill_id, CurrentCast.level));

            AddEffects(this, CurrentCast, arr);
            CurrentCast = null;
            if (CastTime != null)
                lock (CastTime)
                {
                    CastTime.Enabled = false;
                }
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
            if (IsAttacking())
                AbortAttack();

            if (_updatePositionTime == null)
            {
                _updatePositionTime = new Timer();
                _updatePositionTime.Interval = 100;
                _updatePositionTime.Elapsed += UpdatePositionTask;
            }
            else
            {
                if (_updatePositionTime.Enabled) // новый маршрут, но старый не закончен
                    NotifyStopMove(false);
            }

            DestX = x;
            DestY = y;
            DestZ = z;

            double dx = (x - X),
                   dy = (y - Y);
            //dz = (z - Z);
            double distance = Math.Sqrt(dx * dx + dy * dy);

            double speed = CharacterStat.getStat(TEffectType.p_speed);
            double spy = dy / distance,
                   spx = dx / distance;

            _ticksToMove = 1 + (int)(10 * distance / speed);
            _ticksToMoveCompleted = 0;
            _xSpeedTicks = (DestX - X) / _ticksToMove;
            _ySpeedTicks = (DestY - Y) / _ticksToMove;

            Heading = (int)((Math.Atan2(-spx, -spy) * 10430.378) + short.MaxValue);

            BroadcastPacket(new CharMoveToLocation(this));

            _updatePositionTime.Enabled = true;

            AiCharacter.NotifyStartMoving();
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
            {
                _updatePositionTime.Enabled = false;

                if (broadcast)
                    AiCharacter.NotifyStopMoving();

                if (update)
                    BroadcastPacket(new StopMove(this));
            }

            DestX = 0;
            DestY = 0;
            DestZ = 0;
            _xSpeedTicks = 0;
            _ySpeedTicks = 0;
            _ticksToMove = 0;
        }

        public virtual void NotifyArrived()
        {
            _updatePositionTime.Enabled = false;

            DestX = 0;
            DestY = 0;
            DestZ = 0;
            _xSpeedTicks = 0;
            _ySpeedTicks = 0;
            _ticksToMove = 0;

            AiCharacter.NotifyStopMoving();
        }

        private Timer _updatePositionTime;
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

        public virtual int ClanId
        {
            get { return 0; }
            set { }
        }

        public virtual int ClanCrestId => 0;

        public virtual int AllianceId => 0;

        public virtual int AllianceCrestId => 0;

        public virtual int MaxHp { get; set; }
        public virtual int MaxCp { get; set; }
        public virtual int MaxMp { get; set; }

        public override double CurHp { get; set; }

        public override double CurMp { get; set; }

        public override double CurCp { get; set; }

        public override string AsString()
        {
            return "L2Character:" + ObjId;
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
            else if (list != null)
                list.Remove(hashId);
        }

        public bool MutedPhysically
        {
            get { return (_muted0 != null) && (_muted0.Count > 0); }
        }

        public bool MutedMagically
        {
            get { return (_muted1 != null) && (_muted1.Count > 0); }
        }

        public bool MutedSpecial
        {
            get { return (_muted2 != null) && (_muted2.Count > 0); }
        }
    }
}