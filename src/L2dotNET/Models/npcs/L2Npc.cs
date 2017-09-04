using System.Collections.Generic;
using System.Linq;
using System.Timers;
using L2dotNET.model.items;
using L2dotNET.model.player;
using L2dotNET.model.skills2;
using L2dotNET.Network.serverpackets;
using L2dotNET.tables;
using L2dotNET.templates;
using L2dotNET.tools;
using L2dotNET.world;
using log4net;

namespace L2dotNET.model.npcs
{
    public class L2Npc : L2Character
    {
        public new NpcTemplate Template;
        public bool Summoned;
        public bool StructureControlled = false;

        private readonly ILog Log = LogManager.GetLogger(typeof(L2Npc)) ;

        public L2Npc(int objectId, NpcTemplate template) : base(objectId, template)
        {
            Template = template;
            Name = template.Name;
            CStatsInit();
            CurHp = 100;
            CurCp = 100;
            CurMp = 100;
            MaxCp = 100;
            MaxHp = 100;
            MaxMp = 100;
        }

        //public virtual void setTemplate(NpcTemplate template)
        //{
        //    Template = template;
        //    ObjID = IdFactory.Instance.nextId();
        //    CStatsInit();
        //    CharacterStat.setTemplate(template);
        //    CurHP = CharacterStat.getStat(skills2.TEffectType.b_max_hp);
        //    Name = template.Name;
        //    AIProcessor = new citizen();
        //    AIProcessor.dialog = new Dictionary<string, string>();
        //    AIProcessor.dialog.Add("fnHi", "lector001.htm");
        //    AIProcessor.dialog.Add("fnFeudInfo", "gludio_feud_manager001.htm");
        //    AIProcessor.dialog.Add("fnNoFeudInfo", "farm_messenger002.htm");
        //    AIProcessor.myself = this;
        //}

        public override void NotifyAction(L2Player player)
        {
            double dis = Calcs.CalculateDistance(player, this, true);
                TryMoveTo(X, Y, Z);
        }

        public int NpcId => Template.NpcId;

        public int NpcHashId => Template.NpcId + 1000000;

        public byte isRunning()
        {
            return 1;
        }

        public byte IsAlikeDead()
        {
            return 0;
        }

        public override void OnAction(L2Player player)
        {
            if (player.Target != this)
                player.SetTarget(this);
            else
            {
                player.MoveTo(X, Y, Z);
                player.SendPacket(new MoveToPawn(player, this,150));
                if(Template.Type == "L2Monster")
                {
                    Log.Debug("Attack Monester By L2NPC");
                    player.DoAttack(this);
                }
                player.SendActionFailed();
            }
        }

        public virtual void OnTeleportRequest(L2Player player)
        {
            NpcData.Instance.RequestTeleportList(this, player, 1);
        }

        public void UseTeleporter(L2Player player, int type, int entryId)
        {
            NpcData.Instance.RequestTeleport(this, player, type, entryId);
        }

        public virtual void OnDialog(L2Player player, int ask, int reply)
        {
            player.FolkNpc = this;

            //if (ask > 0 && ask < 1000)
            //{
            //    QuestManager.Instance.OnQuestTalk(player, this, ask, reply);
            //    return;
            //}

            //AITemplate t = AIManager.Instance.CheckDialogResult(Template.NpcId);
            //if (t != null)
            //{
            //    t.onDialog(player, ask, reply, this);
            //    return;
            //}

            //switch (ask)
            //{
            //    case -1:
            //        switch (reply)
            //        {
            //            case 8:
            //                player.sendPacket(new ExBuySellList_Buy(player.getAdena()));
            //                player.sendPacket(new ExBuySellList_Sell(player));
            //                break;

            //            default:
            //                NpcData.Instance.Buylist(player, this, (short)reply);
            //                break;
            //        }
            //        break;
            //    case -3:
            //        grandmaster_total.onReply(player, reply, this);
            //        break;
            //    case -19: //нобл запрос
            //        switch (reply)
            //        {
            //            case 0:
            //            case 1:
            //                player.ShowHtm(player.Noblesse == 1 ? "fornobless.htm" : "fornonobless.htm", this);
            //                break;
            //        }
            //        break;
            //    case -20: //нобл запрос
            //        switch (reply)
            //        {
            //            case 2:
            //                NpcData.Instance.RequestTeleportList(this, player, 2);
            //                break;
            //        }
            //        break;
            //    case -21: //нобл запрос
            //        switch (reply)
            //        {
            //            case 2:
            //                NpcData.Instance.RequestTeleportList(this, player, 3);
            //                break;
            //        }
            //        break;
            //    case -22: //нобл запрос
            //        switch (reply)
            //        {
            //            case 2:
            //                NpcData.Instance.RequestTeleportList(this, player, 1);
            //                break;
            //        }
            //        break;
            //    case -303:
            //        MultiSell.Instance.ShowList(player, this, reply);
            //        break;
            //    case -305:
            //        switch (reply)
            //        {
            //            case 1:
            //                //  NpcData.getInstance().multisell(player, this, reply); //TEST
            //                break;
            //        }
            //        break;
            //    case -1000:
            //    {
            //        switch (reply)
            //        {
            //            case 1:
            //                //See the lord and get the tax rate information
            //                break;
            //        }
            //    }
            //        break;
            //}
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

        public void ShowPrivateWarehouse(L2Player player)
        {
            List<L2Item> items = player.GetAllItems().Where(item => item.IsEquipped != 1).ToList();

            player.SendPacket(new WareHouseDepositList(player, items, WareHouseDepositList.WhPrivate));
            player.FolkNpc = this;
        }

        public void ShowClanWarehouse(L2Player player)
        {
            if (player.Clan == null)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.YouDoNotHaveTheRightToUseClanWarehouse);
                player.SendActionFailed();
                return;
            }

            if (player.Clan.Level == 0)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.OnlyLevel1ClanOrHigherCanUseWarehouse);
                player.SendActionFailed();
                return;
            }

            List<L2Item> items = player.GetAllItems().Where(item => item.IsEquipped != 1).ToList();

            player.SendPacket(new WareHouseDepositList(player, items, WareHouseDepositList.WhClan));
            player.FolkNpc = this;
        }

        public void ShowPrivateWarehouseBack(L2Player player)
        {
            //if (player._warehouse == null)
            //{
            //    player.sendSystemMessage(SystemMessage.SystemMessageId.NO_ITEM_DEPOSITED_IN_WH);
            //    player.sendActionFailed();
            //    return;
            //}

            //List<L2Item> items = player.getAllWarehouseItems().Cast<L2Item>().ToList();

            //if (items.Count == 0) // на случай если вх был создан и убраны вещи до времени выхода с сервера
            //{
            //    player.sendSystemMessage(SystemMessage.SystemMessageId.NO_ITEM_DEPOSITED_IN_WH);
            //    player.sendActionFailed();
            //    return;
            //}

            //player.sendPacket(new WareHouseWithdrawalList(player, items, WareHouseWithdrawalList.WH_PRIVATE));
            //player.FolkNpc = this;
        }

        public void ShowClanWarehouseBack(L2Player player)
        {
            if (player.Clan == null)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.YouDoNotHaveTheRightToUseClanWarehouse);
                player.SendActionFailed();
            }
            else
            {
                if (player.Clan.Level != 0)
                    return;

                player.SendSystemMessage(SystemMessage.SystemMessageId.OnlyLevel1ClanOrHigherCanUseWarehouse);
                player.SendActionFailed();
            }
        }

        public void ShowSkillLearn(L2Player player, bool backward)
        {
            player.SendMessage("I cannot teach you anything.");
        }

        public override void BroadcastUserInfo()
        {
            foreach (var character in L2World.Instance.GetObjects().Where(x=>x.GetType() == typeof(L2Character)))
                character.SendPacket(new NpcInfo(this));
        }

        public override void BroadcastUserInfoToObject(L2Object l2Object)
        {
            l2Object.SendPacket(new NpcInfo(this));
        }

        public override void OnSpawn(bool notifyOthers = true)
        {
            if(notifyOthers)
                BroadcastUserInfo();
            StartAi();
        }

        public void ShowAvailRegularSkills(L2Player player, bool backward)
        {
            SortedList<int, AcquireSkill> list = player.ActiveSkillTree ?? new SortedList<int, AcquireSkill>();

            int nextLvl = 800;
            foreach (AcquireSkill e in SkillTable.Instance.GetAllRegularSkills(player.ActiveClass.ClassId.Id).Skills)
            {
                if (e.GetLv > player.Level)
                {
                    if (nextLvl > e.GetLv)
                        nextLvl = e.GetLv;
                    continue;
                }

                if (list.ContainsKey(e.Id))
                    continue;

                if (player.Skills.ContainsKey(e.Id))
                {
                    Skill skill = player.Skills[e.Id];

                    if (skill.Level >= e.Lv)
                        continue;

                    if (list.ContainsKey(e.Id))
                        continue;

                    list.Add(e.Id, e);
                    break;
                }

                list.Add(e.Id, e);
            }

            if (list.Count == 0)
            {
                if (backward)
                {
                    list.Clear();
                    player.ActiveSkillTree = list;
                    player.SendPacket(new AcquireSkillList(AcquireSkillList.SkillType.Usual));
                }

                if (nextLvl != 800)
                    player.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.DoNotHaveFurtherSkillsToLearnS1).AddNumber(nextLvl));
                else
                    player.SendSystemMessage(SystemMessage.SystemMessageId.NoMoreSkillsToLearn);

                player.SendActionFailed();
                return;
            }

            player.ActiveSkillTree = list;
            player.SendPacket(new AcquireSkillList(AcquireSkillList.SkillType.Usual));
            player.FolkNpc = this;
        }

        private Timer _corpseTimer;
        public int ResidenceId;

        public override void DoDie(L2Character killer)
        {
            base.DoDie(killer);

            if (Template.CorpseTime <= 0)
            {
                return;
            }
                

            _corpseTimer = new Timer(Template.CorpseTime * 1000);
            _corpseTimer.Elapsed += new ElapsedEventHandler(RemoveCorpse);
            _corpseTimer.Enabled = true;
        }

        private void RemoveCorpse(object sender, ElapsedEventArgs e)
        {
            _corpseTimer.Enabled = false;
            BroadcastPacket(new DeleteObject(ObjId));
            L2World.Instance.RemoveObject(this);
        }

        public override void DeleteByForce()
        {
            if ((_corpseTimer != null) && _corpseTimer.Enabled)
                _corpseTimer.Enabled = false;

            base.DeleteByForce();
        }

        public bool IsBoss()
        {
            return false; //Template.AiT == templates.TObjectCategory.boss;
        }

        public void ConsumeBody()
        {
            if (_corpseTimer != null)
                _corpseTimer.Enabled = false;

            _corpseTimer = null;

            RemoveCorpse(null, null);
        }

        public bool IsFlying()
        {
            return false;
        }

        public virtual int Attackable => 0;

        public override double Radius => Template.CollisionRadius == 0 ? 12 : Template.CollisionRadius;

        public override double Height => Template.CollisionHeight == 0 ? 22 : Template.CollisionHeight;

        public override string AsString()
        {
            return $"L2Npc:{Template.NpcId}; id {ObjId}";
        }

        public void CreateOnePrivateEx(int npcId, string aiType, int x, int y, int z) { }

        public void CastBuffForQuestReward(L2Character cha, int skillId)
        {
            cha.SendMessage($"L2Npc.CastBuffForQuestReward {skillId}");
            //TODO: Fix the unassigned objected created
            new BuffForQuestReward(this, cha, skillId);
        }
    }
}