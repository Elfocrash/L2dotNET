using System;
using System.Collections.Generic;
using System.Timers;
using L2dotNET.Game.model.items;
using L2dotNET.Game.model.playable;
using L2dotNET.Game.model.skills2;
using L2dotNET.Game.model.stats;
using L2dotNET.Game.network.l2send;
using L2dotNET.Game.tools;
using L2dotNET.Game.model.playable.petai;
using L2dotNET.Game.Enums;

namespace L2dotNET.Game.world
{
    public class L2Character : L2Object
    {
        public SortedList<int, TSkill> _skills = new SortedList<int, TSkill>();

        public virtual string Name { get; set; }
        public virtual string Title { get; set; }
        public int SpawnX { get; set; }
        public int SpawnY { get; set; }
        public int SpawnZ { get; set; }
        private byte[] _zones = new byte[ZoneId.GetZoneCount()];

        public byte IsRunning { get; set; } = 1;

        public int AbnormalBitMask;
        public int AbnormalBitMaskEx;
        public int AbnormalBitMaskEvent;

        public const int AbnormalMaskBleed              = 0x000001;

        public const int AbnormalMaskExInvincible       = 0x000001;
        public const int AbnormalMaskExAirStun          = 0x000002;
        public const int AbnormalMaskExAirRoot          = 0x000004;
        public const int AbnormalMaskExBagSword         = 0x000008;
        public const int AbnormalMaskExAfroYellow       = 0x000010;
        public const int AbnormalMaskExAfroPink         = 0x000020;
        public const int AbnormalMaskExAfroBlack        = 0x000040;
        //unk x80
        public const int AbnormalMaskExStigmaShillien   = 0x000100;
        public const int AbnormalMaskExStakatoRoot      = 0x000200;
        public const int AbnormalMaskExFreezing         = 0x000400;
        public const int AbnormalMaskExVesper           = 0x000800;

        public const int AbnormalMaskEventIceHand       = 0x000008;
        public const int AbnormalMaskEventHeadphone     = 0x000010;
        public const int AbnormalMaskEventCrown1        = 0x000020;
        public const int AbnormalMaskEventCrown2        = 0x000040;
        public const int AbnormalMaskEventCrown3        = 0x000080;

        public virtual void updateAbnormalEffect() { }
        public virtual void updateAbnormalExEffect() { }
        public virtual void updateAbnormalEventEffect() { }

        public StandartAiTemplate AICharacter = new StandartAiTemplate();

        public override void onAction(L2Player player)
        {
            bool newtarget = false;
            if (player.CurrentTarget == null)
            {
                player.CurrentTarget = this;
                newtarget = true;
            }
            else
            {
                if (player.CurrentTarget.ObjID != this.ObjID)
                {
                    player.CurrentTarget = this;
                    newtarget = true;
                }
            }

            if (newtarget)
            {
                player.sendPacket(new MyTargetSelected(this.ObjID, 0));
            }
        }

        public override void onForcedAttack(L2Player player)
        {
            bool newtarget = false;
            if (player.CurrentTarget == null)
            {
                player.CurrentTarget = this;
                newtarget = true;
            }
            else
            {
                if (player.CurrentTarget.ObjID != this.ObjID)
                {
                    player.CurrentTarget = this;
                    newtarget = true;
                }
            }

            if (newtarget)
            {
                player.sendPacket(new MyTargetSelected(this.ObjID, 0));
            }
            else
                player.sendActionFailed();
        }

        public void addSkill(int id, int lvl, bool updDb, bool update)
        {
            TSkill skill = TSkillTable.Instance.Get(id, lvl);
            if (skill != null)
                addSkill(skill, updDb, update);
        }

        public virtual void addSkill(TSkill newsk, bool updDb, bool update)
        {
            lock (_skills)
            {
                if (_skills.ContainsKey(newsk.skill_id))
                {
                    if (newsk.OpType == TSkillOperational.P)
                        removeStats(_skills[newsk.skill_id], this);

                    _skills.Remove(newsk.skill_id);
                }
            }

            _skills.Add(newsk.skill_id, newsk);

            if (newsk.OpType == TSkillOperational.P)
                addStats(newsk, this);
        }

        public void SetInsisdeZone(ZoneId zone, bool state)
        {
            if (state)
                _zones[(int)zone.Id]++;
            else
            {
                _zones[(int)zone.Id]--;
                if (_zones[(int)zone.Id] < 0)
                    _zones[(int)zone.Id] = 0;
            }
        }

        public virtual void removeSkill(int id, bool updDb, bool update)
        {
            lock (_skills)
            {
                if (_skills.ContainsKey(id))
                {
                    if (_skills[id].OpType == TSkillOperational.P)
                        removeStats(_skills[id], this);

                    _skills.Remove(id);
                }
            }
        }

        private void removeStats(TSkill skill, L2Character caster)
        {
            if (skill.effects.Count > 0)
            {
                TEffectResult result = CharacterStat.Stop(skill.effects, caster);
                if (this is L2Player)
                {
                    if (result.TotalUI == 1)
                        broadcastUserInfo();
                    else
                    {
                        if (result.sus != null)
                        {
                            StatusUpdate su = new StatusUpdate(ObjID);
                            foreach (int stat in result.sus.Keys)
                                su.add(stat, (int)result.sus[stat]);

                            broadcastPacket(su, false);

                            if (this is L2Player && result.HpMpCp == 1 && ((L2Player)this).Party != null)
                                ((L2Player)this).Party.broadcastToMembers(new PartySmallWindowUpdate((L2Player)this));
                        }
                    }
                }
                else
                {
                    if (result.TotalUI == 1)
                        broadcastUserInfo();
                }
            }
        }

        public void removeStats(L2Item item)
        {
            if (item.Template.stats.Count > 0)
            {
                TEffectResult result = CharacterStat.Stop(item.Template.stats, this);
                if (this is L2Player)
                {
                    if (result.TotalUI == 1)
                        broadcastUserInfo();
                    else
                    {
                        if (result.sus != null)
                        {
                            StatusUpdate su = new StatusUpdate(ObjID);
                            foreach (int stat in result.sus.Keys)
                                su.add(stat, (int)result.sus[stat]);

                            broadcastPacket(su, false);

                            if (this is L2Player && result.HpMpCp == 1 && ((L2Player)this).Party != null)
                                ((L2Player)this).Party.broadcastToMembers(new PartySmallWindowUpdate((L2Player)this));
                        }
                    }
                }
                else
                {
                    if (result.TotalUI == 1)
                        broadcastUserInfo();
                }
            }
        }

        public void removeStat(TEffect effect)
        {
            List<TEffect> ts = new List<TEffect>();
            ts.Add(effect);
            TEffectResult result = CharacterStat.Stop(ts, this);
            if (this is L2Player)
            {
                if (result.TotalUI == 1)
                    broadcastUserInfo();
                else
                {
                    if (result.sus != null)
                    {
                        StatusUpdate su = new StatusUpdate(ObjID);
                        foreach (int stat in result.sus.Keys)
                            su.add(stat, (int)result.sus[stat]);

                        broadcastPacket(su, false);

                        if (this is L2Player && result.HpMpCp == 1 && ((L2Player)this).Party != null)
                            ((L2Player)this).Party.broadcastToMembers(new PartySmallWindowUpdate((L2Player)this));
                    }
                }
            }
            else
            {
                if (result.TotalUI == 1)
                    broadcastUserInfo();
            }
        }

        private void addStats(TSkill skill, L2Character caster)
        {
            if (skill.effects == null)
                return;

            if (skill.effects.Count > 0)
            {
                TEffectResult result = CharacterStat.Apply(skill.effects, caster);
                if (this is L2Player)
                {
                    if (result.TotalUI == 1)
                        broadcastUserInfo();
                    else
                    {
                        if (result.sus != null)
                        {
                            StatusUpdate su = new StatusUpdate(ObjID);
                            foreach (int stat in result.sus.Keys)
                                su.add(stat, (int)result.sus[stat]);

                            broadcastPacket(su, false);

                            if (this is L2Player && result.HpMpCp == 1 && ((L2Player)this).Party != null)
                                ((L2Player)this).Party.broadcastToMembers(new PartySmallWindowUpdate((L2Player)this));
                        }
                    }
                }
                else
                {
                    if (result.TotalUI == 1)
                        broadcastUserInfo();
                }
            }
        }

        public void addStats(L2Item item)
        {
            if (item.Template.stats.Count > 0)
            {
                TEffectResult result = CharacterStat.Apply(item.Template.stats, this);
                if (this is L2Player)
                {
                    if (result.TotalUI == 1)
                        broadcastUserInfo();
                    else
                    {
                        if (result.sus != null)
                        {
                            StatusUpdate su = new StatusUpdate(ObjID);
                            foreach (int stat in result.sus.Keys)
                                su.add(stat, (int)result.sus[stat]);

                            broadcastPacket(su, false);

                            if (this is L2Player && result.HpMpCp == 1 && ((L2Player)this).Party != null)
                                ((L2Player)this).Party.broadcastToMembers(new PartySmallWindowUpdate((L2Player)this));
                        }
                    }
                }
                else
                {
                    if (result.TotalUI == 1)
                        broadcastUserInfo();
                }
            }
        }

        public void addStat(TEffect effect)
        {
            List<TEffect> ts = new List<TEffect>();
            ts.Add(effect);
            TEffectResult result = CharacterStat.Apply(ts, this);
            if (this is L2Player)
            {
                if (result.TotalUI == 1)
                    broadcastUserInfo();
                else
                {
                    if (result.sus != null)
                    {
                        StatusUpdate su = new StatusUpdate(ObjID);
                        foreach (int stat in result.sus.Keys)
                            su.add(stat, (int)result.sus[stat]);

                        broadcastPacket(su, false);

                        if (this is L2Player && result.HpMpCp == 1 && ((L2Player)this).Party != null)
                            ((L2Player)this).Party.broadcastToMembers(new PartySmallWindowUpdate((L2Player)this));
                    }
                }
            }
            else
            {
                if (result.TotalUI == 1)
                    broadcastUserInfo();
            }
        }

        public void stopEffect(TSkill skill, L2Character caster)
        {
            AbnormalEffect ex = null;
            foreach (AbnormalEffect e in _effects)
            {
                if (e.id == skill.skill_id)
                {
                    ex = e;
                    break;
                }
            }

            if (ex != null)
            {
                lock (_effects)
                {
                    ex.forcedStop(true, true);
                    _effects.Remove(ex);
                }
            }
        }

        public void onAveEnd(AbnormalEffect ave, bool msg, bool icon, L2Character caster)
        {
            int olda = AbnormalBitMask;
            if (ave.skill.abnormal_visual_effect != -1)
            {
                AbnormalBitMask &= ~ave.skill.abnormal_visual_effect;
            }

            bool uis = false;
            if (AbnormalBitMask != olda)
            {
                uis = true;
            }

            removeStats(ave.skill, caster);

            if (msg)
                sendPacket(new SystemMessage(749).AddSkillName(ave.id, ave.lvl));//The effect of $s1 has been removed.

            if (uis)
                broadcastUserInfo();

            if (icon)
                updateMagicEffectIcons();
        }

        public virtual void updateMagicEffectIcons() { }
        public virtual void updateSkillList() { }
        public virtual void sendMessage(string p) { }
        public virtual void sendActionFailed() { }
        public virtual void sendSystemMessage(int p) { }
        public virtual void onPickUp(L2Item item) { }

        public int _buffMax = Config.Instance.gameplayConfig.MaxBuffs;
        public LinkedList<AbnormalEffect> _effects = new LinkedList<AbnormalEffect>();

        public override void addAbnormal(TSkill skill, L2Character caster, bool permanent, bool unlim)
        {
            if (!permanent)
            {
                if (skill.debuff == 1)
                {
                    bool success = true;// TODO _stats.calcDebuffSuccess(skill, caster);

                    if (!success)
                    {
                        //$s1 has resisted your $s2.
                        caster.sendPacket(new SystemMessage(139).AddString(Name).AddSkillName(skill.skill_id, skill.level));
                        
                        //You have resisted $s1's magic.
                        sendPacket(new SystemMessage(159).AddString(caster.Name));
                        return;
                    }
                }
            }

            if (skill.abnormal_time == 0)
            {
                onAveStart(skill, caster);
                return;
            }

            bool cnew = true;

            List<AbnormalEffect> nulled = new List<AbnormalEffect>();
            foreach (AbnormalEffect ave in _effects)
            {
                if (ave.active == 0)
                    continue;

                if (ave.skill.skill_id == skill.skill_id && ave.skill.level >= skill.level)
                {
                    cnew = false;
                    break;
                }

                if ((ave.skill.abnormal_type != null && skill.abnormal_type != null) && ave.skill.abnormal_type.Equals(skill.abnormal_type))
                {
                    if (ave.skill.effect_point > skill.effect_point)
                    {
                        cnew = false;
                        break;
                    }
                    else
                    {
                        nulled.Add(ave);
                    }
                }
            }

            lock (_effects)
            {
                foreach (AbnormalEffect ei in nulled)
                {
                    ei.forcedStop(false, false);
                    _effects.Remove(ei);
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

            if (_effects.Count >= _buffMax)
            {
                int id = 1;
                foreach (AbnormalEffect ave in _effects)
                {
                    if (id == 1)
                    {
                        ave.forcedStop(false, false);
                        break;
                    } id++;
                }

                lock (_effects)
                {
                    _effects.RemoveFirst();
                }
            }

            _effects.AddLast(ic);
            onAveStart(skill, caster);

            {
                SystemMessage sm = new SystemMessage(110); //The effects of $s1 flow through you.
                sm.AddSkillName(ic.id, ic.lvl);
                sendPacket(sm);
            }

            updateMagicEffectIcons();
        }

        public void addAbnormalSPA(int skillId, bool unlim)
        {
            bool addNew = true;
            int lvlnext = 1;
            List<AbnormalEffect> nulled = new List<AbnormalEffect>();
            foreach (AbnormalEffect ave in _effects)
            {
                if (ave.active == 0)
                    continue;

                if (ave.skill.skill_id == skillId)
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
            }

            if (nulled.Count > 0)
            {
                lock (_effects)
                {
                    foreach (AbnormalEffect ei in nulled)
                    {
                        ei.forcedStop(false, false);
                        _effects.Remove(ei);
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

            if (_effects.Count >= _buffMax)
            {
                int id = 1;
                foreach (AbnormalEffect ave in _effects)
                {
                    if (id == 1)
                    {
                        ave.forcedStop(false, false);
                        break;
                    } id++;
                }

                lock (_effects)
                {
                    _effects.RemoveFirst();
                }
            }

            _effects.AddLast(ic);
            onAveStart(newsk, null);

            updateMagicEffectIcons();
        }

        public void onAveStart(TSkill skill, L2Character caster)
        {
            int olda = AbnormalBitMask;
            if (skill.abnormal_visual_effect != -1)
            {
                AbnormalBitMask |= skill.abnormal_visual_effect;
            }

            bool uis = false;
            if (AbnormalBitMask != olda)
            {
                uis = true;
            }

            addStats(skill, caster);

            if (uis)
                broadcastUserInfo();
        }

        public virtual void addEffects(L2Character caster, TSkill skill, SortedList<int, L2Object> objects)
        {
            foreach (L2Object target in objects.Values)
            {
                target.addAbnormal(skill, caster, false, false);
            }
        }

        public virtual void addEffect(L2Character caster, TSkill skill, bool permanent, bool unlim)
        {
            addAbnormal(skill, caster, permanent, unlim);
        }

        public int clientPosX, clientPosY, clientPosZ, clientHeading;

        public virtual void teleport(int x, int y, int z)
        {
            ChangeTarget();
            clearKnowns(true);
            X = x;
            Y = y;
            Z = z;
            if (this is L2Player)
            {
                if (((L2Player)this).Summon != null)
                {
                    L2Player pl = (L2Player)this;
                    pl.Summon.teleport(x, y, z);
                    pl.Summon.isTeleporting = true;
                }

                broadcastPacket(new TeleportToLocation(ObjID, x, y, z, Heading));
            }  
        }

        public int getSkillLevel(int id)
        {
            if (_skills.ContainsKey(id))
                return _skills[id].level;

            return -1;
        }

        System.Timers.Timer _waterTimer;
        DateTime _waterTimeDamage;
        private bool lastInsideWater = false;
        public void waterTimer()
        {
            if (isInWater())
            {
                bool next = false;
                if (_waterTimer == null || !_waterTimer.Enabled)
                {
                    _waterTimer = new System.Timers.Timer();
                    _waterTimer.Elapsed += new ElapsedEventHandler(waterActionTime);
                    _waterTimer.Interval = 3000;
                    next = true;
                }

                if (next)
                {
                    int breath = (int)CharacterStat.getStat(TEffectType.b_breath);
                    _waterTimeDamage = DateTime.Now.AddSeconds(breath);
                    _waterTimer.Enabled = true;

                    if (this is L2Player)
                    {
                        sendPacket(new SetupGauge(ObjID, SetupGauge.SG_color.cyan, breath * 1000));
                    }
                }
            }
            else
            {
                if (_waterTimer != null && _waterTimer.Enabled)
                {
                    _waterTimer.Enabled = false;

                    if (this is L2Player)
                    {
                        sendPacket(new SetupGauge(ObjID, SetupGauge.SG_color.cyan, 1));
                    }
                }
            }
            /// asd
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

        private void waterActionTime(object sender, ElapsedEventArgs e)
        {
            TimeSpan ts = _waterTimeDamage - DateTime.Now;
            if (ts.TotalMilliseconds < 0) //дыхалка кончилась. лупим по ушам
            {
                if (this is L2Player)
                {
                    reduceHpArea(200, 297);
                }
            }
        }

        public void reduceHpArea(int damage, int msgId)
        {
            if (Dead)
                return;

            CurHP -= damage;

            StatusUpdate su = new StatusUpdate(ObjID);
            su.add(StatusUpdate.CUR_HP, (int)CurHP);
            su.add(StatusUpdate.MAX_HP, (int)CharacterStat.getStat(TEffectType.b_max_hp));
            broadcastPacket(su);

            if (CurHP <= 0)
            {
                Dead = true;
                CurHP = 0;
                doDie(null, true);
                return;
            }

            if (this is L2Player)
                sendPacket(new SystemMessage(msgId).AddNumber((int)damage));
        }

        public override void reduceHp(L2Character attacker, double damage)
        {
            if (Dead)
                return;

            if (this is L2Player && attacker is L2Player || attacker is L2Summon)
            {
                if (CurCP > 0)
                {
                    CurCP -= damage;

                    if (CurCP < 0)
                    {
                        damage = CurCP * -1;
                        CurCP = 0;
                    }
                }
            }

            CurHP -= damage;

            StatusUpdate su = new StatusUpdate(ObjID);
            su.add(StatusUpdate.CUR_HP, (int)CurHP);
            su.add(StatusUpdate.CUR_CP, (int)CurCP);
            broadcastPacket(su);

            if (CurHP <= 0)
            {
                CurHP = 0;
                doDie(attacker, false);
                return;
            }

            AICharacter.NotifyOnHit(attacker, damage);
        }


        public virtual void doDie(L2Character killer, bool bytrigger)
        {
            Dead = true;
            StopRegeneration();
            if (isAttacking())
                abortAttack();

            if (isCastingNow())
                abortCast();

            CurHP = 0;
            StatusUpdate su = new StatusUpdate(ObjID);
            su.add(StatusUpdate.CUR_HP, 0);
            broadcastPacket(su);
            
            sendMessage("You died from "+killer.Name);
            killer.sendMessage("You killed "+Name);
            broadcastPacket(new Die(this));

            updateMagicEffectIcons();

            killer.AICharacter.NotifyOnKill(this);
            AICharacter.NotifyOnDie(killer);

            AICharacter.Disable();
        }


        public virtual void DeleteByForce()
        {
            if (AICharacter != null)
                AICharacter.Disable();

            StopRegeneration();

            foreach (AbnormalEffect a in _effects)
                a._timer.Enabled = false;

            _effects.Clear();

            broadcastPacket(new DeleteObject(ObjID));
            L2World.Instance.UnrealiseEntry(this, true);
        }

        public virtual L2Item ActiveWeapon
        {
            get { return null; }
            set { /*cls*/ }
        }

        public virtual L2Item SecondaryWeapon
        {
            get { return null; }
            set { /*cls*/ }
        }

        public virtual L2Item ActiveArmor
        {
            get { return null; }
            set { /*cls*/ }

        }
        public L2Character CurrentTarget;
        public virtual void doAttack(L2Character target)
        {
            if (target == null)
            {
                AICharacter.NotifyTargetNull();
                return;
            }

            if (target.Dead)
            {
                AICharacter.NotifyTargetDead();
                return;
            }

            if (attack_ToHit != null && attack_ToHit.Enabled)
                return;

            if (attack_ToEnd != null && attack_ToEnd.Enabled)
            {
                return;
            }

            double dist = 60, reqMp = 0;

            L2Item weapon = ActiveWeapon;
            double timeAtk = CharacterStat.getStat(TEffectType.b_attack_spd);
            bool dual = false, ranged = false, ss = false;
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
                if (weapon.Template.WeaponType == ItemTemplate.L2ItemWeaponType.crossbow || weapon.Template.WeaponType == ItemTemplate.L2ItemWeaponType.bow)
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
                tryMoveTo(target.X, target.Y, target.Z);
                return;
            }

            if (reqMp > 0 && reqMp > CurMP)
            {
                sendMessage("no mp " + CurMP + " " + reqMp);
                AICharacter.NotifyMpEnd(target);
                return;
            }

            Attack atk = new Attack(this, target, ss, 5);

            if (dual)
            {
                hit1 = genHitSimple(dual, ss);
                atk.addHit(target.ObjID, (int)hit1.damage, hit1.miss, hit1.crit, hit1.shieldDef > 0);

                hit2 = genHitSimple(dual, ss);
                atk.addHit(target.ObjID, (int)hit2.damage, hit2.miss, hit2.crit, hit2.shieldDef > 0);
            }
            else
            {
                hit1 = genHitSimple(false, ss);
                atk.addHit(target.ObjID, (int)hit1.damage, hit1.miss, hit1.crit, hit1.shieldDef > 0);
            }

            CurrentTarget = target;

            if (attack_ToHit == null)
            {
                attack_ToHit = new Timer();
                attack_ToHit.Elapsed += new ElapsedEventHandler(AttackDoHit);
            }

            double timeToHit = ranged ? timeAtk * 0.5 : timeAtk * 0.6;
            attack_ToHit.Interval = timeToHit;
            attack_ToHit.Enabled = true;

            if (dual)
            {
                if (attack_toHitBonus == null)
                {
                    attack_toHitBonus = new Timer();
                    attack_toHitBonus.Elapsed += new ElapsedEventHandler(AttackDoHit2nd);
                }

                attack_toHitBonus.Interval = timeAtk * 0.78;
                attack_toHitBonus.Enabled = true;
            }

            if (attack_ToEnd == null)
            {
                attack_ToEnd = new Timer();
                attack_ToEnd.Elapsed += new ElapsedEventHandler(AttackDoEnd);
            }

            attack_ToEnd.Interval = timeAtk;
            attack_ToEnd.Enabled = true;

            broadcastPacket(atk);
        }

        public class Hit
        {
            public bool miss;
            public double shieldDef;
            public bool crit;
            public double damage;
        }

        public Hit hit1, hit2;

        public Hit genHitSimple(bool dual, bool ss)
        {
            Hit h = new Hit();
            h.miss = Formulas.checkMissed(this, (L2Character)CurrentTarget);
            if (!h.miss)
            {
                h.shieldDef = Formulas.checkShieldDef(this, (L2Character)CurrentTarget);
                h.crit = Formulas.checkCrit(this, (L2Character)CurrentTarget);
                h.damage = Formulas.getPhysHitDamage(this, (L2Character)CurrentTarget, 0);
                if (dual)
                    h.damage *= .5;
                if (ss)
                    h.damage *= 2;
                if (h.crit)
                    h.damage *= 2;
            }

            return h;
        }

        public virtual void AttackDoHit(object sender, ElapsedEventArgs e)
        {
            if (CurrentTarget != null)
            {
                if (!hit1.miss)
                {
                    CurrentTarget.reduceHp(this, hit1.damage);

                    if (CurrentTarget is L2Player) //$c1 has received $s3 damage from $c2.
                        CurrentTarget.sendPacket(new SystemMessage(2262).AddName(CurrentTarget).AddName(this).AddNumber(hit1.damage));
                }
                else
                {
                    if (CurrentTarget is L2Player) //$c1 has evaded $c2's attack.
                    {
                        CurrentTarget.sendPacket(new SystemMessage(2264).AddName(CurrentTarget).AddName(this));
                        ((L2Player)CurrentTarget).AICharacter.NotifyEvaded(this);
                    }
                }
            }

            attack_ToHit.Enabled = false;
        }

        public virtual void AttackDoHit2nd(object sender, ElapsedEventArgs e)
        {
            if (CurrentTarget != null)
            {
                if (!hit2.miss)
                {
                    CurrentTarget.reduceHp(this, hit2.damage);
                    if (CurrentTarget is L2Player) //$c1 has received $s3 damage from $c2.
                        CurrentTarget.sendPacket(new SystemMessage(2262).AddName(CurrentTarget).AddName(this).AddNumber(hit2.damage));
                }
                else
                {
                    if (CurrentTarget is L2Player) //$c1 has evaded $c2's attack.
                    {
                        CurrentTarget.sendPacket(new SystemMessage(2264).AddName(CurrentTarget).AddName(this));
                        ((L2Player)CurrentTarget).AICharacter.NotifyEvaded(this);
                    }
                }
            }

            attack_toHitBonus.Enabled = false;
        }

        public virtual void AttackDoEnd(object sender, ElapsedEventArgs e)
        {
            attack_ToEnd.Enabled = false;

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

        public void broadcastSoulshotUse(int itemId)
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
                broadcastPacket(new MagicSkillUse(this, this, skillId, 1, 0));
                sendSystemMessage(342);//Your soulshots are enabled.
            }
        }

        public virtual void abortAttack()
        {
            if (attack_ToHit != null && attack_ToHit.Enabled)
                attack_ToHit.Enabled = false;

            if (attack_toHitBonus != null && attack_toHitBonus.Enabled)
                attack_toHitBonus.Enabled = false;

            if (attack_ToEnd != null && attack_ToEnd.Enabled)
                attack_ToEnd.Enabled = false;

          //  hit1 = null;
          //  hit2 = null;
        }

        public Timer attack_ToHit, attack_toHitBonus, attack_ToEnd;


        public int _p_block_spell = 0, _p_block_skill = 0;
        public int _p_block_act = 0;

        public virtual bool cantMove()
        {
            if (_p_block_act == 1)
                return true;

            if ((AbnormalBitMaskEx & AbnormalMaskExFreezing) == AbnormalMaskExFreezing)
                return true;

            if (isCastingNow())
                return true;

            return false;
        }

        public void tryMoveTo(int x, int y, int z)
        {
            if (cantMove())
            {
                sendActionFailed();
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
                updateAbnormalExEffect();
        }

        public CStats CharacterStat;

        public void CStatsInit()
        {
            if(CharacterStat == null)
                CharacterStat = new CStats(this);
        }

        public void ChangeTarget(L2Character target = null)
        {
            if (target == null)
            {
                broadcastPacket(new TargetUnselected(this));
                CurrentTarget = null;
            }
            else
            {
                if (CurrentTarget != null)
                    if (CurrentTarget.ObjID != target.ObjID)
                    {
                        broadcastPacket(new TargetUnselected(this));
                    }
                    else
                    {
                        OnOldTargetSelection(target);
                        return;
                    }

                CurrentTarget = target;

                broadcastPacket(new TargetSelected(ObjID, target));
                OnNewTargetSelection(target);
            }
        }

        public virtual void OnOldTargetSelection(L2Object target) { }
        public virtual void OnNewTargetSelection(L2Object target) { }

        public override void RegenUpdateTaskDone(object sender, ElapsedEventArgs e)
        {
            bool hp = CurHP < CharacterStat.getStat(TEffectType.b_max_hp), mp = CurMP < CharacterStat.getStat(TEffectType.b_max_mp), cp = false;

            if (this is L2Player)
                cp = CurCP < CharacterStat.getStat(TEffectType.b_max_cp);

            if (hp || mp || cp)
            {
                StatusUpdate su = new StatusUpdate(ObjID);
                if (hp)
                    su.add(StatusUpdate.CUR_HP, (int)CurHP);
                if (mp)
                    su.add(StatusUpdate.CUR_MP, (int)CurMP);
                if (cp)
                    su.add(StatusUpdate.CUR_CP, (int)CurCP);

                broadcastPacket(su);

                if (this is L2Summon)
                {
                    if (((L2Summon)this).Owner != null)
                        ((L2Summon)this).Owner.sendPacket(new PetStatusUpdate((L2Summon)this));
                }

                if (this is L2Player)
                {
                    if(((L2Player)this).Party != null)
                        ((L2Player)this).Party.broadcastToMembers(new PartySmallWindowUpdate((L2Player)this));
                }
            }
        }

        public override void RegenTaskDone(object sender, ElapsedEventArgs e)
        {
            double maxhp = CharacterStat.getStat(TEffectType.b_max_hp);
            if (CurHP < maxhp)
            {
                CurHP += CharacterStat.getStat(TEffectType.b_reg_hp);
                if (CurHP > maxhp)
                    CurHP = maxhp;
            }

            double maxmp = CharacterStat.getStat(TEffectType.b_max_mp);
            if (CurMP < maxmp)
            {
                CurMP += CharacterStat.getStat(TEffectType.b_reg_mp);
                if (CurMP > maxmp)
                    CurMP = maxmp;
            }

            if (this is L2Player)
            {
                double maxcp = CharacterStat.getStat(TEffectType.b_max_cp);
                if (CurCP < maxcp)
                {
                    CurCP += CharacterStat.getStat(TEffectType.b_reg_cp);
                    if (CurCP > maxcp)
                        CurCP = maxcp;
                }
            }
        }

        public virtual bool isCastingNow()
        {
            if (castTime == null)
                return false;

            return castTime.Enabled;
        }

        public virtual void abortCast()
        {
            if (castTime != null)
            {
                castTime.Enabled = false;
            }

            broadcastPacket(new MagicSkillCanceld(ObjID));
            currentCast = null;
        }

        public TSkill currentCast;
        public System.Timers.Timer castTime;

        public SortedList<int, L2SkillCoolTime> _reuse = new SortedList<int, L2SkillCoolTime>();


        public int castSkill(TSkill skill)
        {
            if (isCastingNow())
            {
                return 1;
            }

            L2Object target = skill.getTargetCastId(this);

            if (target == null)
            {
                return 2;
            }

            if (skill.cast_range != -1)
            {
                double dis = Calcs.calculateDistance(this, target, true);
                if (dis > skill.cast_range)
                {
                    return 3;
                }
            }

            if (skill.reuse_delay > 0)
            {
                if (_reuse.ContainsKey(skill.skill_id))
                {
                    TimeSpan ts = _reuse[skill.skill_id].stopTime - DateTime.Now;

                    if (ts.TotalMilliseconds > 0)
                        return 4;
                }
            }

            if (skill.mp_consume1 > 0 || skill.mp_consume2 > 0)
            {
                if (CurMP < skill.mp_consume1 + skill.mp_consume2)
                {
                    return 5;
                }
            }

            if (skill.hp_consume > 0)
            {
                if (CurHP < skill.hp_consume)
                {
                    return 6;
                }
            }

            if (skill.effects.Count > 0)
            {
                bool fail = false;
                foreach (TEffect ef in skill.effects)
                    if (!ef.canUse(this))
                    {
                        fail = true;
                        break;
                    }

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
                _reuse.Add(reuse.id, reuse);
            }

            {
                //SystemMessage sm = new SystemMessage(46); //You use $s1.
                //sm.addSkillName(skill.ClientID, skill.Level);
                //sendPacket(sm);
                //TODO nearby objects notify
            }

            if (skill.hp_consume > 0)
            {
                CurHP -= skill.hp_consume;

                StatusUpdate su = new StatusUpdate(ObjID);
                su.add(StatusUpdate.CUR_HP, (int)CurHP);
                broadcastPacket(su);
            }

            if (skill.mp_consume1 > 0)
            {
                CurMP -= skill.mp_consume1;

                StatusUpdate su = new StatusUpdate(ObjID);
                su.add(StatusUpdate.CUR_MP, (int)CurMP);
                broadcastPacket(su);
            }

            int hitTime = (int)skill.skill_hit_time;

            int hitT = hitTime > 0 ? (int)(hitTime * 0.95) : 0;
            currentCast = skill;

            broadcastPacket(new MagicSkillUse(this, target, skill, hitTime == 0 ? 20 : hitTime));

            if (hitTime > 50)
            {
                if (castTime == null)
                {
                    castTime = new System.Timers.Timer();
                    castTime.Elapsed += new ElapsedEventHandler(castEnd);
                }

                castTime.Interval = hitT;
                castTime.Enabled = true;
            }
            else
                castEnd();

            return -1;
        }

        private void castEnd(object sender = null, ElapsedEventArgs e = null)
        {
            if (currentCast.mp_consume2 > 0)
            {
                if (CurMP < currentCast.mp_consume2)
                {
                    currentCast = null;
                    castTime.Enabled = false;
                    return;
                }
                else
                {
                    CurMP -= currentCast.mp_consume2;

                    StatusUpdate su = new StatusUpdate(ObjID);
                    su.add(StatusUpdate.CUR_MP, (int)CurMP);
                    broadcastPacket(su);
                }
            }

            if (currentCast.cast_range != -1)
            {
                bool block = false;
                if (CurrentTarget != null)
                {
                    double dis = Calcs.calculateDistance(this, CurrentTarget, true);
                    if (dis > currentCast.effective_range)
                        block = true;
                }
                else
                    block = true;

                if (block)
                {
                    currentCast = null;
                    castTime.Enabled = false;
                    return;
                }
            }

            SortedList<int, L2Object> arr = currentCast.getAffectedTargets(this);
            List<int> broadcast = new List<int>();
            broadcast.AddRange(arr.Keys);

            broadcastPacket(new MagicSkillLaunched(this, broadcast, currentCast.skill_id, currentCast.level));

            addEffects(this, currentCast, arr);
            currentCast = null;
            if(castTime != null)
                lock(castTime)
                    castTime.Enabled = false;
        }

        public virtual L2Character[] getPartyCharacters()
        {
            return new L2Character[] { this };
        }

        public bool isMoving()
        {
            if (updatePositionTime != null)
                return updatePositionTime.Enabled;

            return false;
        }

        public void MoveTo(int x, int y, int z)
        {
            if (isAttacking())
                abortAttack();

            if (updatePositionTime == null)
            {
                updatePositionTime = new Timer();
                updatePositionTime.Interval = 100;
                updatePositionTime.Elapsed += new ElapsedEventHandler(UpdatePositionTask);
            }
            else
            {
                if (updatePositionTime.Enabled) // новый маршрут, но старый не закончен
                {
                    NotifyStopMove(false);
                }
            }

            DestX = x;
            DestY = y;
            DestZ = z;

            double dx = (x - X), dy = (y - Y), dz = (z - Z);
            double distance = Math.Sqrt(dx * dx + dy * dy);

            double speed = CharacterStat.getStat(TEffectType.p_speed);
            double spy = dy / distance, spx = dx / distance;
 
            TicksToMove = 1 + (int)(10 * distance / speed);
            TicksToMoveCompleted = 0;
            XSpeedTicks = (DestX - X) / TicksToMove;
            YSpeedTicks = (DestY - Y) / TicksToMove;

            Heading = (int)((Math.Atan2(-spx, -spy) * 10430.378) + short.MaxValue);

            broadcastPacket(new CharMoveToLocation(this));

            updatePositionTime.Enabled = true;

            AICharacter.NotifyStartMoving();
       }

        private void UpdatePositionTask(object sender, ElapsedEventArgs e)
        {
            validateWaterZones();

            if (DestX == X && DestY == Y && DestZ == Z)
            {
                NotifyArrived();
                return;
            }

            if (TicksToMove > TicksToMoveCompleted)
            {
                TicksToMoveCompleted++;
                X += (int)XSpeedTicks ;
                Y += (int)YSpeedTicks;
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
            if (updatePositionTime.Enabled)
            {
                updatePositionTime.Enabled = false;

                if (broadcast)
                    AICharacter.NotifyStopMoving();

                if (update)
                    broadcastPacket(new StopMove(this));
            }

            DestX = 0;
            DestY = 0;
            DestZ = 0;
            XSpeedTicks = 0;
            YSpeedTicks = 0;
            TicksToMove = 0;
        }

        public virtual void NotifyArrived() 
        {
            updatePositionTime.Enabled = false;

            DestX = 0;
            DestY = 0;
            DestZ = 0;
            XSpeedTicks = 0;
            YSpeedTicks = 0;
            TicksToMove = 0;

            AICharacter.NotifyStopMoving();
        }

        private Timer updatePositionTime;
        private int TicksToMove, TicksToMoveCompleted = 0;
        private float XSpeedTicks;
        private float YSpeedTicks;




        public bool isInFrontOfTarget()
        {
            return false;
        }

        public bool isInFrontOfTarget(int x, int y, int heading)
        {
            return false;
        }

        public bool isBehindTarget()
        {
            return false;
        }

        public virtual bool isAttacking()
        {
            if (attack_ToEnd != null)
                return attack_ToEnd.Enabled;

            return false;
        }

        public virtual int ClanId { get { return 0; } set { } }
        public virtual int ClanCrestId { get { return 0; } }
        public virtual int AllianceId { get { return 0; } }
        public virtual int AllianceCrestId { get { return 0; } }

        public virtual int MaxHP { get; set; }
        public virtual int MaxCP { get; set; }
        public virtual int MaxMP { get; set; }

        public override double CurHP
        {
            get; set;
        }

        public override double CurMP
        {
            get; set;
        }

        public override double CurCP
        {
            get; set;
        }

        public override string asString()
        {
            return "L2Character:" + ObjID;
        }

        public virtual L2Item getWeaponItem()
        {
            return null;
        }

        List<long> Muted0;
        List<long> Muted1;
        List<long> Muted2;
        public void Mute(int type, long hashId, bool start)
        {
            List<long> list = null;
            if (type == 0)
            {
                if (Muted0 == null)
                    Muted0 = new List<long>();

                list = Muted0;
            }
            else if (type == 1)
            {
                if (Muted1 == null)
                    Muted1 = new List<long>();

                list = Muted1;
            }
            else if (type == 2)
            {
                if (Muted2 == null)
                    Muted2 = new List<long>();

                list = Muted2;
            }

            if (start)
            {
                if (!list.Contains(hashId))
                    list.Add(hashId);
            }
            else
                list.Remove(hashId);
        }

        public bool MutedPhysically
        {
            get
            {
                return Muted0 != null && Muted0.Count > 0;  
            }
        }

        public bool MutedMagically
        {
            get
            {
                return Muted1 != null && Muted1.Count > 0;
            }
        }

        public bool MutedSpecial
        {
            get
            {
                return Muted2 != null && Muted2.Count > 0;
            }
        }
    }
}
