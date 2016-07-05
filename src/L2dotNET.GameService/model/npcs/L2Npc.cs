using System.Collections.Generic;
using System.Linq;
using System.Timers;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tables;
using L2dotNET.GameService.Templates;
using L2dotNET.GameService.Tools;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Npcs
{
    public class L2Npc : L2Character
    {
        public NpcTemplate Template;
        public bool Summoned;
        public bool StructureControlled = false;
        public AI.Template.Ai AiProcessor;

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
            if (dis < 151)
                AiProcessor.Talked(player);
            else
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
            player.SendMessage("onAction " + AsString());

            player.ChangeTarget(this);
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

            AiProcessor.TalkedReply(player, ask, reply);

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

        public void ShowPrivateWarehouse(L2Player player)
        {
            List<L2Item> items = player.GetAllItems().Where(item => (item.IsEquipped != 1)).ToList();

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

            List<L2Item> items = player.GetAllItems().Where(item => (item.IsEquipped != 1) ).ToList();

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
                if (player.Clan.Level == 0)
                {
                    player.SendSystemMessage(SystemMessage.SystemMessageId.OnlyLevel1ClanOrHigherCanUseWarehouse);
                    player.SendActionFailed();
                }
            }
        }

        public virtual void ShowSkillLearn(L2Player player, bool backward)
        {
            player.SendMessage("I cannot teach you anything.");
        }

        public override void BroadcastUserInfo()
        {
            foreach (L2Player obj in KnownObjects.Values.OfType<L2Player>())
                obj.SendPacket(new NpcInfo(this));
        }

        public override void OnSpawn()
        {
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

                    if (!list.ContainsKey(e.Id))
                    {
                        list.Add(e.Id, e);
                        break;
                    }
                }
                else
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

        public override void DoDie(L2Character killer, bool bytrigger)
        {
            base.DoDie(killer, bytrigger);

            if (Template.CorpseTime > 0)
            {
                _corpseTimer = new Timer(Template.CorpseTime * 1000);
                _corpseTimer.Elapsed += new ElapsedEventHandler(RemoveCorpse);
                _corpseTimer.Enabled = true;
            }
        }

        private void RemoveCorpse(object sender, ElapsedEventArgs e)
        {
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
            return "L2Npc:" + Template.NpcId + "; id " + ObjId;
        }

        public void CreateOnePrivateEx(int npcId, string aiType, int x, int y, int z) { }

        public double MaxHp => CharacterStat.GetStat(EffectType.BMaxHp);

        public void CastBuffForQuestReward(L2Character cha, int skillId)
        {
            cha.SendMessage("L2Npc.CastBuffForQuestReward " + skillId);
            new BuffForQuestReward(this, cha, skillId);
        }
    }
}