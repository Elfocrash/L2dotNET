using System.Threading.Tasks;
using System.Timers;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;
using L2dotNET.Templates;
using L2dotNET.Tools;
using L2dotNET.World;
using L2dotNET.Tables;
using NLog;

namespace L2dotNET.Models.Npcs
{
    public class L2Npc : L2Character
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public new NpcTemplate Template;
        public bool Summoned;
        public bool StructureControlled = false;

        public L2Npc(SpawnTable spawnTable, int objectId, NpcTemplate template, L2Spawn spawn) : base(objectId, template)
        {
            Template = template;
            Name = Template.Name;
            InitializeCharacterStatus();
            CharStatus.SetCurrentHp(MaxHp);
            CharStatus.SetCurrentMp(MaxMp);
            this.spawn = spawn;
            //CStatsInit();
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
        //    AIProcessor.diaLog.Add("fnHi", "lector001.htm");
        //    AIProcessor.diaLog.Add("fnFeudInfo", "gludio_feud_manager001.htm");
        //    AIProcessor.diaLog.Add("fnNoFeudInfo", "farm_messenger002.htm");
        //    AIProcessor.myself = this;
        //}

        public override async Task NotifyActionAsync(L2Player player)
        {
            double dis = Calcs.CalculateDistance(player, this, true);
            await TryMoveToAsync(X, Y, Z);
        }

        public int NpcId => Template.NpcId;

        public int NpcHashId => Template.NpcId + 1000000;

        public new byte IsRunning()
        {
            return 1;
        }

        public byte IsAlikeDead()
        {
            return 0;
        }

        public override async Task OnActionAsync(L2Player player)
        {
            if (player.Target != this)
                player.SetTargetAsync(this);
            else
            {
                await player.MoveToAsync(X, Y, Z);
                await player.SendPacketAsync(new MoveToPawn(player, this, 150));
                if (Template.Type == "L2Monster")
                {
                    Log.Debug("Attack Monester By L2NPC");
                    await player.DoAttackAsync(this);
                }
                await player.SendActionFailedAsync();
            }
        }

        public override async Task OnActionShiftAsync(L2Player player)
        {
            if (player.Target != this)
            {
                player.SetTargetAsync(this);
                return;
            }
            await player.MoveToAsync(X, Y, Z);
            await player.SendPacketAsync(new MoveToPawn(player, this, 150));

            await ShowNPCInfoAsync(player);
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

        public override async Task OnForcedAttackAsync(L2Player player)
        {
            
            if (player.Target != this)
            {
                player.SetTargetAsync(this);
                return;
            }

            await player.MoveToAsync(X, Y, Z);
            await player.SendPacketAsync(new MoveToPawn(player, this, 150));
        }

        public async Task ShowSkillLearnAsync(L2Player player, bool backward)
        {
            await player.SendMessageAsync("I cannot teach you anything.");
        }

        public override async Task BroadcastUserInfoAsync()
        {
            // TODO: Sends to all players on the server. It is not right
            foreach (L2Player pl in L2World.GetPlayers())
            {
                await pl.SendPacketAsync(new NpcInfo(this));
            }
        }

        public override async Task BroadcastUserInfoToObjectAsync(L2Object l2Object)
        {
            await l2Object.SendPacketAsync(new NpcInfo(this));
        }

        public override async Task OnSpawnAsync(bool notifyOthers = true)
        {
            if (notifyOthers)
                await BroadcastUserInfoAsync();
            StartAi();
        }

        private Timer _corpseTimer;
        public int ResidenceId;

        public override async Task DoDieAsync(L2Character killer)
        {
            await base.DoDieAsync(killer);

            if (Template.CorpseTime <= 0)
            {
                return;
            }
            _corpseTimer = new Timer(Template.CorpseTime * 1000);
            _corpseTimer.Elapsed += RemoveCorpse;
            _corpseTimer.Enabled = true;
        }

        private async void RemoveCorpse(object sender, ElapsedEventArgs e)
        {
            _corpseTimer.Enabled = false;
            _corpseTimer.Stop();
            await BroadcastPacketAsync(new DeleteObject(ObjectId));
            L2World.RemoveObject(this);
        }

        public override async Task DeleteByForceAsync()
        {
            if ((_corpseTimer != null) && _corpseTimer.Enabled)
                _corpseTimer.Enabled = false;

            await base.DeleteByForceAsync();
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

        public async Task ShowNPCInfoAsync(L2Player player)
        {
            NpcHtmlMessage html = new NpcHtmlMessage(player, "./html/admin/npcinfo.htm", ObjectId);

            html.Replace("%objid%", ObjectId);
            html.Replace("%class%", "null");
            html.Replace("%id%", NpcId);
            html.Replace("%lvl%", Level);
            html.Replace("%name%", Name);
            html.Replace("%tmplid%", Template.IdTemplate);
            html.Replace("%aggro%", Attackable > 0 ? Template.AggroRange : 0);
            html.Replace("%corpse%", Template.CorpseTime);
            html.Replace("%enchant%", Template.EnchantEffect);
            html.Replace("%hp%", CharStatus.CurrentHp);
            html.Replace("%hpmax%", MaxHp);
            html.Replace("%mp%", CharStatus.CurrentMp);
            html.Replace("%mpmax%", MaxMp);
            html.Replace("%patk%", Template.BasePAtk);
            html.Replace("%matk%", Template.BaseMAtk);
            html.Replace("%mdef%", Template.BaseMDef);
            html.Replace("%pdef%", Template.BasePDef);
            html.Replace("%accu%", CharacterStat.Accuracy);
            html.Replace("%evas%", CharacterStat.EvasionRate(this));
            html.Replace("%crit%", Template.BaseCritRate);
            html.Replace("%aspd%", Template.BasePAtkSpd);
            html.Replace("%cspd%", CharacterStat.MAttackSpeed);
            html.Replace("%rspd%", Template.BaseRunSpd);
            html.Replace("%str%", Str);
            html.Replace("%con%", Con);
            html.Replace("%dex%", Dex);
            html.Replace("%int%", Int);
            html.Replace("%wit%", Wit);
            html.Replace("%men%", Men);
            html.Replace("%loc%", $"{X} {Y} {Z}");
            html.Replace("%dist%", player.GetPlanDistanceSq(X,Y));
            //         // byte attackAttribute = ((L2Character)this).getAttackElement();
            //         html.replace("%ele_atk_value%", "%todo%" /* String.valueOf(((L2Character)this).getAttackElementValue(attackAttribute)) */);
            //         html.replace("%ele_dfire%", String.valueOf(((L2Character)this).getDefenseElementValue((byte)2)));
            //         html.replace("%ele_dwater%", String.valueOf(((L2Character)this).getDefenseElementValue((byte)3)));
            //         html.replace("%ele_dwind%", String.valueOf(((L2Character)this).getDefenseElementValue((byte)1)));
            //         html.replace("%ele_dearth%", String.valueOf(((L2Character)this).getDefenseElementValue((byte)4)));
            //         html.replace("%ele_dholy%", String.valueOf(((L2Character)this).getDefenseElementValue((byte)5)));
            //         html.replace("%ele_ddark%", String.valueOf(((L2Character)this).getDefenseElementValue((byte)6)));

            if (spawn != null)
            {
                html.Replace("%spawn%", $"{spawn.Location.X} {spawn.Location.Y} {spawn.Location.Z}");
                html.Replace("%loc2d%", player.GetPlanDistanceSq(spawn.Location.Y,spawn.Location.X));
                html.Replace("%loc3d%", "<font color=FF0000>--</font>");
                //html.Replace("%loc3d%", player.getDistanceSq(spawn.Location.X,spawn.Location.Y,spawn.Location.Z); -Not implemented
                html.Replace("%resp%", spawn.Location.RespawnDelay / 1000);
            }
            else
            {
                html.Replace("%spawn%", "<font color=FF0000>null</font>");
                html.Replace("%loc2d%", "<font color=FF0000>--</font>");
                html.Replace("%loc3d%", "<font color=FF0000>--</font>");
                html.Replace("%resp%", "<font color=FF0000>--</font>");
            }

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

            if (Template.GetType().ToString() == "L2dotNET.Models.Npcs.L2Merchant")
                html.Replace("%butt%", "<button value=\"Shop\" action=\"bypass -h admin_showShop " + NpcId + "\" width=65 height=19 back=\"L2UI_ch3.smallbutton2_over\" fore=\"L2UI_ch3.smallbutton2\">");
            else
            html.Replace("%butt%", "");

            await player.SendPacketAsync(html);
        }
        public virtual int Attackable => 0;

        public override double Radius => Template.CollisionRadius == 0 ? 12 : Template.CollisionRadius;

        public override double Height => Template.CollisionHeight == 0 ? 22 : Template.CollisionHeight;

        public override string AsString()
        {
            return $"L2Npc:{Template.NpcId}; id {ObjectId}";
        }

        public void CreateOnePrivateEx(int npcId, string aiType, int x, int y, int z) { }

        public async Task CastBuffForQuestRewardAsync(L2Character cha, int skillId)
        {
            await cha.SendMessageAsync($"L2Npc.CastBuffForQuestReward {skillId}");
        }
    }
}