using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Timers;
using log4net;
using L2dotNET.GameService.Enums;
using L2dotNET.GameService.Model.Communities;
using L2dotNET.GameService.Model.Inventory;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Npcs;
using L2dotNET.GameService.Model.Npcs.Cubic;
using L2dotNET.GameService.Model.Npcs.Decor;
using L2dotNET.GameService.Model.Playable;
using L2dotNET.GameService.Model.Player.AI;
using L2dotNET.GameService.Model.Player.Partials;
using L2dotNET.GameService.Model.Player.Transformation;
using L2dotNET.GameService.Model.Quests;
using L2dotNET.GameService.Model.Skills;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.GameService.Model.Skills2.Effects;
using L2dotNET.GameService.Model.Vehicles;
using L2dotNET.GameService.Network;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tables;
using L2dotNET.GameService.Tables.Multisell;
using L2dotNET.GameService.Templates;
using L2dotNET.GameService.Tools;
using L2dotNET.GameService.World;
using L2dotNET.Models;
using L2dotNET.Services.Contracts;
using L2dotNET.Utility;
using Ninject;

namespace L2dotNET.GameService.Model.Player
{
    [Synchronization]
    public class L2Player : L2Character
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(L2Player));

        [Inject]
        public IPlayerService playerService
        {
            get { return GameServer.Kernel.Get<IPlayerService>(); }
        }

        public string AccountName { get; set; }
        public ClassId ClassId { get; set; }
        public L2PartyRoom PartyRoom { get; set; }
        public L2Party Party { get; set; }
        public byte Sex { get; set; }
        public int HairStyle { get; set; }
        public int HairColor { get; set; }
        public int Face { get; set; }
        public bool WhisperBlock { get; set; } = false;
        public GameClient Gameclient { get; set; }
        public long Exp { get; set; }
        public long ExpOnDeath { get; set; }
        public long ExpAfterLogin { get; set; }
        public int SP { get; set; }
        public override int MaxHp { get; set; }
        public override double CurHp { get; set; }
        public override int MaxCp { get; set; }
        public override double CurCp { get; set; }
        public override int MaxMp { get; set; }
        public override double CurMp { get; set; }
        public int Karma;
        public int PvpKills { get; set; }
        public long DeleteTime { get; set; }
        public int CanCraft { get; set; }
        public int PkKills { get; set; }
        public int RecLeft { get; set; }
        public int RecHave { get; set; }
        public int AccessLevel { get; set; }
        public int Online { get; set; }
        public int OnlineTime { get; set; }
        public int CharSlot { get; set; }
        public int DeathPenaltyLevel { get; set; }
        public int CurrentWeight { get; set; }
        public double CollRadius { get; set; }
        public double CollHeight { get; set; }
        public int CursedWeaponLevel { get; set; }
        public short LastAccountSelection { get; set; }
        public long LastAccess { get; set; }
        public int ClanPrivs { get; set; }
        public int WantsPeace { get; set; }
        public int IsIn7sDungeon { get; set; }
        public int PunishLevel { get; set; }
        public int PunishTimer { get; set; }
        public int PowerGrade { get; set; }
        public int Nobless { get; set; }
        public int Hero { get; set; }
        public int Subpledge { get; set; }
        public long LastRecomDate { get; set; }
        public int LevelJoinedAcademy { get; set; }
        public int Apprentice { get; set; }
        public int Sponsor { get; set; }
        public int VarkaKetraAlly { get; set; }
        public long ClanJoinExpiryTime { get; set; }
        public int ClanCreateExpiryTime { get; set; }
        public PcInventory Inventory { get; set; }


        public L2Player RestorePlayer(int id, GameClient client)
        {
            L2Player player = new L2Player();
            player.ObjId = id;

            player.Gameclient = client;

            PlayerModel playerModel = playerService.GetAccountByLogin(id);

            player.Name = playerModel.Name;
            player.Title = playerModel.Title;
            player.Level = (byte)playerModel.Level;
            player.CurHp = playerModel.CurHp;
            player.CurMp = playerModel.CurMp;
            player.CurCp = playerModel.CurCp;

            player.Face = playerModel.Face;
            player.HairStyle = playerModel.HairStyle;
            player.HairColor = playerModel.HairColor;
            player.Sex = (byte)playerModel.Sex;

            player.X = playerModel.X;
            player.Y = playerModel.Y;
            player.Z = playerModel.Z;
            player.Heading = playerModel.Heading;

            player.Exp = playerModel.Exp;
            player.ExpOnDeath = playerModel.ExpBeforeDeath;

            player.SP = playerModel.Sp;
            player.Karma = playerModel.Karma;
            player.PvpKills = playerModel.PvpKills;
            player.PkKills = playerModel.PkKills;
            //byte bclass = (byte)playerModel.BaseClass;
            //byte aclass = (byte)playerModel.ClassId;

            player.BaseClass = CharTemplateTable.Instance.GetTemplate(playerModel.BaseClass);
            player.ActiveClass = CharTemplateTable.Instance.GetTemplate(playerModel.ClassId);

            player.RecLeft = playerModel.RecLeft;
            player.RecHave = playerModel.RecHave;

            player.CharSlot = playerModel.CharSlot;
            player.DeathPenaltyLevel = playerModel.DeathPenaltyLevel;
            player.ClanId = playerModel.ClanId;
            player.ClanPrivs = playerModel.ClanPrivs;

            player.Penalty_ClanCreate = playerModel.ClanCreateExpiryTime.ToString();
            player.Penalty_ClanJoin = playerModel.ClanJoinExpiryTime.ToString();

            player.Inventory = new PcInventory(this);

            player.CStatsInit();

            //restoreItems(player);

            return player;
        }

        public static L2Player create()
        {
            L2Player player = new L2Player();
            player.ObjId = IdFactory.Instance.nextId();

            //player.Inventory = new InvPC();
            //player.Inventory._owner = player;

            return player;
        }



        public void UpdatePlayer()
        {
            PlayerModel playerModel = new PlayerModel { ObjectId = ObjId, Level = Level, MaxHp = MaxHp, CurHp = (int)CurHp, MaxCp = MaxCp, CurCp = (int)CurCp, MaxMp = MaxMp, CurMp = (int)CurMp, Face = Face, HairStyle = HairStyle, HairColor = HairColor, Sex = Sex, Heading = Heading, X = X, Y = Y, Z = Z, Exp = Exp, ExpBeforeDeath = ExpOnDeath, Sp = SP, Karma = Karma, PvpKills = PvpKills, PkKills = PkKills, ClanId = ClanId, Race = (int)BaseClass.ClassId.ClassRace, ClassId = (int)ActiveClass.ClassId.Id, BaseClass = (int)BaseClass.ClassId.Id, DeleteTime = DeleteTime, CanCraft = CanCraft, Title = Title, RecHave = RecHave, RecLeft = RecLeft, AccessLevel = AccessLevel, ClanPrivs = ClanPrivs, WantsPeace = WantsPeace, IsIn7SDungeon = IsIn7sDungeon, PunishLevel = PunishLevel, PunishTimer = PunishTimer, PowerGrade = PowerGrade, Nobless = Nobless, Sponsor = Sponsor, VarkaKetraAlly = VarkaKetraAlly, ClanCreateExpiryTime = ClanCreateExpiryTime, ClanJoinExpiryTime = ClanJoinExpiryTime, DeathPenaltyLevel = DeathPenaltyLevel };

            playerService.UpdatePlayer(playerModel);
        }

        public void Termination()
        {
            if (IsRestored)
            {
                if (Party != null)
                    Party.Leave(this);

                if (Summon != null)
                    Summon.unSummon();

                UpdatePlayer();
                //saveInventory();

                L2World.Instance.RemoveObject(this);
            }
        }

        public int INT => ActiveClass.BaseINT;

        public int STR => ActiveClass.BaseSTR;

        public int CON => ActiveClass.BaseCON;

        public int MEN => ActiveClass.BaseMEN;

        public int DEX => ActiveClass.BaseDEX;

        public int WIT => ActiveClass.BaseWIT;

        public byte Builder = 1;
        public byte Noblesse = 0;
        public byte Heroic = 0;

        public byte _privateStoreType = 0;

        public byte getPrivateStoreType()
        {
            return _privateStoreType;
        }

        public override int AllianceCrestId
        {
            get { return Clan == null ? 0 : Clan.AllianceCrestId; }
        }

        public override int AllianceId
        {
            get { return Clan == null ? 0 : Clan.AllianceID; }
        }

        public override int ClanId
        {
            get { return Clan == null ? 0 : Clan.ClanID; }
            set { Clan = ClanTable.Instance.GetClan(value); }
        }

        public override int ClanCrestId
        {
            get { return Clan == null ? 0 : Clan.CrestID; }
        }

        public int getTitleColor()
        {
            return 0xFFFF77;
        }

        public int getNameColor()
        {
            return 0xFFFFFF;
        }

        internal uint GetFishz()
        {
            return 0;
        }

        internal uint GetFishy()
        {
            return 0;
        }

        internal uint GetFishx()
        {
            return 0;
        }

        internal bool isFishing()
        {
            return false;
        }

        public override void SendPacket(GameServerNetworkPacket pk)
        {
            Gameclient.SendPacket(pk);
        }

        private ActionFailed af;

        public override void SendActionFailed()
        {
            if (af == null)
                af = new ActionFailed();

            SendPacket(af);
        }

        public override void SendSystemMessage(SystemMessage.SystemMessageId msgId)
        {
            SendPacket(new SystemMessage(msgId));
        }

        public int _penaltyWeight;
        public int _penalty_grade = 0;

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
            else
                player.SendActionFailed();
        }

        public override void SendMessage(string p)
        {
            SendPacket(new SystemMessage(SystemMessage.SystemMessageId.S1).AddString(p));
        }

        public int _currentFocusEnergy = 0;

        public int getForceIncreased()
        {
            return _currentFocusEnergy;
        }

        public TSkill getSkill(int magicId)
        {
            return Skills.ContainsKey(magicId) ? Skills[magicId] : null;
        }

        public override bool IsCastingNow()
        {
            if (petSummonTime != null)
                return petSummonTime.Enabled;

            if (nonpetSummonTime != null)
                return nonpetSummonTime.Enabled;

            return base.IsCastingNow();
        }

        public void updateReuse()
        {
            SendPacket(new SkillCoolTime(this));
        }

        public void castSkill(TSkill skill, bool ctrlPressed, bool shiftPressed)
        {
            if (IsCastingNow())
            {
                SendActionFailed();
                return;
            }

            if (isSittingInProgress() || isSitting())
            {
                SendActionFailed();
                return;
            }

            if (!skill.ConditionOk(this))
            {
                SendActionFailed();
                return;
            }

            L2Character target = skill.getTargetCastId(this);

            if (target == null)
            {
                SendSystemMessage(SystemMessage.SystemMessageId.TARGET_CANT_FOUND);
                SendActionFailed();
                return;
            }

            if (skill.cast_range != -1)
            {
                double dis = Calcs.calculateDistance(this, target, true);
                if (dis > skill.cast_range)
                {
                    TryMoveTo(target.X, target.Y, target.Z);
                    SendActionFailed();
                    return;
                }
            }

            if (skill.reuse_delay > 0)
                if (Reuse.ContainsKey(skill.skill_id))
                {
                    TimeSpan ts = Reuse[skill.skill_id].stopTime - DateTime.Now;

                    if (ts.TotalMilliseconds > 0)
                    {
                        if (ts.TotalHours > 0)
                        {
                            SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.S2_HOURS_S3_MINUTES_S4_SECONDS_REMAINING_IN_S1_REUSE_TIME);
                            sm.AddSkillName(skill.skill_id, skill.level);
                            sm.AddNumber(ts.Hours);
                            sm.AddNumber(ts.Minutes);
                            sm.AddNumber(ts.Seconds);
                            SendPacket(sm);
                        }
                        else if (ts.TotalMinutes > 0)
                        {
                            SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.S2_MINUTES_S3_SECONDS_REMAINING_IN_S1_REUSE_TIME);
                            sm.AddSkillName(skill.skill_id, skill.level);
                            sm.AddNumber(ts.Minutes);
                            sm.AddNumber(ts.Seconds);
                            SendPacket(sm);
                        }
                        else
                        {
                            SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.S2_SECONDS_REMAINING_IN_S1_REUSE_TIME);
                            sm.AddSkillName(skill.skill_id, skill.level);
                            sm.AddNumber(ts.Seconds);
                            SendPacket(sm);
                        }

                        SendActionFailed();
                        return;
                    }
                }

            if ((skill.mp_consume1 > 0) || (skill.mp_consume2 > 0))
                if (CurMp < skill.mp_consume1 + skill.mp_consume2)
                {
                    SendSystemMessage(SystemMessage.SystemMessageId.NOT_ENOUGH_MP);
                    SendActionFailed();
                    return;
                }

            if (skill.hp_consume > 0)
                if (CurHp < skill.hp_consume)
                {
                    SendSystemMessage(SystemMessage.SystemMessageId.NOT_ENOUGH_HP);
                    SendActionFailed();
                    return;
                }

            //if (skill.ConsumeItemId != 0)
            //{
            //    long count = Inventory.getItemCount(skill.ConsumeItemId);
            //    if (count < skill.ConsumeItemCount)
            //    {
            //        sendPacket(new SystemMessage(SystemMessage.SystemMessageId.S1_CANNOT_BE_USED).AddSkillName(skill.skill_id, skill.level));
            //        sendActionFailed();
            //        return;
            //    }
            //}

            byte blowOk = 0;
            if (skill.effects.Count > 0)
            {
                bool fail = false;
                foreach (TEffect ef in skill.effects)
                {
                    if (!ef.canUse(this))
                    {
                        SendActionFailed();
                        fail = true;
                        break;
                    }

                    if (ef is i_fatal_blow && (blowOk == 0))
                        blowOk = ((i_fatal_blow)ef).success(target);
                }

                if (fail)
                    return;
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
                updateReuse();
            }

            SendPacket(new SystemMessage(SystemMessage.SystemMessageId.USE_S1).AddSkillName(skill.skill_id, skill.level));

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

            //if (skill.ConsumeItemId != 0)
            //   Inventory.destroyItem(skill.ConsumeItemId, skill.ConsumeItemCount, true, true);

            int hitTime = skill.skill_hit_time;

            int hitT = hitTime > 0 ? (int)(hitTime * 0.95) : 0;
            CurrentCast = skill;

            if (hitTime > 0)
                SendPacket(new SetupGauge(ObjId, SetupGauge.SG_color.blue, hitTime - 20));

            BroadcastPacket(new MagicSkillUse(this, target, skill, hitTime == 0 ? 20 : hitTime, blowOk));
            if (hitTime > 50)
            {
                if (CastTime == null)
                {
                    CastTime = new Timer();
                    CastTime.Elapsed += new ElapsedEventHandler(castEnd);
                }

                CastTime.Interval = hitT;
                CastTime.Enabled = true;
            }
            else
                castEnd();
        }

        private void castEnd(object sender = null, ElapsedEventArgs e = null)
        {
            if (CurrentCast.mp_consume2 > 0)
            {
                if (CurMp < CurrentCast.mp_consume2)
                {
                    SendSystemMessage(SystemMessage.SystemMessageId.NOT_ENOUGH_MP);
                    SendActionFailed();

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
                    SendSystemMessage(SystemMessage.SystemMessageId.DIST_TOO_FAR_CASTING_STOPPED);
                    SendActionFailed();

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
                CastTime.Enabled = false;
        }

        public bool _diet = false;

        public override void UpdateMagicEffectIcons()
        {
            MagicEffectIcons m = new MagicEffectIcons();
            PartySpelled p = null;

            if (Party != null)
                p = new PartySpelled(this);

            List<AbnormalEffect> nulled = new List<AbnormalEffect>();
            foreach (AbnormalEffect ei in Effects.Where(ei => ei != null))
                if (ei.active == 1)
                {
                    int time = ei.getTime();
                    m.addIcon(ei.id, ei.lvl, time);

                    if (p != null)
                        p.addIcon(ei.id, ei.lvl, time);
                }
                else
                    nulled.Add(ei);

            lock (Effects)
                foreach (AbnormalEffect ei in nulled)
                    Effects.Remove(ei);

            nulled.Clear();
            SendPacket(m);

            if ((p != null) && (Party != null))
                Party.broadcastToMembers(p);
        }

        public void onGameInit()
        {
            CStatsInit();
            CharacterStat.setTemplate(ActiveClass);
            ExpAfterLogin = 0;
        }

        public List<QuestInfo> _quests = new List<QuestInfo>();

        public void quest_Talk(L2Npc npc, int questId)
        {
            foreach (QuestInfo qi in _quests.Where(qi => !qi.completed))
                qi._template.onTalkToNpc(this, npc, qi.stage);
        }

        public void ShowHtm(string file, L2Object o)
        {
            if (file.EndsWithIgnoreCase(".htm"))
            {
                SendPacket(new NpcHtmlMessage(this, file, o.ObjId, 0));
                if (o is L2Npc)
                    FolkNpc = (L2Npc)o;
            }
            else
                ShowHtmPlain(file, o);
        }

        public void ShowHtm(string file, L2Npc npc, int questId)
        {
            if (file.EndsWithIgnoreCase(".htm"))
            {
                NpcHtmlMessage htm = new NpcHtmlMessage(this, file, npc.ObjId, 0);
                htm.replace("<?quest_id?>", questId);
                SendPacket(htm);
                FolkNpc = npc;
            }
            else
                ShowHtmPlain(file, npc);
        }

        public bool questComplete(int questId)
        {
            return _quests.Where(qi => qi.id == questId).Select(qi => qi.completed).FirstOrDefault();
        }

        public bool questInProgress(int questId)
        {
            return _quests.Any(qi => (qi.id == questId) && !qi.completed);
        }

        public void questAccept(QuestInfo qi)
        {
            _quests.Add(qi);
            SendPacket(new PlaySound("ItemSound.quest_accept"));
            sendQuestList();

            //SQL_Block sqb = new SQL_Block("user_quests");
            //sqb.param("ownerId", ObjID);
            //sqb.param("qid", qi.id);
            //sqb.sql_insert(false);
        }

        public void ShowHtmPlain(string plain, L2Object o)
        {
            SendPacket(new NpcHtmlMessage(this, plain, o == null ? -1 : o.ObjId, true));
            if (o is L2Npc)
                FolkNpc = (L2Npc)o;
        }

        public void changeQuestStage(int questId, int stage)
        {
            foreach (QuestInfo qi in _quests.Where(qi => qi.id == questId))
            {
                qi.stage = stage;
                SendPacket(new PlaySound("ItemSound.quest_middle"));

                //SQL_Block sqb = new SQL_Block("user_quests");
                //sqb.param("qstage", stage);
                //sqb.where("ownerId", ObjID);
                //sqb.where("qid", qi.id);
                //sqb.sql_update(false);
                break;
            }

            sendQuestList();
        }

        public List<QuestInfo> getAllActiveQuests()
        {
            return _quests.Where(qi => !qi.completed).ToList();
        }

        public void sendQuestList()
        {
            SendPacket(new QuestList(this));
        }

        public void addExpSp(int exp, int sp, bool msg)
        {
            SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.YOU_EARNED_S1_EXP_AND_S2_SP);
            sm.AddNumber(exp);
            sm.AddNumber(sp);
            SendPacket(sm);

            Exp += exp;
            SP += sp;

            StatusUpdate su = new StatusUpdate(ObjId);
            su.add(StatusUpdate.EXP, (int)Exp);
            su.add(StatusUpdate.SP, SP);
            SendPacket(su);
        }

        public void finishQuest(int questId)
        {
            foreach (QuestInfo qi in _quests.Where(qi => qi.id == questId))
            {
                if (!qi._template.repeatable)
                {
                    qi.completed = true;
                    qi._template = null;

                    //SQL_Block sqb = new SQL_Block("user_quests");
                    //sqb.param("qfin", 1);
                    //sqb.where("ownerId", ObjID);
                    //sqb.where("iclass", ActiveClass.id);
                    //sqb.where("qid", qi.id);
                    //sqb.sql_update(false);
                }
                else
                {
                    lock (_quests)
                    {
                        _quests.Remove(qi);
                    }
                }

                SendPacket(new PlaySound("ItemSound.quest_finish"));
                break;
            }

            sendQuestList();
        }

        public void db_restoreSkills()
        {
            //MySqlConnection connection = SQLjec.getInstance().conn();
            //MySqlCommand cmd = connection.CreateCommand();

            //connection.Open();

            //cmd.CommandText = "SELECT * FROM user_skills WHERE ownerId=" + ObjID + " AND iclass=" + ActiveClass.id;
            //cmd.CommandType = CommandType.Text;

            //MySqlDataReader reader = cmd.ExecuteReader();

            //TSkillTable st = TSkillTable.getInstance();
            //while (reader.Read())
            //{
            //    int id = reader.GetInt32("id");
            //    int lvl = reader.GetInt32("lvl");
            //    TSkill skill = st.get(id, lvl);
            //    if (skill != null)
            //    {
            //        addSkill(skill, false, false);
            //    }
            //}

            //reader.Close();
            //connection.Close();
        }

        public override void AddSkill(TSkill newsk, bool updDb, bool update)
        {
            base.AddSkill(newsk, updDb, update);

            if (update)
                UpdateSkillList();

            if (updDb)
            {
                //SQL_Block sqb = new SQL_Block("user_skills");
                //sqb.param("ownerId", ObjID);
                //sqb.param("id", newsk.skill_id);
                //sqb.param("lvl", newsk.level);
                //sqb.param("iclass", ActiveClass.id);
                //sqb.sql_insert(false);
            }
        }

        public override void RemoveSkill(int id, bool updDb, bool update)
        {
            base.RemoveSkill(id, updDb, update);

            if (update)
                UpdateSkillList();

            if (updDb)
            {
                //SQL_Block sqb = new SQL_Block("user_skills");
                //sqb.where("ownerId", ObjID);
                //sqb.where("id", id);
                //sqb.where("iclass", ActiveClass.id);
                //sqb.sql_delete(false);
            }
        }

        public void stopQuest(QuestInfo qi, bool updDb)
        {
            if (updDb)
            {
                //SQL_Block sqb = new SQL_Block("user_quests");
                //sqb.where("ownerId", ObjID);
                //sqb.where("qid", qi.id);
                //sqb.where("iclass", ActiveClass.id);
                //sqb.sql_delete(false);
            }

            _quests.Remove(qi);
            SendPacket(new PlaySound("ItemSound.quest_giveup"));
        }

        public L2Clan Clan;

        public int ItemLimit_Inventory = 80,
                   ItemLimit_Selling = 5,
                   ItemLimit_Buying = 5,
                   ItemLimit_RecipeDwarven = 50,
                   ItemLimit_RecipeCommon = 50,
                   ItemLimit_Warehouse = 120,
                   ItemLimit_ClanWarehouse = 150,
                   ItemLimit_Extra = 0,
                   ItemLimit_Quest = 20;

        public L2Npc FolkNpc;
        public int last_x1 = -4;
        public int last_y1;

        public override void UpdateSkillList()
        {
            SendPacket(new SkillList(this, PBlockAct, PBlockSpell, PBlockSkill));
        }

        private Timer _timerTooFar;
        public string _locale = "en";

        public void timer()
        {
            _timerTooFar = new Timer(30 * 1000);
            _timerTooFar.Elapsed += new ElapsedEventHandler(_timeToFarTimerTask);
            _timerTooFar.Interval = 10000;
            _timerTooFar.Enabled = true;
        }

        public void _timeToFarTimerTask(object sender, ElapsedEventArgs e)
        {
            ValidateVisibleObjects(X, Y, true);
        }

        public int p_create_common_item = 0;
        public int p_create_item = 0;
        public List<L2Recipe> _recipeBook;

        public void registerRecipe(L2Recipe newr, bool updDb, bool cleanup)
        {
            if (_recipeBook == null)
                _recipeBook = new List<L2Recipe>();

            if (cleanup)
                _recipeBook.Clear();

            _recipeBook.Add(newr);

            if (updDb)
            {
                //SQL_Block sqb = new SQL_Block("user_recipes");
                //sqb.param("ownerId", ObjID);
                //sqb.param("recid", newr.RecipeID);
                //sqb.param("iclass", ActiveClass.id);
                //sqb.sql_insert(false);
            }
        }

        public void db_restoreRecipes()
        {
            //MySqlConnection connection = SQLjec.getInstance().conn();
            //MySqlCommand cmd = connection.CreateCommand();

            //connection.Open();

            //cmd.CommandText = "SELECT recid FROM user_recipes WHERE ownerId=" + ObjID + " AND iclass=" + ActiveClass.id + " ORDER BY tact ASC";
            //cmd.CommandType = CommandType.Text;

            //MySqlDataReader reader = cmd.ExecuteReader();

            //while (reader.Read())
            //{
            //    int recid = reader.GetInt32("recid");

            //    L2Recipe rec = RecipeTable.getInstance().getById(recid);
            //    if (rec != null)
            //    {
            //        if (_recipeBook == null)
            //            _recipeBook = new List<L2Recipe>();

            //        _recipeBook.Add(rec);
            //    }
            //}

            //reader.Close();
            //connection.Close();
        }

        public void unregisterRecipe(L2Recipe rec, bool updDb)
        {
            if (_recipeBook == null)
                return;

            lock (_recipeBook)
            {
                foreach (L2Recipe r in _recipeBook.Where(r => r.RecipeID == rec.RecipeID))
                {
                    if (updDb)
                    {
                        //MySqlConnection connection = SQLjec.getInstance().conn();
                        //MySqlCommand cmd = connection.CreateCommand();

                        //connection.Open();

                        //string query = string.Format(
                        //    "DELETE FROM user_recipes WHERE ownerId='{0}' AND recid='{1}' AND iclass='{2}'",
                        //    ObjID,
                        //    r.RecipeID,
                        //    ActiveClass.id);

                        //cmd.CommandText = query;
                        //cmd.CommandType = CommandType.Text;
                        //cmd.ExecuteNonQuery();

                        //connection.Close();
                    }

                    _recipeBook.Remove(r);

                    SendPacket(new RecipeBookItemList(this, rec._iscommonrecipe));
                    break;
                }
            }
        }

        public bool isAlikeDead()
        {
            return false;
        }

        public int getClanCrestLargeId()
        {
            return Clan == null ? 0 : Clan.LargeCrestID;
        }

        public List<L2Shortcut> _shortcuts = new List<L2Shortcut>();
        public int zoneId = -1;
        public int _obsx = -1;
        public int _obsy;
        public int _obsz;

        public void registerShortcut(int slot, int page, int type, int id, int level, int characterType)
        {
            lock (_shortcuts)
            {
                foreach (L2Shortcut sc in _shortcuts.Where(sc => (sc.Slot == slot) && (sc.Page == page)))
                {
                    _shortcuts.Remove(sc);

                    //SQL_Block sqb = new SQL_Block("user_shortcuts");
                    //sqb.where("ownerId", ObjID);
                    //sqb.where("classId", ActiveClass.id);
                    //sqb.where("slot", _slot);
                    //sqb.where("page", _page);
                    //sqb.sql_delete(false);
                    break;
                }
            }

            {
                L2Shortcut sc = new L2Shortcut(slot, page, type, id, level, characterType);
                _shortcuts.Add(sc);

                SendPacket(new ShortCutRegister(sc));

                //SQL_Block sqb = new SQL_Block("user_shortcuts");
                //sqb.param("ownerId", ObjID);
                //sqb.param("classId", ActiveClass.id);
                //sqb.param("slot", _slot);
                //sqb.param("page", _page);
                //sqb.param("type", _type);
                //sqb.param("id", _id);
                //sqb.param("lvl", _level);
                //sqb.param("cha", _characterType);
                //sqb.sql_insert(false);
            }
        }

        public int Souls;
        public int TransformID = 0;
        public L2Transform Transform;
        public int AgationID;

        public PcTemplate BaseClass;
        public PcTemplate ActiveClass;

        public bool subActive()
        {
            return ActiveClass != BaseClass;
        }

        public bool isQuestCompleted(int p)
        {
            return _quests.Where(qi => qi.id == p).Select(qi => qi.completed).FirstOrDefault();
        }

        public int getQuestCond(int questId)
        {
            return _quests.Where(qi => qi.id == questId).Select(qi => qi.stage).FirstOrDefault();
        }

        public override void OnPickUp(L2Item item)
        {
            //item.Location = L2Item.L2ItemLocation.inventory;
            //Inventory.addItem(item, true, true);
        }

        public void AddItem(int itemId, int count)
        {
            Inventory.AddItem(itemId, count, this);
        }

        public void DestroyItem(L2Item item, int count)
        {
            Inventory.DestroyItem(item, count, this);
        }

        public void DestroyItemById(int itemId, int count)
        {
            Inventory.DestroyItemById(itemId, count, this);
        }

        public bool ReduceAdena(int count)
        {
            return Inventory.ReduceAdena(count, this);
        }

        public L2Item GetItemByObjId(int objId)
        {
            return Inventory.GetItemByObjectId(objId);
        }

        public List<L2Item> GetAllItems()
        {
            return Inventory.Items;
        }

        public int GetAdena()
        {
            return Inventory.AdenaCount();
        }

        public void AddAdena(int count, bool sendMessage)
        {
            if(sendMessage)
                SendPacket(new SystemMessage(SystemMessage.SystemMessageId.EARNED_S1_ADENA).AddNumber(count));

            if (count > 0)
            {
                InventoryUpdate iu = new InventoryUpdate();
                iu.addNewItem(Inventory.AddItem(57, count, this));
                SendPacket(iu);
            }
        }

        public override string AsString()
        {
            return "L2Player:" + Name;
        }

        public override void OnRemObject(L2Object obj)
        {
            SendPacket(new DeleteObject(obj.ObjId));
        }

        public override void OnAddObject(L2Object obj, GameServerNetworkPacket pk, string msg = null)
        {
            if (obj is L2Npc)
            {
                SendPacket(new NpcInfo((L2Npc)obj));
            }
            else if (obj is L2Player)
            {
                SendPacket(new CharInfo((L2Player)obj));

                if (msg != null)
                    ((L2Player)obj).SendMessage(msg);
            }
            else if (obj is L2Item)
            {
                SendPacket(pk ?? new SpawnItem((L2Item)obj));
            }
            else if (obj is L2Summon)
            {
                SendPacket(pk ?? new PetInfo((L2Summon)obj));
            }
            else if (obj is L2Chair)
            {
                SendPacket(new StaticObject((L2Chair)obj));
            }
            else if (obj is L2StaticObject)
            {
                SendPacket(new StaticObject((L2StaticObject)obj));
            }
            else if (obj is L2Boat)
            {
                SendPacket(new VehicleInfo((L2Boat)obj));
            }
        }

        public void BroadcastCharInfo()
        {
            foreach (L2Player player in L2World.Instance.GetPlayers().Where(player => player != this))
                player.SendPacket(new CharInfo(this));
        }

        public override void BroadcastUserInfo()
        {
            SendPacket(new UserInfo(this));

            //if (getPolyType() == PolyType.NPC)
            //    Broadcast.toKnownPlayers(this, new AbstractNpcInfo.PcMorphInfo(this, getPolyTemplate()));
            //else
            BroadcastCharInfo();
        }

        public override void AddKnownObject(L2Object obj)
        {
            SendInfoFrom(obj);
        }

        private void SendInfoFrom(L2Object obj)
        {
            //if (obj.getPolyType() == PolyType.ITEM)
            //    sendPacket(new SpawnItem(obj));
            //else
            //{
            // send object info to player
            obj.SendInfo(this);

            //         if (obj is L2Character)
            //{
            //             // Update the state of the L2Character object client side by sending Server->Client packet MoveToPawn/MoveToLocation and AutoAttackStart to the L2PcInstance
            //             L2Character obj2 = (L2Character)obj;
            //             if (obj2. hasAI())
            //                 obj2.getAI().describeStateToPlayer(this);
            //         }
            //     }
        }

        public void untransform()
        {
            if (Transform != null)
            {
                Transform.Template.onTransformEnd(this);
                Transform = null;
            }
        }

        public bool HasItem(int itemId, int count)
        {
            foreach (L2Item item in Inventory.Items)
                if(item.Template.ItemID == itemId)
                    if (item.Count >= count)
                        return true;
            return false;
        }

        public override void SendInfo(L2Player player)
        {
            //if (this.Boa isInBoat())
            //    getPosition().set(getBoat().getPosition());

            //if (getPolyType() == PolyType.NPC)
            //    activeChar.sendPacket(new AbstractNpcInfo.PcMorphInfo(this, getPolyTemplate()));
            //else
            //{
            player.SendPacket(new CharInfo(this));

            if (IsSitting)
            {
                //               L2Object obj = World.getInstance().getObject(_throneId);
                //               if (obj is L2StaticObject)
                //activeChar.sendPacket(new ChairSit(getObjectId(), ((L2StaticObjectInstance)object).getStaticObjectId()));
            }
            //}

            //int relation = getRelation(activeChar);
            //boolean isAutoAttackable = isAutoAttackable(activeChar);

            //activeChar.sendPacket(new RelationChanged(this, relation, isAutoAttackable));
            //if (getPet() != null)
            //    activeChar.sendPacket(new RelationChanged(getPet(), relation, isAutoAttackable));

            //relation = activeChar.getRelation(this);
            //isAutoAttackable = activeChar.isAutoAttackable(this);

            //sendPacket(new RelationChanged(activeChar, relation, isAutoAttackable));
            //if (activeChar.getPet() != null)
            //    sendPacket(new RelationChanged(activeChar.getPet(), relation, isAutoAttackable));

            //if (isInBoat())
            //    activeChar.sendPacket(new GetOnVehicle(getObjectId(), getBoat().getObjectId(), getVehiclePosition()));

            //switch (getStoreType())
            //{
            //    case SELL:
            //    case PACKAGE_SELL:
            //        activeChar.sendPacket(new PrivateStoreMsgSell(this));
            //        break;

            //    case BUY:
            //        activeChar.sendPacket(new PrivateStoreMsgBuy(this));
            //        break;

            //    case MANUFACTURE:
            //        activeChar.sendPacket(new RecipeShopMsg(this));
            //        break;
            //}
        }

        public void setTransform(L2Transform tr)
        {
            Transform = tr;
            Transform.owner = this;
            Transform.Template.onTransformStart(this);
        }

        public SortedList<int, db_InstanceReuse> InstanceReuse = new SortedList<int, db_InstanceReuse>();
        public int ViewingAdminPage;
        public int ViewingAdminTeleportGroup = -1;
        public int TeleportPayID;
        public int LastMinigameScore;
        public short ClanType;
        public int Fame;

        public void ShowHtmAdmin(string val, bool plain)
        {
            if (plain)
                SendPacket(new TutorialShowHtml(this, val, true, true));
            else
                SendPacket(new TutorialShowHtml(this, val, true));

            ViewingAdminPage = 1;
        }

        public void ShowHtmBBS(string val)
        {
            ShowBoard.separateAndSend(val, this);
        }

        public void sendItemList(bool open = false)
        {
            SendPacket(new ItemList(this, open));
            SendPacket(new ExQuestItemList(this));
        }

        public void updateWeight()
        {
            long oldweight = CurrentWeight;
            long total = 0;
            //if (!_diet)
            //    foreach (L2Item it in Inventory.Items.Values.Where(it => it.Template.Weight != 0))
            //        if (it.Template.isStackable())
            //            total += it.Template.Weight * it.Count;
            //        else
            //            total += it.Template.Weight;

            CurrentWeight = total >= int.MaxValue ? int.MaxValue : (int)total;

            if (oldweight != total)
            {
                StatusUpdate su = new StatusUpdate(ObjId);
                su.add(StatusUpdate.CUR_LOAD, CurrentWeight);
                SendPacket(su);

                long weightproc = total * 1000 / (int)CharacterStat.getStat(TEffectType.b_max_weight);

                int newWeightPenalty;
                if (weightproc < 500)
                    newWeightPenalty = 0;
                else if (weightproc < 666)
                    newWeightPenalty = 1;
                else if (weightproc < 800)
                    newWeightPenalty = 2;
                else if (weightproc < 1000)
                    newWeightPenalty = 3;
                else
                    newWeightPenalty = 4;

                if (_penaltyWeight != newWeightPenalty)
                {
                    if (newWeightPenalty > 0)
                        AddSkill(4270, newWeightPenalty, false, true);
                    else
                        RemoveSkill(4270, false, true);

                    _penaltyWeight = newWeightPenalty;

                    SendPacket(new EtcStatusUpdate(this));
                }
            }
        }

        public bool CheckFreeWeight(int weight)
        {
            if (CurrentWeight + weight >= CharacterStat.getStat(TEffectType.b_max_weight))
                return false;

            return true;
        }

        public bool CheckFreeWeight80(int weight)
        {
            if (CurrentWeight + weight >= (CharacterStat.getStat(TEffectType.b_max_weight) * .8))
                return false;

            return true;
        }

        public void SpawnMe()
        {
            //_isVisible = true;

            Region = L2World.Instance.GetRegion(new Location(X, Y, Z));

            L2World.Instance.AddPlayer(this);

            OnSpawn();
        }


        public byte ClanRank()
        {
            if ((ClanId == 0) || (Clan == null))
                return 0;

            bool leader = Clan.LeaderID == ObjId;
            e_ClanRank rank = e_ClanRank.vagabond;
            switch (ClanType)
            {
                case (short)e_ClanType.CLAN_MAIN:
                {
                    switch (Clan.Level)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                            rank = e_ClanRank._1;
                            break;
                        case 4:
                            if (leader)
                                rank = e_ClanRank._3;
                            break;
                        case 5:
                            rank = leader ? e_ClanRank._4 : e_ClanRank._3;
                            break;
                        case 6:
                            if (leader)
                                rank = e_ClanRank._5;
                            else if (Clan.isSubLeader(ObjId, new[] { e_ClanType.CLAN_KNIGHT1, e_ClanType.CLAN_KNIGHT2 }) != e_ClanType.None)
                                rank = e_ClanRank._4;
                            else
                                rank = e_ClanRank._3;
                            break;
                        case 7:
                            if (leader)
                                rank = e_ClanRank._7;
                            else if (Clan.isSubLeader(ObjId, new[] { e_ClanType.CLAN_KNIGHT1, e_ClanType.CLAN_KNIGHT2 }) != e_ClanType.None)
                                rank = e_ClanRank._6;
                            else if (Clan.isSubLeader(ObjId, new[] { e_ClanType.CLAN_KNIGHT3, e_ClanType.CLAN_KNIGHT4, e_ClanType.CLAN_KNIGHT5, e_ClanType.CLAN_KNIGHT6 }) != e_ClanType.None)
                                rank = e_ClanRank._5;
                            else
                                rank = e_ClanRank._4;
                            break;
                        case 8:
                            if (leader)
                                rank = e_ClanRank._8;
                            else if (Clan.isSubLeader(ObjId, new[] { e_ClanType.CLAN_KNIGHT1, e_ClanType.CLAN_KNIGHT2 }) != e_ClanType.None)
                                rank = e_ClanRank._7;
                            else if (Clan.isSubLeader(ObjId, new[] { e_ClanType.CLAN_KNIGHT3, e_ClanType.CLAN_KNIGHT4, e_ClanType.CLAN_KNIGHT5, e_ClanType.CLAN_KNIGHT6 }) != e_ClanType.None)
                                rank = e_ClanRank._6;
                            else
                                rank = e_ClanRank._5;
                            break;
                        case 9:
                            if (leader)
                                rank = e_ClanRank._9;
                            else if (Clan.isSubLeader(ObjId, new[] { e_ClanType.CLAN_KNIGHT1, e_ClanType.CLAN_KNIGHT2 }) != e_ClanType.None)
                                rank = e_ClanRank._8;
                            else if (Clan.isSubLeader(ObjId, new[] { e_ClanType.CLAN_KNIGHT3, e_ClanType.CLAN_KNIGHT4, e_ClanType.CLAN_KNIGHT5, e_ClanType.CLAN_KNIGHT6 }) != e_ClanType.None)
                                rank = e_ClanRank._7;
                            else
                                rank = e_ClanRank._6;
                            break;
                        case 10:
                            if (leader)
                                rank = e_ClanRank._10;
                            else if (Clan.isSubLeader(ObjId, new[] { e_ClanType.CLAN_KNIGHT1, e_ClanType.CLAN_KNIGHT2 }) != e_ClanType.None)
                                rank = e_ClanRank._9;
                            else if (Clan.isSubLeader(ObjId, new[] { e_ClanType.CLAN_KNIGHT3, e_ClanType.CLAN_KNIGHT4, e_ClanType.CLAN_KNIGHT5, e_ClanType.CLAN_KNIGHT6 }) != e_ClanType.None)
                                rank = e_ClanRank._8;
                            else
                                rank = e_ClanRank._7;
                            break;
                        case 11:
                            if (leader)
                                rank = e_ClanRank._11;
                            else if (Clan.isSubLeader(ObjId, new[] { e_ClanType.CLAN_KNIGHT1, e_ClanType.CLAN_KNIGHT2 }) != e_ClanType.None)
                                rank = e_ClanRank._10;
                            else if (Clan.isSubLeader(ObjId, new[] { e_ClanType.CLAN_KNIGHT3, e_ClanType.CLAN_KNIGHT4, e_ClanType.CLAN_KNIGHT5, e_ClanType.CLAN_KNIGHT6 }) != e_ClanType.None)
                                rank = e_ClanRank._9;
                            else
                                rank = e_ClanRank._8;
                            break;
                    }
                }

                    break;
                case (short)e_ClanType.CLAN_ACADEMY:
                {
                    rank = e_ClanRank._1;
                }
                    break;
                case (short)e_ClanType.CLAN_KNIGHT1:
                case (short)e_ClanType.CLAN_KNIGHT2:
                {
                    switch (Clan.Level)
                    {
                        case 6:
                            rank = e_ClanRank._2;
                            break;
                        case 7:
                            rank = e_ClanRank._3;
                            break;
                        case 8:
                            rank = e_ClanRank._4;
                            break;
                        case 9:
                            rank = e_ClanRank._5;
                            break;
                        case 10:
                            rank = e_ClanRank._6;
                            break;
                        case 11:
                            rank = e_ClanRank._7;
                            break;
                    }
                }

                    break;
                case (short)e_ClanType.CLAN_KNIGHT3:
                case (short)e_ClanType.CLAN_KNIGHT4:
                case (short)e_ClanType.CLAN_KNIGHT5:
                case (short)e_ClanType.CLAN_KNIGHT6:
                {
                    switch (Clan.Level)
                    {
                        case 7:
                            rank = e_ClanRank._2;
                            break;
                        case 8:
                            rank = e_ClanRank._3;
                            break;
                        case 9:
                            rank = e_ClanRank._4;
                            break;
                        case 10:
                            rank = e_ClanRank._5;
                            break;
                        case 11:
                            rank = e_ClanRank._6;
                            break;
                    }
                }

                    break;
            }

            if ((Noblesse == 1) && ((byte)rank < 5))
                rank = e_ClanRank._5;

            if ((Heroic == 1) && ((byte)rank < 8))
                rank = e_ClanRank._8;

            return (byte)rank;
        }

        public override void UpdateAbnormalEventEffect()
        {
            BroadcastPacket(new ExBrExtraUserInfo(ObjId, AbnormalBitMaskEvent));
        }

        public override void UpdateAbnormalExEffect()
        {
            BroadcastUserInfo();
        }

        public string Penalty_ClanCreate = "0";
        public string Penalty_ClanJoin = "0";

        public void setPenalty_ClanCreate(DateTime time, bool sql)
        {
            if (DateTime.Now < time)
                Penalty_ClanCreate = time.ToString("yyyy-MM-dd HH-mm-ss");
            else
                Penalty_ClanCreate = "0";

            if (sql) { }
        }

        public void setPenalty_ClanJoin(DateTime time, bool sql)
        {
            if (DateTime.Now < time)
                Penalty_ClanJoin = time.ToString("yyyy-MM-dd HH-mm-ss");
            else
                Penalty_ClanJoin = "0";

            if (sql) { }
        }

        public bool IsRestored;

        public void TotalRestore()
        {
            if (IsRestored)
                return;

            onGameInit();
            db_restoreSkills();
            //db_restoreQuests();
            db_restoreRecipes();
            // db_restoreShortcuts(); elfo to be added

            IsRestored = true;
        }

        public void db_restoreShortcuts()
        {
            //MySqlConnection connection = SQLjec.getInstance().conn();
            //MySqlCommand cmd = connection.CreateCommand();

            //connection.Open();

            //cmd.CommandText = "SELECT * FROM user_shortcuts WHERE ownerId=" + ObjID + " and classId=" + ActiveClass.id;
            //cmd.CommandType = CommandType.Text;

            //MySqlDataReader reader = cmd.ExecuteReader();

            //while (reader.Read())
            //{
            //    L2Shortcut sc = new L2Shortcut();
            //    sc._slot = reader.GetInt32("slot");
            //    sc._page = reader.GetInt32("page");
            //    sc._type = reader.GetInt32("type");
            //    sc._id = reader.GetInt32("id");
            //    sc._level = reader.GetInt32("lvl");
            //    sc._characterType = reader.GetInt32("cha");

            //    _shortcuts.Add(sc);
            //}

            //reader.Close();
            //connection.Close();
        }

        public void ReduceSouls(byte count)
        {
            Souls -= count;
            SendPacket(new EtcStatusUpdate(this));
        }

        public void AddSouls(byte count)
        {
            Souls += count;
            SendPacket(new EtcStatusUpdate(this));

            SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.YOUR_SOUL_COUNT_HAS_INCREASED_BY_S1_NOW_AT_S2);
            sm.AddNumber(count);
            sm.AddNumber(Souls);
            SendPacket(sm);
        }

        public void IncreaseSouls()
        {
            if ((Souls + 1 > 45) || (Souls == 45))
            {
                SendSystemMessage(SystemMessage.SystemMessageId.SOUL_CANNOT_BE_INCREASED_ANYMORE);
                return;
            }

            AddSouls(1);
        }

        public bool IsCursed = false;

        public byte PartyState;
        public L2Player requester;
        public int itemDistribution;

        public void PendToJoinParty(L2Player asker, int askerItemDistribution)
        {
            PartyState = 1;
            requester = asker;
            requester.itemDistribution = askerItemDistribution;
            SendPacket(new AskJoinParty(asker.Name, askerItemDistribution));
        }

        public void ClearPend()
        {
            PartyState = 0;
            requester = null;
        }

        public L2Summon Summon;
        public bool IsInOlympiad = false;
        public L2Item EnchantScroll,
                      EnchantItem,
                      EnchantStone;
        public byte EnchantState = 0;

        // 0 cls, 1 violet, 2 blink
        public byte PvPStatus;

        public byte GetEnchantValue()
        {
            int val = 0;//Inventory.getWeaponEnchanment();

            if (MountType > 0)
                return 0;

            if (val > 127)
                val = 127;

            return (byte)val;
        }

        public override void OnNewTargetSelection(L2Object target)
        {
            int color = 0;

            if (target is L2Summon)
                if (!((((L2Summon)target).Owner != null) && (((L2Summon)target).Owner.ObjId == ObjId)))
                    color = ((L2Summon)target).Level - Level;

            SendPacket(new MyTargetSelected(target.ObjId, color));
        }

        public override void OnOldTargetSelection(L2Object target)
        {
            double dis = Calcs.calculateDistance(this, target, true);
            if (dis < 151)
                target.NotifyAction(this);
            else
                TryMoveTo(target.X, target.Y, target.Z);

            SendActionFailed();
        }

        private Timer petSummonTime,
                      nonpetSummonTime;
        private int PetID = -1;
        private L2Item PetControlItem;

        public void PetSummon(L2Item item, int NpcID, bool isPet = true)
        {
            if (Summon != null)
            {
                SendSystemMessage(SystemMessage.SystemMessageId.YOU_ALREADY_HAVE_A_PET);
                return;
            }

            if (isPet)
            {
                if (petSummonTime == null)
                {
                    petSummonTime = new Timer();
                    petSummonTime.Interval = 5000;
                    petSummonTime.Elapsed += new ElapsedEventHandler(PetSummonEnd);
                }

                petSummonTime.Enabled = true;
                SendSystemMessage(SystemMessage.SystemMessageId.SUMMON_A_PET);
            }
            else
            {
                if (nonpetSummonTime == null)
                {
                    nonpetSummonTime = new Timer();
                    nonpetSummonTime.Interval = 5000;
                    nonpetSummonTime.Elapsed += new ElapsedEventHandler(NonpetSummonEnd);
                }

                nonpetSummonTime.Enabled = true;
            }

            PetID = NpcID;
            PetControlItem = item;

            BroadcastPacket(new MagicSkillUse(this, this, 1111, 1, 5000));
            SendPacket(new SetupGauge(ObjId, SetupGauge.SG_color.blue, 4900));
        }

        private void PetSummonEnd(object sender, ElapsedEventArgs e)
        {
            L2Pet pet = new L2Pet();
            //pet.setTemplate(NpcTable.Instance.GetNpcTemplate(PetID));
            pet.setOwner(this);
            pet.ControlItem = PetControlItem;
            // pet.sql_restore();
            pet.SpawmMe();

            petSummonTime.Enabled = false;
        }

        private void NonpetSummonEnd(object sender, ElapsedEventArgs e)
        {
            L2Summon summon = new L2Summon();
            //summon.setTemplate(NpcTable.Instance.GetNpcTemplate(PetID));
            summon.setOwner(this);
            summon.ControlItem = PetControlItem;
            summon.SpawmMe();

            nonpetSummonTime.Enabled = false;
        }

        public override bool CantMove()
        {
            if ((petSummonTime != null) && petSummonTime.Enabled)
                return true;
            if ((nonpetSummonTime != null) && nonpetSummonTime.Enabled)
                return true;

            return base.CantMove();
        }

        public override L2Character[] GetPartyCharacters()
        {
            List<L2Character> chars = new List<L2Character>();
            chars.Add(this);
            if (Summon != null)
                chars.Add(Summon);

            if (Party != null)
                foreach (L2Player pl in Party.Members.Where(pl => pl.ObjId != ObjId))
                {
                    chars.Add(pl);

                    if (pl.Summon != null)
                        chars.Add(pl.Summon);
                }

            return chars.ToArray();
        }

        public override void AbortCast()
        {
            if ((petSummonTime != null) && petSummonTime.Enabled)
                petSummonTime.Enabled = false;

            if ((nonpetSummonTime != null) && nonpetSummonTime.Enabled)
                nonpetSummonTime.Enabled = false;

            base.AbortCast();
        }

        public override void StartAi()
        {
            AiCharacter = new PlayerAI(this);
        }

        public List<TSpecEffect> specEffects = new List<TSpecEffect>();

        public bool isSittingInProgress()
        {
            if (sitTime != null)
                return sitTime.Enabled;

            return false;
        }

        public bool isSitting()
        {
            return IsSitting;
        }

        private Timer sitTime;
        private bool IsSitting;

        public void Sit()
        {
            if (sitTime == null)
            {
                sitTime = new Timer();
                sitTime.Interval = 2500;
                sitTime.Elapsed += new ElapsedEventHandler(SitEnd);
            }

            sitTime.Enabled = true;
            BroadcastPacket(new ChangeWaitType(this, ChangeWaitType.SIT));
        }

        public void Stand()
        {
            sitTime.Enabled = true;
            BroadcastPacket(new ChangeWaitType(this, ChangeWaitType.STAND));
            //TODO stop relax effect
        }

        private void SitEnd(object sender, ElapsedEventArgs e)
        {
            sitTime.Enabled = false;
            IsSitting = !IsSitting;

            if (!IsSitting && (chair != null))
            {
                chair.IsUsedAlready = false;
                chair = null;
            }
        }

        private L2Chair chair;
        public L2Boat Boat;
        public int BoatX;
        public int BoatY;
        public int BoatZ;

        public void SetChair(L2Chair chairObj)
        {
            chair = chairObj;
            chair.IsUsedAlready = true;
            BroadcastPacket(new ChairSit(ObjId, chairObj.StaticID));
        }

        public bool isOnShip()
        {
            return false;
        }

        public bool IsWard()
        {
            return false;
        }

        // arrow, bolt
        public L2Item SecondaryWeaponSupport;

        public override L2Item ActiveWeapon => null;

        public override void AbortAttack()
        {
            base.AbortAttack();
            AiCharacter.StopAutoAttack();
        }

        public override void DoAttack(L2Character target)
        {
            if (target == null)
            {
                SendMessage("null");
                AiCharacter.StopAutoAttack();
                SendActionFailed();
                return;
            }

            if (target.Dead)
            {
                SendMessage("dead");
                AiCharacter.StopAutoAttack();
                SendActionFailed();
                return;
            }

            if ((AttackToHit != null) && AttackToHit.Enabled)
            {
                SendActionFailed();
                return;
            }

            if ((AttackToEnd != null) && AttackToEnd.Enabled)
            {
                SendActionFailed();
                return;
            }

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

                    if ((SecondaryWeaponSupport == null) || (SecondaryWeaponSupport.Count < weapon.Template.SoulshotCount))
                    {
                        if (weapon.Template.WeaponType == ItemTemplate.L2ItemWeaponType.bow)
                            SendSystemMessage(SystemMessage.SystemMessageId.NOT_ENOUGH_ARROWS);
                        else
                            SendSystemMessage(SystemMessage.SystemMessageId.NOT_ENOUGH_BOLTS);
                        SendActionFailed();
                        return;
                    }
                }
            }
            else
            {
                timeAtk = (1362 * 345 / timeAtk);
                dual = true;
            }

            if (!Calcs.checkIfInRange((int)dist, this, target, true))
            {
                SendMessage("too far " + dist);
                TryMoveTo(target.X, target.Y, target.Z);
                return;
            }

            if ((reqMp > 0) && (reqMp > CurMp))
            {
                SendMessage("no mp " + CurMp + " " + reqMp);
                SendActionFailed();
                return;
            }

            if (ranged)
            {
                SendPacket(new SetupGauge(ObjId, SetupGauge.SG_color.red, (int)timeAtk));
                //Inventory.destroyItem(SecondaryWeaponSupport, 1, false, true);
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
                AttackToHit.Elapsed += new ElapsedEventHandler(AttackDoHit);
            }

            double timeToHit = ranged ? timeAtk * 0.5 : timeAtk * 0.6;
            AttackToHit.Interval = timeToHit;
            AttackToHit.Enabled = true;

            if (dual)
            {
                if (AttackToHitBonus == null)
                {
                    AttackToHitBonus = new Timer();
                    AttackToHitBonus.Elapsed += new ElapsedEventHandler(AttackDoHit2Nd);
                }

                AttackToHitBonus.Interval = timeAtk * 0.78;
                AttackToHitBonus.Enabled = true;
            }

            if (AttackToEnd == null)
            {
                AttackToEnd = new Timer();
                AttackToEnd.Elapsed += new ElapsedEventHandler(AttackDoEnd);
            }

            AttackToEnd.Interval = timeAtk;
            AttackToEnd.Enabled = true;

            BroadcastPacket(atk);
        }

        public override void AttackDoHit(object sender, ElapsedEventArgs e)
        {
            if ((CurrentTarget != null) && !CurrentTarget.Dead)
                if (!Hit1.Miss)
                {
                    if (Hit1.Crit)
                        SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1_LANDED_A_CRITICAL_HIT).AddPlayerName(Name));

                    SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1_HAS_GIVEN_C2_DAMAGE_OF_S3).AddPlayerName(Name).AddName(CurrentTarget).AddNumber(Hit1.Damage));
                    CurrentTarget.ReduceHp(this, Hit1.Damage);

                    if (CurrentTarget is L2Player)
                        CurrentTarget.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1_HAS_RECEIVED_S3_DAMAGE_FROM_C2).AddName(CurrentTarget).AddName(this).AddNumber(Hit1.Damage));
                }
                else
                {
                    SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1_ATTACK_WENT_ASTRAY).AddPlayerName(Name));

                    if (CurrentTarget is L2Player)
                    {
                        CurrentTarget.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1_HAS_EVADED_C2_ATTACK).AddName(CurrentTarget).AddName(this));
                        ((L2Player)CurrentTarget).AiCharacter.NotifyEvaded(this);
                    }
                }

            AttackToHit.Enabled = false;
        }

        public override void AttackDoHit2Nd(object sender, ElapsedEventArgs e)
        {
            if ((CurrentTarget != null) && !CurrentTarget.Dead)
                if (!Hit2.Miss)
                {
                    if (Hit2.Crit)
                        SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1_LANDED_A_CRITICAL_HIT).AddName(this));

                    SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1_HAS_GIVEN_C2_DAMAGE_OF_S3).AddName(this).AddName(CurrentTarget).AddNumber(Hit2.Damage));
                    CurrentTarget.ReduceHp(this, Hit2.Damage);

                    if (CurrentTarget is L2Player)
                        CurrentTarget.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1_HAS_RECEIVED_S3_DAMAGE_FROM_C2).AddName(CurrentTarget).AddName(this).AddNumber(Hit2.Damage));
                }
                else
                {
                    SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1_ATTACK_WENT_ASTRAY).AddPlayerName(Name));

                    if (CurrentTarget is L2Player)
                    {
                        CurrentTarget.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1_HAS_EVADED_C2_ATTACK).AddName(CurrentTarget).AddName(this));
                        ((L2Player)CurrentTarget).AiCharacter.NotifyEvaded(this);
                    }
                }

            AttackToHitBonus.Enabled = false;
        }

        public override void AttackDoEnd(object sender, ElapsedEventArgs e)
        {
            AttackToEnd.Enabled = false;

            //L2Item weapon = Inventory.getWeapon();
            //if (weapon != null)
            //{
            //    if (weapon.Soulshot)
            //        weapon.Soulshot = false;

            //    foreach (int sid in weapon.Template.getSoulshots().Where(sid => autoSoulshots.Contains(sid)))
            //    {
            //        if (Inventory.getItemCount(sid) < weapon.Template.SoulshotCount)
            //        {
            //            sendPacket(new SystemMessage(SystemMessage.SystemMessageId.AUTO_USE_CANCELLED_LACK_OF_S1).AddItemName(sid));

            //            lock (autoSoulshots)
            //            {
            //                autoSoulshots.Remove(sid);
            //                sendPacket(new ExAutoSoulShot(sid, 0));
            //            }
            //        }
            //        else
            //        {
            //            Inventory.destroyItem(sid, weapon.Template.SoulshotCount, false, true);
            //            weapon.Soulshot = true;
            //            broadcastSoulshotUse(sid);
            //        }

            //        break;
            //    }
            //}
        }

        public override double Radius
        {
            get
            {
                if (MountType > 0)
                    return MountedTemplate.CollisionRadius;

                return Sex == 0 ? BaseClass.CollisionRadius : BaseClass.CollisionRadiusFemale;
            }
        }

        public override double Height
        {
            get
            {
                if (TransformID > 0)
                    return Transform.Template.getHeight(Sex);

                if (MountType > 0)
                    return MountedTemplate.CollisionHeight;

                return Sex == 0 ? BaseClass.CollisionHeight : BaseClass.CollisionHeightFemale;
            }
        }

        public List<int> autoSoulshots = new List<int>();
        public List<int> setKeyItems;
        public int setKeyId;

        public int MountType;
        public NpcTemplate MountedTemplate;
        public int TradeState;

        public void Mount(NpcTemplate npc)
        {
            BroadcastPacket(new Ride(this, true, npc.NpcId));
            MountedTemplate = npc;
            BroadcastUserInfo();
        }

        public void MountPet()
        {
            if (Summon != null)
                Mount(Summon.Template);
        }

        public void unMount()
        {
            BroadcastPacket(new Ride(this, false));
            MountedTemplate = null;
            BroadcastUserInfo();
        }

        public SortedList<int, long> currentTrade;
        public int sstt;

        public long AddItemToTrade(int objId, long num)
        {
            if (currentTrade == null)
                currentTrade = new SortedList<int, long>();

            if (currentTrade.ContainsKey(objId))
            {
                currentTrade[objId] += num;
                return currentTrade[objId];
            }

            currentTrade.Add(objId, num);
            return num;
        }

        public void NotifyDayChange(GameServerNetworkPacket pk)
        {
            SendPacket(pk);
            if (pk is SunSet) //включаем ночные скилы
                AiCharacter.NotifyOnStartNight();
            else
                AiCharacter.NotifyOnStartDay();
        }

        public int VehicleId
        {
            get { return Boat != null ? Boat.ObjId : 0; }
        }

        public void Revive(double percent)
        {
            BroadcastPacket(new Revive(ObjId));
            Dead = false;
            StartRegeneration();
        }

        private DateTime pingTimeout;
        private int lastPingId;
        public int Ping = -1;
        public MultiSellList CustomMultiSellList;
        public int LastRequestedMultiSellId = -1;
        public int AttackingId;
        public SortedList<int, TAcquireSkill> ActiveSkillTree;

        public void RequestPing()
        {
            lastPingId = new Random().Next(int.MaxValue);
            NetPing ping = new NetPing(lastPingId);
            pingTimeout = DateTime.Now;
            SendPacket(ping);
        }

        public void UpdatePing(int id, int ms, int unk)
        {
            if (lastPingId != id)
            {
                log.Warn($"player fail to ping respond right {id} {lastPingId} at {pingTimeout.ToLocalTime()}");
                return;
            }

            Ping = ms;
            SendMessage("Your connection latency is " + ms);
        }

        public void InstantTeleportWithItem(int x, int y, int z, int id, long cnt)
        {
            //Inventory.destroyItem(id, cnt, true, true);
        }

        public void RedistExp(L2Warrior mob)
        {
            double expPet = 0.0;
            if ((Summon != null) && (Summon.ConsumeExp > 0))
                expPet = Summon.ConsumeExp / 100.0 + 1;

            double expReward = mob.Template.Exp / (1.0);
            int sp = mob.Template.Sp;
            SendMessage("debug: expPet " + expPet);
            SendMessage("debug: mob.Template " + mob.Template.Exp + " @");
            SendMessage("debug: expReward " + expReward);
            SendMessage("debug: sp " + sp);

            byte oldLvl = Level;
            Exp += (long)expReward;
            byte newLvl = Experience.getLevel(Exp);
            bool lvlChanged = oldLvl != newLvl;

            Exp += (long)expReward;
            if (lvlChanged)
            {
                Level = newLvl;
                BroadcastPacket(new SocialAction(ObjId, 2122));
            }

            if (!lvlChanged)
                SendPacket(new UserInfo(this));
            else
                BroadcastUserInfo();
        }

        public byte ClanLevel
        {
            get { return Clan == null ? (byte)0 : Clan.Level; }
        }

        public void broadcastSkillUse(int skillId)
        {
            TSkill skill = TSkillTable.Instance.Get(skillId);
            BroadcastPacket(new MagicSkillUse(this, this, skill.skill_id, skill.level, skill.skill_hit_time));
        }

        public bool ClanLeader
        {
            get
            {
                if (Clan == null)
                    return false;

                return Clan.LeaderID == ObjId;
            }
        }

        public bool HavePledgePower(int bit)
        {
            return (Clan != null) && Clan.hasRights(this, bit);
        }

        public override L2Item GetWeaponItem()
        {
            return null;
        }

        public void UpdateAgathionEnergy(int count)
        {
            SendMessage("@UpdateAgathionEnergy " + count);
        }

        public List<Cubic> cubics = new List<Cubic>();

        public void StopCubic(Cubic cubic)
        {
            foreach (Cubic cub in cubics.Where(cub => cub.template.id == cubic.template.id))
            {
                lock (cubics)
                {
                    cubics.Remove(cub);
                }

                BroadcastUserInfo();
                break;
            }
        }

        public void AddCubic(Cubic cubic, bool update)
        {
            int max = (int)CharacterStat.getStat(TEffectType.p_cubic_mastery);
            if (max == 0)
                max = 1;

            if (cubics.Count == max)
            {
                Cubic cub = cubics[0];
                cub.OnEnd(false);
                lock (cubics)
                {
                    cubics.RemoveAt(0);
                }
            }

            foreach (Cubic cub in cubics.Where(cub => cub.template.id == cubic.template.id))
            {
                lock (cubics)
                {
                    cub.OnEnd(false);
                    cubics.Remove(cub);
                }
                break;
            }

            cubic.OnSummon();
            cubics.Add(cubic);
            if (update)
                BroadcastUserInfo();
        }

        public override void DoDie(L2Character killer, bool bytrigger)
        {
            if (cubics.Count > 0)
            {
                foreach (Cubic cub in cubics)
                    cub.OnEnd(false);

                cubics.Clear();
            }

            base.DoDie(killer, bytrigger);
        }
    }
}