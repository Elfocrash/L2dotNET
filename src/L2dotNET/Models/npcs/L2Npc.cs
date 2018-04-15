using System.Collections.Generic;
using System.Linq;
using System.Timers;
using log4net;
using L2dotNET.Models.Items;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;
using L2dotNET.Templates;
using L2dotNET.Tools;
using L2dotNET.World;
using L2dotNET.Tables;

namespace L2dotNET.Models.Npcs
{
    public class L2Npc : L2Character
    {
        private readonly ILog Log = LogManager.GetLogger(typeof(L2Npc));

        public new NpcTemplate Template;
        public bool Summoned;
        public bool StructureControlled = false;

        public L2Npc(int objectId, NpcTemplate template, L2Spawn spawn) : base(objectId, template)
        {
            Template = template;
            Template = template;
            Name = template.Name;
            InitializeCharacterStatus();
            this.spawn = spawn;
            //CStatsInit();
            //CurHp = 100;
            //CurCp = 100;
            //CurMp = 100;
            //MaxCp = 100;
            //MaxHp = 100;
            //MaxMp = 100;
        }

        protected L2Spawn spawn;

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
                player.SendPacket(new MoveToPawn(player, this, 150));
                if (Template.Type == "L2Monster")
                {
                    Log.Debug("Attack Monester By L2NPC");
                    player.DoAttack(this);
                }
                player.SendActionFailed();
            }
        }
        public override void OnActionShift(L2Player player)
        {
            if (player.Target != this)
            {
                player.SetTarget(this);
                player.SendPacket(new MyTargetSelected(ObjId, 0));
                return;
            }
            player.MoveTo(X, Y, Z);
            player.SendPacket(new MoveToPawn(player, this, 150));

            ShowNPCInfo(player);
        }

        public virtual void OnTeleportRequest(L2Player player)
        {

        }

        public void UseTeleporter(L2Player player, int type, int entryId)
        {

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
            //if (player.Clan == null)
            //{
            //    player.SendSystemMessage(SystemMessage.SystemMessageId.YouDoNotHaveTheRightToUseClanWarehouse);
            //    player.SendActionFailed();
            //    return;
            //}

            //if (player.Clan.Level == 0)
            //{
            //    player.SendSystemMessage(SystemMessage.SystemMessageId.OnlyLevel1ClanOrHigherCanUseWarehouse);
            //    player.SendActionFailed();
            //    return;
            //}

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
            //if (player.Clan == null)
            //{
            //    player.SendSystemMessage(SystemMessage.SystemMessageId.YouDoNotHaveTheRightToUseClanWarehouse);
            //    player.SendActionFailed();
            //}
            //else
            //{
            //    if (player.Clan.Level != 0)
            //        return;

            //    player.SendSystemMessage(SystemMessage.SystemMessageId.OnlyLevel1ClanOrHigherCanUseWarehouse);
            //    player.SendActionFailed();
            //}
        }

        public void ShowSkillLearn(L2Player player, bool backward)
        {
            player.SendMessage("I cannot teach you anything.");
        }

        public override void BroadcastUserInfo()
        {
            // TODO: Sends to all players on the server. It is not right
            foreach (L2Player pl in L2World.Instance.GetPlayers())
            {
                pl.SendPacket(new NpcInfo(this));
            }
        }

        public override void BroadcastUserInfoToObject(L2Object l2Object)
        {
            l2Object.SendPacket(new NpcInfo(this));
        }

        public override void OnSpawn(bool notifyOthers = true)
        {
            if (notifyOthers)
                BroadcastUserInfo();
            StartAi();
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
            _corpseTimer.Stop();
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

        public void ShowNPCInfo(L2Player player)
        {
            NpcHtmlMessage html = new NpcHtmlMessage(player, "./html/admin/npcinfo.htm", ObjId);

            html.Replace("%objid%", ObjId);
            html.Replace("%class%", "null");
            html.Replace("%id%", NpcId);
            html.Replace("%lvl%", Level);
            html.Replace("%name%", Name);
            html.Replace("%tmplid%", Template.IdTemplate);
            html.Replace("%aggro%", Attackable > 0 ? Template.AggroRange : 0);
            html.Replace("%corpse%", Template.CorpseTime);
            //html.replace("%enchant%", String.valueOf(getTemplate().getEnchantEffect()));
            html.Replace("%hp%", CharStatus.CurrentHp);
            html.Replace("%hpmax%", MaxHp);
            html.Replace("%mp%", CharStatus.CurrentMp);
            html.Replace("%mpmax%", MaxMp);
            html.Replace("%patk%", Template.BasePAtk);
            html.Replace("%matk%", Template.BaseMAtk);
            html.Replace("%mdef%", Template.BaseMDef);
            html.Replace("%pdef%", Template.BasePDef);
            html.Replace("%accu%", "Not Defined");
            html.Replace("%evas%", "Not Defined");
            html.Replace("%crit%", Template.BaseCritRate);
            html.Replace("%aspd%", Template.BasePAtkSpd);
            html.Replace("%cspd%", "Not Defined");
            html.Replace("%str%", Str);
            html.Replace("%con%", Con);
            html.Replace("%int%", Int);
            html.Replace("%wit%", Wit);
            html.Replace("%men%", Men);
            html.Replace("%loc%", $"{X} {Y} {Z}");
            //         html.replace("%dist%", String.valueOf((int)Math.sqrt(player.getDistanceSq(this))));

            //         // byte attackAttribute = ((L2Character)this).getAttackElement();
            //         html.replace("%ele_atk_value%", "%todo%" /* String.valueOf(((L2Character)this).getAttackElementValue(attackAttribute)) */);
            //         html.replace("%ele_dfire%", String.valueOf(((L2Character)this).getDefenseElementValue((byte)2)));
            //         html.replace("%ele_dwater%", String.valueOf(((L2Character)this).getDefenseElementValue((byte)3)));
            //         html.replace("%ele_dwind%", String.valueOf(((L2Character)this).getDefenseElementValue((byte)1)));
            //         html.replace("%ele_dearth%", String.valueOf(((L2Character)this).getDefenseElementValue((byte)4)));
            //         html.replace("%ele_dholy%", String.valueOf(((L2Character)this).getDefenseElementValue((byte)5)));
            //         html.replace("%ele_ddark%", String.valueOf(((L2Character)this).getDefenseElementValue((byte)6)));

            //         if (getSpawn() != null)
            //         {
            //             html.replace("%spawn%", getSpawn().getLocx() + " " + getSpawn().getLocy() + " " + getSpawn().getLocz());
            //             html.replace("%loc2d%", String.valueOf((int)Math.sqrt(((L2Character)this).getPlanDistanceSq(getSpawn().getLocx(), getSpawn().getLocy()))));
            //             html.replace("%loc3d%", String.valueOf((int)Math.sqrt(((L2Character)this).getDistanceSq(getSpawn().getLocx(), getSpawn().getLocy(), getSpawn().getLocz()))));
            //             html.replace("%resp%", String.valueOf(getSpawn().getRespawnDelay() / 1000));
            //         }
            //         else
            //         {
            //             html.replace("%spawn%", "<font color=FF0000>null</font>");
            //             html.replace("%loc2d%", "<font color=FF0000>--</font>");
            //             html.replace("%loc3d%", "<font color=FF0000>--</font>");
            //             html.replace("%resp%", "<font color=FF0000>--</font>");
            //         }

            //         if (hasAI())
            //         {
            //             html.replace("%ai_intention%", "<tr><td><table width=270 border=0><tr><td width=100><font color=FFAA00>Intention:</font></td><td align=right width=170>" + String.valueOf(getAI().getIntention().name()) + "</td></tr></table></td></tr>");
            //             html.replace("%ai%", "<tr><td><table width=270 border=0><tr><td width=100><font color=FFAA00>AI</font></td><td align=right width=170>" + getAI().getClass().getSimpleName() + "</td></tr></table></td></tr>");
            //             html.replace("%ai_type%", "<tr><td><table width=270 border=0><tr><td width=100><font color=FFAA00>AIType</font></td><td align=right width=170>" + String.valueOf(getAiType()) + "</td></tr></table></td></tr>");
            //             html.replace("%ai_clan%", "<tr><td><table width=270 border=0><tr><td width=100><font color=FFAA00>Clan & Range:</font></td><td align=right width=170>" + String.valueOf(getClan()) + " " + String.valueOf(getClanRange()) + "</td></tr></table></td></tr>");
            //             html.replace("%ai_enemy_clan%", "<tr><td><table width=270 border=0><tr><td width=100><font color=FFAA00>Enemy & Range:</font></td><td align=right width=170>" + String.valueOf(getEnemyClan()) + " " + String.valueOf(getEnemyRange()) + "</td></tr></table></td></tr>");
            //         }
            //         else
            //         {
            //             html.replace("%ai_intention%", "");
            //             html.replace("%ai%", "");
            //             html.replace("%ai_type%", "");
            //             html.replace("%ai_clan%", "");
            //             html.replace("%ai_enemy_clan%", "");
            //         }

            //         if (this instanceof L2MerchantInstance)
            //	html.replace("%butt%", "<button value=\"Shop\" action=\"bypass -h admin_showShop " + String.valueOf(getTemplate().getNpcId()) + "\" width=65 height=19 back=\"L2UI_ch3.smallbutton2_over\" fore=\"L2UI_ch3.smallbutton2\">");
            //else
            //	html.replace("%butt%", "");

            player.SendPacket(html);
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
        }
    }
}