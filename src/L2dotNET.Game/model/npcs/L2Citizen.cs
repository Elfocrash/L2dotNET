using System.Collections.Generic;
using System.Threading;
using System.Timers;
using L2dotNET.Game.ai.npcai;
using L2dotNET.Game.ai.template;
using L2dotNET.Game.model.items;
using L2dotNET.Game.model.npcs.ai;
using L2dotNET.Game.model.quests;
using L2dotNET.Game.model.skills2;
using L2dotNET.Game.network.l2send;
using L2dotNET.Game.tables;
using L2dotNET.Game.tables.multisell;
using L2dotNET.Game.tools;
using L2dotNET.Game.world;

namespace L2dotNET.Game.model.npcs
{
    public class L2Citizen : L2Character
    {
        public NpcTemplate Template;
        public bool _summoned;
        public bool structureControlled = false;
        public AI AIProcessor;

        public virtual void setTemplate(NpcTemplate template)
        {
            Template = template;
            ObjID = IdFactory.Instance.nextId();
            CStatsInit();
            CharacterStat.setTemplate(template);
            CurHP = CharacterStat.getStat(skills2.TEffectType.b_max_hp);
            Name = template.Name;
            AIProcessor = new citizen();
            AIProcessor.dialog = new Dictionary<string, string>();
            AIProcessor.dialog.Add("fnHi", "lector001.htm");
            AIProcessor.dialog.Add("fnFeudInfo", "gludio_feud_manager001.htm");
            AIProcessor.dialog.Add("fnNoFeudInfo", "farm_messenger002.htm");
            AIProcessor.myself = this;
        }

        public override void NotifyAction(L2Player player)
        {
            double dis = Calcs.calculateDistance(player, this, true);
            if (dis < 151)
            {
                AIProcessor.Talked(player);

                //AITemplate t = AIManager.getInstance().checkChatWindow(Template.NpcId);
                //if (t != null)
                //{
                //    t.onShowChat(player, this);
                //    return;
                //}

                //if (Template.fnHi != null)
                //    sendPacket(new NpcHtmlMessage(player, Template.fnHi, ObjID, 0));
            }
            else
                tryMoveTo(X, Y, Z);
        }

        public int NpcId
        {
            get { return Template.NpcId; }
        }

        public int NpcHashId
        {
            get { return Template.NpcId + 1000000; }
        }

        public byte isRunning()
        {
            return 1;
        }

        public byte isAlikeDead()
        {
            return 0;
        }

        public override void onAction(L2Player player)
        {
            player.sendMessage("onAction " + asString());

            player.ChangeTarget(this);
        }

        public virtual void onTeleportRequest(L2Player player)
        {
            NpcData.Instance.RequestTeleportList(this, player, 1);
        }

        public void UseTeleporter(L2Player player, int type, int entryId)
        {
            NpcData.Instance.RequestTeleport(this, player, type, entryId);
        }

        public virtual void onDialog(L2Player player, int ask, int reply)
        {
            player.FolkNpc = this;

            AIProcessor.TalkedReply(player, ask, reply);
            return;

            if (ask > 0 && ask < 1000)
            {
                QuestManager.getInstance().onQuestTalk(player, this, ask, reply);
                return;
            }

            AITemplate t = AIManager.Instance.CheckDialogResult(Template.NpcId);
            if (t != null)
            {
                t.onDialog(player, ask, reply, this);
                return;
            }

            switch (ask)
            {
                case -1:
                    switch (reply)
                    {
                        case 8:
                            player.sendPacket(new ExBuySellList_Buy(player.getAdena()));
                            player.sendPacket(new ExBuySellList_Sell(player));
                            break;

                        default:
                            NpcData.Instance.Buylist(player, this, (short)reply);
                            break;
                    }
                    break;
                case -3:
                    grandmaster_total.onReply(player, reply, this);
                    break;
                case -19: //нобл запрос
                    switch (reply)
                    {
                        case 0:
                        case 1:
                            player.ShowHtm(player.Noblesse == 1 ? "fornobless.htm" : "fornonobless.htm", this);
                            break;
                    }
                    break;
                case -20: //нобл запрос
                    switch (reply)
                    {
                        case 2:
                            NpcData.Instance.RequestTeleportList(this, player, 2);
                            break;
                    }
                    break;
                case -21: //нобл запрос
                    switch (reply)
                    {
                        case 2:
                            NpcData.Instance.RequestTeleportList(this, player, 3);
                            break;
                    }
                    break;
                case -22: //нобл запрос
                    switch (reply)
                    {
                        case 2:
                            NpcData.Instance.RequestTeleportList(this, player, 1);
                            break;
                    }
                    break;
                case -303:
                    MultiSell.Instance.ShowList(player, this, reply);
                    break;
                case -305:
                    switch (reply)
                    {
                        case 1:
                            //  NpcData.getInstance().multisell(player, this, reply); //TEST
                            break;

                    }
                    break;
                case -1000:
                    {
                        switch (reply)
                        {
                            case 1:
                                //See the lord and get the tax rate information
                                break;

                        }
                    }
                    break;
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
            {
                player.sendActionFailed();
            }

        }

        public void showPrivateWarehouse(L2Player player)
        {
            List<L2Item> items = new List<L2Item>();
            foreach (L2Item item in player.getAllItems())
            {
                if (item._isEquipped == 1 || item.Template.Type == ItemTemplate.L2ItemType.questitem)
                    continue;

                items.Add(item);
            }

            player.sendPacket(new WareHouseDepositList(player, items, WareHouseDepositList.WH_PRIVATE));
            player.FolkNpc = this;
        }

        public void showClanWarehouse(L2Player player)
        {
            if (player.Clan == null)
            {
                player.sendSystemMessage(709); //You do not have the right to use the clan warehouse.
                player.sendActionFailed();
                return;
            }

            if (player.Clan.Level == 0)
            {
                player.sendSystemMessage(710); //Only clans of clan level 1 or higher can use a clan warehouse.
                player.sendActionFailed();
                return;
            }

            List<L2Item> items = new List<L2Item>();
            foreach (L2Item item in player.getAllItems())
            {
                if (item._isEquipped == 1 || item.Template.Type == ItemTemplate.L2ItemType.questitem)
                    continue;

                items.Add(item);
            }

            player.sendPacket(new WareHouseDepositList(player, items, WareHouseDepositList.WH_CLAN));
            player.FolkNpc = this;
        }

        public void showPrivateWarehouseBack(L2Player player)
        {
            if (player._warehouse == null)
            {
                player.sendSystemMessage(282); //You have not deposited any items in your warehouse.
                player.sendActionFailed();
                return;
            }

            List<L2Item> items = new List<L2Item>();
            foreach (L2Item item in player.getAllWarehouseItems())
            {
                items.Add(item);
            }

            if (items.Count == 0)// на случай если вх был создан и убраны вещи до времени выхода с сервера
            {
                player.sendSystemMessage(282); //You have not deposited any items in your warehouse.
                player.sendActionFailed();
                return;
            }

            player.sendPacket(new WareHouseWithdrawalList(player, items, WareHouseWithdrawalList.WH_PRIVATE));
            player.FolkNpc = this;
        }

        public void showClanWarehouseBack(L2Player player)
        {
            if (player.Clan == null)
            {
                player.sendSystemMessage(709); //You do not have the right to use the clan warehouse.
                player.sendActionFailed();
                return;
            }

            if (player.Clan.Level == 0)
            {
                player.sendSystemMessage(710); //Only clans of clan level 1 or higher can use a clan warehouse.
                player.sendActionFailed();
                return;
            }
        }

        public virtual void showSkillLearn(L2Player player, bool backward)
        {
            player.sendMessage("I cannot teach you anything.");
        }

        public override void broadcastUserInfo()
        {
            foreach(L2Object obj in knownObjects.Values)
                if(obj is L2Player)
                    obj.sendPacket(new NpcInfo(this));
        }

        public override void onSpawn()
        {
            broadcastUserInfo();
            StartAI();
        }

        public void showAvailRegularSkills(L2Player player, bool backward)
        {
            SortedList<int, TAcquireSkill> list;
            if (player.ActiveSkillTree != null)
                list = player.ActiveSkillTree;
            else
                list = new SortedList<int, TAcquireSkill>();

            int nextLvl = 800;
            foreach (TAcquireSkill e in TSkillTable.Instance.GetAllRegularSkills(player.ActiveClass.ClassId.Id).skills)
            {
                if (e.get_lv > player.Level)
                {
                    if (nextLvl > e.get_lv)
                        nextLvl = e.get_lv;
                    continue;
                }

                if (list.ContainsKey(e.id))
                    continue;

                if (player._skills.ContainsKey(e.id))
                {
                    TSkill skill = player._skills[e.id];

                    if (skill.level >= e.lv)
                        continue;

                    if (!list.ContainsKey(e.id))
                    {
                        list.Add(e.id, e);
                        break;
                    }
                }
                else
                    list.Add(e.id, e);
            }

            if (list.Count == 0)
            {
                if (backward)
                {
                    list.Clear();
                    player.ActiveSkillTree = list;
                    player.sendPacket(new AcquireSkillList(AcquireSkillList.ESTT_NORMAL, player));
                }

                if (nextLvl != 800)
                    //You do not have any further skills to learn.  Come back when you have reached Level $s1.
                    player.sendPacket(new SystemMessage(607).AddNumber(nextLvl));
                else
                {
                    //There are no other skills to learn.
                    player.sendSystemMessage(750);
                }

                player.sendActionFailed();
                return;
            }

            player.ActiveSkillTree = list;
            player.sendPacket(new AcquireSkillList(AcquireSkillList.ESTT_NORMAL, player));
            player.FolkNpc = this;
        }

        System.Timers.Timer _corpseTimer;
        public int residenceId;
        public override void doDie(L2Character killer, bool bytrigger)
        {
            base.doDie(killer, bytrigger);

            if (Template.corpse_time > 0)
            {
                _corpseTimer = new System.Timers.Timer(Template.corpse_time * 1000);
                _corpseTimer.Elapsed += new ElapsedEventHandler(removeCorpse);
                _corpseTimer.Enabled = true;
            }
        }

        private void removeCorpse(object sender, ElapsedEventArgs e)
        {
            broadcastPacket(new DeleteObject(ObjID));
            L2World.Instance.UnrealiseEntry(this, true);
        }

        public override void DeleteByForce()
        {
            if (_corpseTimer != null && _corpseTimer.Enabled)
                _corpseTimer.Enabled = false;

            base.DeleteByForce();
        }

        public bool isBoss()
        {
            return Template.Category == templates.TObjectCategory.boss;
        }

        public void consumeBody()
        {
            if (_corpseTimer != null)
                _corpseTimer.Enabled = false;

            _corpseTimer = null;

            removeCorpse(null, null);
        }

        public bool isFlying()
        {
            return false;
        }

        public virtual int Attackable
        {
            get { return 0; }
        }

        public override double Radius
        {
            get 
            {
                if (Template.CollisionRadius == 0)
                    return 12;

                return Template.CollisionRadius; 
            }
        }

        public override double Height
        {
            get 
            {
                if (Template.CollisionHeight == 0)
                    return 22;

                return Template.CollisionHeight; 
            }
        }

        public override string asString()
        {
            return "L2Citizen:" + Template.NpcId + "; id " + ObjID;
        }

        public void CreateOnePrivateEx(int npcId, string ai_type, int x, int y, int z)
        {
            
        }

        public double MaxHp 
        {
            get {
                return CharacterStat.getStat(TEffectType.b_max_hp);
            }
        }

        public void CastBuffForQuestReward(L2Character cha, int skillId)
        {
            cha.sendMessage("l2citizen.CastBuffForQuestReward " + skillId);
            new BuffForQuestReward(this, cha, skillId);
        }
    }

    class BuffForQuestReward
    {
        public L2Character cha;
        private L2Citizen npc;
        private int skillId;
        private TSkill skill;

        public BuffForQuestReward(L2Citizen npc, L2Character target, int skillId)
        {
            this.npc = npc;
            this.cha = target;
            this.skillId = skillId;
            skill = TSkillTable.Instance.Get(skillId);
            cha.broadcastPacket(new MagicSkillUse(npc, cha, skill, skill.skill_hit_time));

            new Thread(Run).Start();
        }

        private void Run()
        {
            if (cha == null || npc == null || skill == null)
                return;

            Thread.Sleep(skill.skill_hit_time);
            cha.addEffect(npc, skill, true, false);
        }
    }
}
