using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Timers;
using log4net;
using L2dotNET.Enums;
using L2dotNET.GameService.Model.Communities;
using L2dotNET.GameService.Model.Inventory;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Npcs;
using L2dotNET.GameService.Model.Npcs.Cubic;
using L2dotNET.GameService.Model.Npcs.Decor;
using L2dotNET.GameService.Model.Playable;
using L2dotNET.GameService.Model.Player.AI;
using L2dotNET.GameService.Model.Player.General;
using L2dotNET.GameService.Model.Player.Transformation;
using L2dotNET.GameService.Model.Quests;
using L2dotNET.GameService.Model.Skills;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.GameService.Model.Skills2.Effects;
using L2dotNET.GameService.Model.Vehicles;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tables;
using L2dotNET.GameService.Tables.Multisell;
using L2dotNET.GameService.Templates;
using L2dotNET.GameService.Tools;
using L2dotNET.GameService.World;
using L2dotNET.Models;
using L2dotNET.Network;
using L2dotNET.Services.Contracts;
using L2dotNET.Utility;
using Ninject;

namespace L2dotNET.GameService.Model.Player
{
    [Synchronization]
    public class L2Player : L2Character
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(L2Player));

        [Inject]
        public IPlayerService PlayerService => GameServer.Kernel.Get<IPlayerService>();

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
        public int Sp { get; set; }
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
        public int IsIn7SDungeon { get; set; }
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
        public dynamic SessionData { get; set; }

        public L2Player RestorePlayer(int id, GameClient client)
        {
            PlayerModel playerModel = PlayerService.GetAccountByLogin(id);
            L2Player player = new L2Player
            {
                ObjId = id,
                Gameclient = client,
                Name = playerModel.Name,
                Title = playerModel.Title,
                Level = (byte)playerModel.Level,
                CurHp = playerModel.CurHp,
                CurMp = playerModel.CurMp,
                CurCp = playerModel.CurCp,
                Face = playerModel.Face,
                HairStyle = playerModel.HairStyle,
                HairColor = playerModel.HairColor,
                Sex = (byte)playerModel.Sex,
                X = playerModel.X,
                Y = playerModel.Y,
                Z = playerModel.Z,
                Heading = playerModel.Heading,
                Exp = playerModel.Exp,
                ExpOnDeath = playerModel.ExpBeforeDeath,
                Sp = playerModel.Sp,
                Karma = playerModel.Karma,
                PvpKills = playerModel.PvpKills,
                PkKills = playerModel.PkKills,
                BaseClass = CharTemplateTable.Instance.GetTemplate(playerModel.BaseClass),
                ActiveClass = CharTemplateTable.Instance.GetTemplate(playerModel.ClassId),
                RecLeft = playerModel.RecLeft,
                RecHave = playerModel.RecHave,
                CharSlot = playerModel.CharSlot,
                DeathPenaltyLevel = playerModel.DeathPenaltyLevel,
                ClanId = playerModel.ClanId,
                ClanPrivs = playerModel.ClanPrivs,
                PenaltyClanCreate = playerModel.ClanCreateExpiryTime.ToString(),
                PenaltyClanJoin = playerModel.ClanJoinExpiryTime.ToString(),
                Inventory = new PcInventory(this),
                DeleteTime = playerModel.DeleteTime
            };

            player.CStatsInit();
            SessionData = new PlayerBag();

            return player;
        }

        public static L2Player Create()
        {
            L2Player player = new L2Player
            {
                ObjId = IdFactory.Instance.NextId()
            };

            //player.Inventory = new InvPC();
            //player.Inventory._owner = player;

            return player;
        }

        public void UpdatePlayer()
        {
            PlayerModel playerModel = new PlayerModel
            {
                ObjectId = ObjId,
                Level = Level,
                MaxHp = MaxHp,
                CurHp = (int)CurHp,
                MaxCp = MaxCp,
                CurCp = (int)CurCp,
                MaxMp = MaxMp,
                CurMp = (int)CurMp,
                Face = Face,
                HairStyle = HairStyle,
                HairColor = HairColor,
                Sex = Sex,
                Heading = Heading,
                X = X,
                Y = Y,
                Z = Z,
                Exp = Exp,
                ExpBeforeDeath = ExpOnDeath,
                Sp = Sp,
                Karma = Karma,
                PvpKills = PvpKills,
                PkKills = PkKills,
                ClanId = ClanId,
                Race = (int)BaseClass.ClassId.ClassRace,
                ClassId = (int)ActiveClass.ClassId.Id,
                BaseClass = (int)BaseClass.ClassId.Id,
                DeleteTime = DeleteTime,
                CanCraft = CanCraft,
                Title = Title,
                RecHave = RecHave,
                RecLeft = RecLeft,
                AccessLevel = AccessLevel,
                ClanPrivs = ClanPrivs,
                WantsPeace = WantsPeace,
                IsIn7SDungeon = IsIn7SDungeon,
                PunishLevel = PunishLevel,
                PunishTimer = PunishTimer,
                PowerGrade = PowerGrade,
                Nobless = Nobless,
                Sponsor = Sponsor,
                VarkaKetraAlly = VarkaKetraAlly,
                ClanCreateExpiryTime = ClanCreateExpiryTime,
                ClanJoinExpiryTime = ClanJoinExpiryTime,
                DeathPenaltyLevel = DeathPenaltyLevel
            };

            PlayerService.UpdatePlayer(playerModel);
        }

        public int Int => ActiveClass.BaseInt;

        public int Str => ActiveClass.BaseStr;

        public int Con => ActiveClass.BaseCon;

        public int Men => ActiveClass.BaseMen;

        public int Dex => ActiveClass.BaseDex;

        public int Wit => ActiveClass.BaseWit;

        public byte Builder = 1;
        public byte Noblesse = 0;
        public byte Heroic = 0;

        public byte PrivateStoreType = 0;

        public byte GetPrivateStoreType()
        {
            return PrivateStoreType;
        }

        public override int AllianceCrestId => Clan?.AllianceCrestId ?? 0;

        public override int AllianceId => Clan?.AllianceId ?? 0;

        public override int ClanId
        {
            get { return Clan?.ClanId ?? 0; }
            set { Clan = ClanTable.Instance.GetClan(value); }
        }

        public override int ClanCrestId => Clan?.CrestId ?? 0;

        public int GetTitleColor()
        {
            return 0xFFFF77;
        }

        public int GetNameColor()
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

        internal bool IsFishing()
        {
            return false;
        }

        public override void SendPacket(GameserverPacket pk)
        {
            Gameclient.SendPacket(pk);
        }

        private ActionFailed _af;

        public override void SendActionFailed()
        {
            if (_af == null)
                _af = new ActionFailed();

            SendPacket(_af);
        }

        public override void SendSystemMessage(SystemMessage.SystemMessageId msgId)
        {
            SendPacket(new SystemMessage(msgId));
        }

        public int PenaltyWeight;
        public int PenaltyGrade = 0;

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

        public int CurrentFocusEnergy = 0;

        public int GetForceIncreased()
        {
            return CurrentFocusEnergy;
        }

        public Skill GetSkill(int magicId)
        {
            return Skills.ContainsKey(magicId) ? Skills[magicId] : null;
        }

        public override bool IsCastingNow()
        {
            if (_petSummonTime != null)
                return _petSummonTime.Enabled;

            if (_nonpetSummonTime != null)
                return _nonpetSummonTime.Enabled;

            return base.IsCastingNow();
        }

        public void UpdateReuse()
        {
            SendPacket(new SkillCoolTime(this));
        }

        public void CastSkill(Skill skill, bool ctrlPressed, bool shiftPressed)
        {
            if (IsCastingNow())
            {
                SendActionFailed();
                return;
            }

            if (IsSittingInProgress() || IsSitting())
            {
                SendActionFailed();
                return;
            }

            if (!skill.ConditionOk(this))
            {
                SendActionFailed();
                return;
            }

            L2Character target = skill.GetTargetCastId(this);

            if (target == null)
            {
                SendSystemMessage(SystemMessage.SystemMessageId.TargetCantFound);
                SendActionFailed();
                return;
            }

            if (skill.CastRange != -1)
            {
                double dis = Calcs.CalculateDistance(this, target, true);
                if (dis > skill.CastRange)
                {
                    TryMoveTo(target.X, target.Y, target.Z);
                    SendActionFailed();
                    return;
                }
            }

            if (skill.ReuseDelay > 0)
            {
                if (Reuse.ContainsKey(skill.SkillId))
                {
                    TimeSpan ts = Reuse[skill.SkillId].StopTime - DateTime.Now;

                    if (ts.TotalMilliseconds > 0)
                    {
                        if (ts.TotalHours > 0)
                        {
                            SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.S2HoursS3MinutesS4SecondsRemainingInS1ReuseTime);
                            sm.AddSkillName(skill.SkillId, skill.Level);
                            sm.AddNumber(ts.Hours);
                            sm.AddNumber(ts.Minutes);
                            sm.AddNumber(ts.Seconds);
                            SendPacket(sm);
                        }
                        else
                        {
                            if (ts.TotalMinutes > 0)
                            {
                                SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.S2MinutesS3SecondsRemainingInS1ReuseTime);
                                sm.AddSkillName(skill.SkillId, skill.Level);
                                sm.AddNumber(ts.Minutes);
                                sm.AddNumber(ts.Seconds);
                                SendPacket(sm);
                            }
                            else
                            {
                                SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.S2SecondsRemainingInS1ReuseTime);
                                sm.AddSkillName(skill.SkillId, skill.Level);
                                sm.AddNumber(ts.Seconds);
                                SendPacket(sm);
                            }
                        }

                        SendActionFailed();
                        return;
                    }
                }
            }

            if ((skill.MpConsume1 > 0) || (skill.MpConsume2 > 0))
            {
                if (CurMp < (skill.MpConsume1 + skill.MpConsume2))
                {
                    SendSystemMessage(SystemMessage.SystemMessageId.NotEnoughMp);
                    SendActionFailed();
                    return;
                }
            }

            if (skill.HpConsume > 0)
            {
                if (CurHp < skill.HpConsume)
                {
                    SendSystemMessage(SystemMessage.SystemMessageId.NotEnoughHp);
                    SendActionFailed();
                    return;
                }
            }

            //if (skill.ConsumeItemId != 0)
            //{
            //    int count = Inventory.getItemCount(skill.ConsumeItemId);
            //    if (count < skill.ConsumeItemCount)
            //    {
            //        sendPacket(new SystemMessage(SystemMessage.SystemMessageId.S1_CANNOT_BE_USED).AddSkillName(skill.skill_id, skill.level));
            //        sendActionFailed();
            //        return;
            //    }
            //}

            byte blowOk = 0;
            if (skill.Effects.Count > 0)
            {
                bool fail = false;
                foreach (Effect ef in skill.Effects)
                {
                    if (!ef.CanUse(this))
                    {
                        SendActionFailed();
                        fail = true;
                        break;
                    }

                    if (ef is FatalBlow && (blowOk == 0))
                        blowOk = ((FatalBlow)ef).Success(target);
                }

                if (fail)
                    return;
            }

            if (skill.ReuseDelay > 0)
            {
                L2SkillCoolTime reuse = new L2SkillCoolTime
                {
                    Id = skill.SkillId,
                    Lvl = skill.Level,
                    Total = (int)skill.ReuseDelay,
                    Owner = this
                };
                reuse.Delay = reuse.Total;
                reuse.Timer();
                Reuse.Add(reuse.Id, reuse);
                UpdateReuse();
            }

            //SendPacket(new SystemMessage(SystemMessage.SystemMessageId.UseS1).AddSkillName(skill.skill_id, skill.level));

            if (skill.HpConsume > 0)
            {
                CurHp -= skill.HpConsume;

                StatusUpdate su = new StatusUpdate(ObjId);
                su.Add(StatusUpdate.CurHp, (int)CurHp);
                BroadcastPacket(su);
            }

            if (skill.MpConsume1 > 0)
            {
                CurMp -= skill.MpConsume1;

                StatusUpdate su = new StatusUpdate(ObjId);
                su.Add(StatusUpdate.CurMp, (int)CurMp);
                BroadcastPacket(su);
            }

            //if (skill.ConsumeItemId != 0)
            //   Inventory.destroyItem(skill.ConsumeItemId, skill.ConsumeItemCount, true, true);

            int hitTime = skill.SkillHitTime;

            int hitT = hitTime > 0 ? (int)(hitTime * 0.95) : 0;
            CurrentCast = skill;

            if (hitTime > 0)
                SendPacket(new SetupGauge(ObjId, SetupGauge.SgColor.Blue, hitTime - 20));

            BroadcastPacket(new MagicSkillUse(this, target, skill, hitTime == 0 ? 20 : hitTime, blowOk));
            if (hitTime > 50)
            {
                if (CastTime == null)
                {
                    CastTime = new Timer();
                    CastTime.Elapsed += castEnd;
                }

                CastTime.Interval = hitT;
                CastTime.Enabled = true;
            }
            else
                castEnd();
        }

        private void castEnd(object sender = null, ElapsedEventArgs e = null)
        {
            if (CurrentCast.MpConsume2 > 0)
            {
                if (CurMp < CurrentCast.MpConsume2)
                {
                    SendSystemMessage(SystemMessage.SystemMessageId.NotEnoughMp);
                    SendActionFailed();

                    CurrentCast = null;
                    CastTime.Enabled = false;
                    return;
                }

                CurMp -= CurrentCast.MpConsume2;

                StatusUpdate su = new StatusUpdate(ObjId);
                su.Add(StatusUpdate.CurMp, (int)CurMp);
                BroadcastPacket(su);
            }

            if (CurrentCast.CastRange != -1)
            {
                bool block = false;
                if (CurrentTarget != null)
                {
                    double dis = Calcs.CalculateDistance(this, CurrentTarget, true);
                    if (dis > CurrentCast.EffectiveRange)
                        block = true;
                }
                else
                    block = true;

                if (block)
                {
                    SendSystemMessage(SystemMessage.SystemMessageId.DistTooFarCastingStopped);
                    SendActionFailed();

                    CurrentCast = null;
                    CastTime.Enabled = false;
                    return;
                }
            }

            SortedList<int, L2Object> arr = CurrentCast.GetAffectedTargets(this);
            List<int> broadcast = new List<int>();
            broadcast.AddRange(arr.Keys);

            BroadcastPacket(new MagicSkillLaunched(this, broadcast, CurrentCast.SkillId, CurrentCast.Level));

            AddEffects(this, CurrentCast, arr);
            CurrentCast = null;
            if (CastTime != null)
                CastTime.Enabled = false;
        }

        public bool Diet = false;

        public override void UpdateMagicEffectIcons()
        {
            MagicEffectIcons m = new MagicEffectIcons();
            PartySpelled p = null;

            if (Party != null)
                p = new PartySpelled(this);

            List<AbnormalEffect> nulled = new List<AbnormalEffect>();
            lock (Effects)
            {
                foreach (AbnormalEffect ei in Effects.Where(ei => ei != null))
                {
                    if (ei.Active == 1)
                    {
                        int time = ei.GetTime();
                        m.AddIcon(ei.Id, ei.Lvl, time);

                        p?.AddIcon(ei.Id, ei.Lvl, time);
                    }
                    else
                        nulled.Add(ei);
                }

                nulled.ForEach(ei => Effects.Remove(ei));
            }

            nulled.Clear();
            SendPacket(m);

            if (p != null)
                Party?.BroadcastToMembers(p);
        }

        public void OnGameInit()
        {
            CStatsInit();
            CharacterStat.SetTemplate(ActiveClass);
            ExpAfterLogin = 0;
        }

        public List<QuestInfo> Quests = new List<QuestInfo>();

        public void quest_Talk(L2Npc npc, int questId)
        {
            foreach (QuestInfo qi in Quests.Where(qi => !qi.Completed))
                qi.Template.OnTalkToNpc(this, npc, qi.Stage);
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
                htm.Replace("<?quest_id?>", questId);
                SendPacket(htm);
                FolkNpc = npc;
            }
            else
                ShowHtmPlain(file, npc);
        }

        public bool QuestComplete(int questId)
        {
            return Quests.Where(qi => qi.Id == questId).Select(qi => qi.Completed).FirstOrDefault();
        }

        public bool QuestInProgress(int questId)
        {
            return Quests.Any(qi => (qi.Id == questId) && !qi.Completed);
        }

        public void QuestAccept(QuestInfo qi)
        {
            Quests.Add(qi);
            SendPacket(new PlaySound("ItemSound.quest_accept"));
            SendQuestList();

            //SQL_Block sqb = new SQL_Block("user_quests");
            //sqb.param("ownerId", ObjID);
            //sqb.param("qid", qi.id);
            //sqb.sql_insert(false);
        }

        public void ShowHtmPlain(string plain, L2Object o)
        {
            SendPacket(new NpcHtmlMessage(this, plain, o?.ObjId ?? -1, true));
            if (o is L2Npc)
                FolkNpc = (L2Npc)o;
        }

        public void ChangeQuestStage(int questId, int stage)
        {
            foreach (QuestInfo qi in Quests.Where(qi => qi.Id == questId))
            {
                qi.Stage = stage;
                SendPacket(new PlaySound("ItemSound.quest_middle"));

                //SQL_Block sqb = new SQL_Block("user_quests");
                //sqb.param("qstage", stage);
                //sqb.where("ownerId", ObjID);
                //sqb.where("qid", qi.id);
                //sqb.sql_update(false);
                break;
            }

            SendQuestList();
        }

        public List<QuestInfo> GetAllActiveQuests()
        {
            return Quests.Where(qi => !qi.Completed).ToList();
        }

        public void SendQuestList()
        {
            SendPacket(new QuestList(this));
        }

        public void AddExpSp(int exp, int sp, bool msg)
        {
            SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.YouEarnedS1ExpAndS2Sp);
            sm.AddNumber(exp);
            sm.AddNumber(sp);
            SendPacket(sm);

            Exp += exp;
            Sp += sp;

            StatusUpdate su = new StatusUpdate(ObjId);
            su.Add(StatusUpdate.Exp, (int)Exp);
            su.Add(StatusUpdate.Sp, Sp);
            SendPacket(su);
        }

        public void FinishQuest(int questId)
        {
            foreach (QuestInfo qi in Quests.Where(qi => qi.Id == questId))
            {
                if (!qi.Template.Repeatable)
                {
                    qi.Completed = true;
                    qi.Template = null;

                    //SQL_Block sqb = new SQL_Block("user_quests");
                    //sqb.param("qfin", 1);
                    //sqb.where("ownerId", ObjID);
                    //sqb.where("iclass", ActiveClass.id);
                    //sqb.where("qid", qi.id);
                    //sqb.sql_update(false);
                }
                else
                {
                    lock (Quests)
                        Quests.Remove(qi);
                }

                SendPacket(new PlaySound("ItemSound.quest_finish"));
                break;
            }

            SendQuestList();
        }

        public void db_restoreSkills()
        {
            //MySqlConnection connection = SQLjec.getInstance().conn();
            //MySqlCommand cmd = connection.CreateCommand();

            //connection.Open();

            //cmd.CommandText = $"SELECT * FROM user_skills WHERE ownerId={ObjID} AND iclass={ActiveClass.id}";
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

        public override void AddSkill(Skill newsk, bool updDb, bool update)
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

        public void StopQuest(QuestInfo qi, bool updDb)
        {
            if (updDb)
            {
                //SQL_Block sqb = new SQL_Block("user_quests");
                //sqb.where("ownerId", ObjID);
                //sqb.where("qid", qi.id);
                //sqb.where("iclass", ActiveClass.id);
                //sqb.sql_delete(false);
            }

            Quests.Remove(qi);
            SendPacket(new PlaySound("ItemSound.quest_giveup"));
        }

        public L2Clan Clan;

        public int ItemLimitInventory = 80,
                   ItemLimitSelling = 5,
                   ItemLimitBuying = 5,
                   ItemLimitRecipeDwarven = 50,
                   ItemLimitRecipeCommon = 50,
                   ItemLimitWarehouse = 120,
                   ItemLimitClanWarehouse = 150,
                   ItemLimitExtra = 0,
                   ItemLimitQuest = 20;

        public L2Npc FolkNpc;
        public int LastX1 = -4;
        public int LastY1;

        public override void UpdateSkillList()
        {
            SendPacket(new SkillList(this, PBlockAct, PBlockSpell, PBlockSkill));
        }

        private Timer _timerTooFar;
        public string Locale = "en";

        public void Timer()
        {
            _timerTooFar = new Timer(30 * 1000);
            _timerTooFar.Elapsed += _timeToFarTimerTask;
            _timerTooFar.Interval = 10000;
            _timerTooFar.Enabled = true;
        }

        public void _timeToFarTimerTask(object sender, ElapsedEventArgs e)
        {
            ValidateVisibleObjects(X, Y, true);
        }

        public int PCreateCommonItem = 0;
        public int PCreateItem = 0;
        public List<L2Recipe> RecipeBook = new List<L2Recipe>();

        public void RegisterRecipe(L2Recipe newr, bool updDb, bool cleanup)
        {
            lock (RecipeBook)
            {
                if (cleanup)
                    RecipeBook.Clear();

                RecipeBook.Add(newr);

                if (updDb)
                {
                    //SQL_Block sqb = new SQL_Block("user_recipes");
                    //sqb.param("ownerId", ObjID);
                    //sqb.param("recid", newr.RecipeID);
                    //sqb.param("iclass", ActiveClass.id);
                    //sqb.sql_insert(false);
                }
            }
        }

        public void db_restoreRecipes()
        {
            //MySqlConnection connection = SQLjec.getInstance().conn();
            //MySqlCommand cmd = connection.CreateCommand();

            //connection.Open();

            //cmd.CommandText = $"SELECT recid FROM user_recipes WHERE ownerId={ObjID} AND iclass={ActiveClass.id} ORDER BY tact ASC";
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

        public void UnregisterRecipe(L2Recipe rec, bool updDb)
        {
            lock (RecipeBook)
            {
                foreach (L2Recipe r in RecipeBook.Where(r => r.RecipeId == rec.RecipeId))
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

                    RecipeBook.Remove(r);

                    SendPacket(new RecipeBookItemList(this, rec.Iscommonrecipe));
                    break;
                }
            }
        }

        public bool IsAlikeDead()
        {
            return false;
        }

        public int GetClanCrestLargeId()
        {
            return Clan?.LargeCrestId ?? 0;
        }

        public List<L2Shortcut> Shortcuts = new List<L2Shortcut>();
        public int ZoneId = -1;
        public int Obsx = -1;
        public int Obsy;
        public int Obsz;

        public void RegisterShortcut(int slot, int page, int type, int id, int level, int characterType)
        {
            lock (Shortcuts)
            {
                L2Shortcut shortcut = Shortcuts.FirstOrDefault(sc => (sc.Slot == slot) && (sc.Page == page));
                if (shortcut != null)
                {
                    Shortcuts.Remove(shortcut);

                    //SQL_Block sqb = new SQL_Block("user_shortcuts");
                    //sqb.where("ownerId", ObjID);
                    //sqb.where("classId", ActiveClass.id);
                    //sqb.where("slot", _slot);
                    //sqb.where("page", _page);
                    //sqb.sql_delete(false);
                }
            }

            {
                L2Shortcut sc = new L2Shortcut(slot, page, type, id, level, characterType);
                lock (Shortcuts)
                    Shortcuts.Add(sc);

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
        public int TransformId = 0;
        public L2Transform Transform;
        public int AgationId;

        public PcTemplate BaseClass;
        public PcTemplate ActiveClass;

        public bool SubActive()
        {
            return ActiveClass != BaseClass;
        }

        public bool IsQuestCompleted(int p)
        {
            return Quests.Where(qi => qi.Id == p).Select(qi => qi.Completed).FirstOrDefault();
        }

        public int GetQuestCond(int questId)
        {
            return Quests.Where(qi => qi.Id == questId).Select(qi => qi.Stage).FirstOrDefault();
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
            if (sendMessage)
                SendPacket(new SystemMessage(SystemMessage.SystemMessageId.EarnedS1Adena).AddNumber(count));

            if (count <= 0)
                return;

            InventoryUpdate iu = new InventoryUpdate();
            iu.AddNewItem(Inventory.AddItem(57, count, this));
            SendPacket(iu);
        }

        public override string AsString()
        {
            return $"L2Player:{Name}";
        }

        public override void OnRemObject(L2Object obj)
        {
            SendPacket(new DeleteObject(obj.ObjId));
        }

        public override void OnAddObject(L2Object obj, GameserverPacket pk, string msg = null)
        {
            if (obj is L2Npc)
                SendPacket(new NpcInfo((L2Npc)obj));
            else
            {
                if (obj is L2Player)
                {
                    SendPacket(new CharInfo((L2Player)obj));

                    if (msg != null)
                        ((L2Player)obj).SendMessage(msg);
                }
                else
                {
                    if (obj is L2Item)
                        SendPacket(pk ?? new SpawnItem((L2Item)obj));
                    else
                    {
                        if (obj is L2Summon)
                            SendPacket(pk ?? new PetInfo((L2Summon)obj));
                        else
                        {
                            if (obj is L2Chair)
                                SendPacket(new StaticObject((L2Chair)obj));
                            else
                            {
                                if (obj is L2StaticObject)
                                    SendPacket(new StaticObject((L2StaticObject)obj));
                                else
                                {
                                    if (obj is L2Boat)
                                        SendPacket(new VehicleInfo((L2Boat)obj));
                                }
                            }
                        }
                    }
                }
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

        public void Untransform()
        {
            if (Transform == null)
                return;

            Transform.Template.OnTransformEnd(this);
            Transform = null;
        }

        public override void DeleteMe()
        {
            CleanUp();
            //Store();
            base.DeleteMe();
        }

        public void CleanUp()
        {
            if (!IsRestored)
                return;

            Party?.Leave(this);

            Summon?.UnSummon();
            Online = 0;
            UpdatePlayer();
            L2World.Instance.RemovePlayer(this);
            DecayMe();
           
        }

        public bool HasItem(int itemId, int count)
        {
            return Inventory.Items.Where(item => item.Template.ItemId == itemId).Any(item => item.Count >= count);
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

            if (_isSitting)
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

        public void SetTransform(L2Transform tr)
        {
            Transform = tr;
            Transform.Owner = this;
            Transform.Template.OnTransformStart(this);
        }

        public int ViewingAdminPage;
        public int ViewingAdminTeleportGroup = -1;
        public int TeleportPayId;
        public int LastMinigameScore;
        public short ClanType;
        public int Fame;

        public void ShowHtmAdmin(string val, bool plain)
        {
            SendPacket(new TutorialShowHtml(this, val, true));

            ViewingAdminPage = 1;
        }

        public void ShowHtmBbs(string val)
        {
            ShowBoard.SeparateAndSend(val, this);
        }

        public void SendItemList(bool open = false)
        {
            SendPacket(new ItemList(this, open));
           // SendPacket(new ExQuestItemList(this));
        }

        public void UpdateWeight()
        {
            int oldweight = CurrentWeight;
            int total = 0;
            //if (!_diet)
            //    foreach (L2Item it in Inventory.Items.Values.Where(it => it.Template.Weight != 0))
            //{
            //        if (it.Template.isStackable())
            //            total += it.Template.Weight * it.Count;
            //        else
            //            total += it.Template.Weight;
            //}

            CurrentWeight = total >= int.MaxValue ? int.MaxValue : total;

            if (oldweight == total)
                return;

            StatusUpdate su = new StatusUpdate(ObjId);
            su.Add(StatusUpdate.CurLoad, CurrentWeight);
            SendPacket(su);

            int weightproc = (total * 1000) / (int)CharacterStat.GetStat(EffectType.BMaxWeight);

            int newWeightPenalty;
            if (weightproc < 500)
                newWeightPenalty = 0;
            else
            {
                if (weightproc < 666)
                    newWeightPenalty = 1;
                else
                {
                    if (weightproc < 800)
                        newWeightPenalty = 2;
                    else
                    {
                        if (weightproc < 1000)
                            newWeightPenalty = 3;
                        else
                            newWeightPenalty = 4;
                    }
                }
            }

            if (PenaltyWeight == newWeightPenalty)
                return;

            if (newWeightPenalty > 0)
                AddSkill(4270, newWeightPenalty, false, true);
            else
                RemoveSkill(4270, false, true);

            PenaltyWeight = newWeightPenalty;

            SendPacket(new EtcStatusUpdate(this));
        }

        public bool CheckFreeWeight(int weight)
        {
            if ((CurrentWeight + weight) >= CharacterStat.GetStat(EffectType.BMaxWeight))
                return false;

            return true;
        }

        public bool CheckFreeWeight80(int weight)
        {
            if ((CurrentWeight + weight) >= (CharacterStat.GetStat(EffectType.BMaxWeight) * .8))
                return false;

            return true;
        }


        public byte ClanRank()
        {
            if ((ClanId == 0) || (Clan == null))
                return 0;

            bool leader = Clan.LeaderId == ObjId;
            EClanRank rank = EClanRank.Vagabond;
            switch (ClanType)
            {
                case (short)EClanType.ClanMain:
                {
                    switch (Clan.Level)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                            rank = EClanRank._1;
                            break;
                        case 4:
                            if (leader)
                                rank = EClanRank._3;
                            break;
                        case 5:
                            rank = leader ? EClanRank._4 : EClanRank._3;
                            break;
                        case 6:
                            if (leader)
                                rank = EClanRank._5;
                            else
                            {
                                if (Clan.IsSubLeader(ObjId, new[] { EClanType.ClanKnight1, EClanType.ClanKnight2 }) != EClanType.None)
                                    rank = EClanRank._4;
                                else
                                    rank = EClanRank._3;
                            }
                            break;
                        case 7:
                            if (leader)
                                rank = EClanRank._7;
                            else
                            {
                                if (Clan.IsSubLeader(ObjId, new[] { EClanType.ClanKnight1, EClanType.ClanKnight2 }) != EClanType.None)
                                    rank = EClanRank._6;
                                else
                                {
                                    if (Clan.IsSubLeader(ObjId, new[] { EClanType.ClanKnight3, EClanType.ClanKnight4, EClanType.ClanKnight5, EClanType.ClanKnight6 }) != EClanType.None)
                                        rank = EClanRank._5;
                                    else
                                        rank = EClanRank._4;
                                }
                            }
                            break;
                        case 8:
                            if (leader)
                                rank = EClanRank._8;
                            else
                            {
                                if (Clan.IsSubLeader(ObjId, new[] { EClanType.ClanKnight1, EClanType.ClanKnight2 }) != EClanType.None)
                                    rank = EClanRank._7;
                                else
                                {
                                    if (Clan.IsSubLeader(ObjId, new[] { EClanType.ClanKnight3, EClanType.ClanKnight4, EClanType.ClanKnight5, EClanType.ClanKnight6 }) != EClanType.None)
                                        rank = EClanRank._6;
                                    else
                                        rank = EClanRank._5;
                                }
                            }
                            break;
                        case 9:
                            if (leader)
                                rank = EClanRank._9;
                            else
                            {
                                if (Clan.IsSubLeader(ObjId, new[] { EClanType.ClanKnight1, EClanType.ClanKnight2 }) != EClanType.None)
                                    rank = EClanRank._8;
                                else
                                {
                                    if (Clan.IsSubLeader(ObjId, new[] { EClanType.ClanKnight3, EClanType.ClanKnight4, EClanType.ClanKnight5, EClanType.ClanKnight6 }) != EClanType.None)
                                        rank = EClanRank._7;
                                    else
                                        rank = EClanRank._6;
                                }
                            }
                            break;
                        case 10:
                            if (leader)
                                rank = EClanRank._10;
                            else
                            {
                                if (Clan.IsSubLeader(ObjId, new[] { EClanType.ClanKnight1, EClanType.ClanKnight2 }) != EClanType.None)
                                    rank = EClanRank._9;
                                else
                                {
                                    if (Clan.IsSubLeader(ObjId, new[] { EClanType.ClanKnight3, EClanType.ClanKnight4, EClanType.ClanKnight5, EClanType.ClanKnight6 }) != EClanType.None)
                                        rank = EClanRank._8;
                                    else
                                        rank = EClanRank._7;
                                }
                            }
                            break;
                        case 11:
                            if (leader)
                                rank = EClanRank._11;
                            else
                            {
                                if (Clan.IsSubLeader(ObjId, new[] { EClanType.ClanKnight1, EClanType.ClanKnight2 }) != EClanType.None)
                                    rank = EClanRank._10;
                                else
                                {
                                    if (Clan.IsSubLeader(ObjId, new[] { EClanType.ClanKnight3, EClanType.ClanKnight4, EClanType.ClanKnight5, EClanType.ClanKnight6 }) != EClanType.None)
                                        rank = EClanRank._9;
                                    else
                                        rank = EClanRank._8;
                                }
                            }
                            break;
                    }
                }

                    break;
                case (short)EClanType.ClanAcademy:
                {
                    rank = EClanRank._1;
                }
                    break;
                case (short)EClanType.ClanKnight1:
                case (short)EClanType.ClanKnight2:
                {
                    switch (Clan.Level)
                    {
                        case 6:
                            rank = EClanRank._2;
                            break;
                        case 7:
                            rank = EClanRank._3;
                            break;
                        case 8:
                            rank = EClanRank._4;
                            break;
                        case 9:
                            rank = EClanRank._5;
                            break;
                        case 10:
                            rank = EClanRank._6;
                            break;
                        case 11:
                            rank = EClanRank._7;
                            break;
                    }
                }

                    break;
                case (short)EClanType.ClanKnight3:
                case (short)EClanType.ClanKnight4:
                case (short)EClanType.ClanKnight5:
                case (short)EClanType.ClanKnight6:
                {
                    switch (Clan.Level)
                    {
                        case 7:
                            rank = EClanRank._2;
                            break;
                        case 8:
                            rank = EClanRank._3;
                            break;
                        case 9:
                            rank = EClanRank._4;
                            break;
                        case 10:
                            rank = EClanRank._5;
                            break;
                        case 11:
                            rank = EClanRank._6;
                            break;
                    }
                }

                    break;
            }

            if ((Noblesse == 1) && ((byte)rank < 5))
                rank = EClanRank._5;

            if ((Heroic == 1) && ((byte)rank < 8))
                rank = EClanRank._8;

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

        public string PenaltyClanCreate = "0";
        public string PenaltyClanJoin = "0";

        public void setPenalty_ClanCreate(DateTime time, bool sql)
        {
            PenaltyClanCreate = DateTime.Now < time ? time.ToString("yyyy-MM-dd HH-mm-ss") : "0";

            if (sql) { }
        }

        public void setPenalty_ClanJoin(DateTime time, bool sql)
        {
            PenaltyClanJoin = DateTime.Now < time ? time.ToString("yyyy-MM-dd HH-mm-ss") : "0";

            if (sql) { }
        }

        public bool IsRestored;

        public void TotalRestore()
        {
            if (IsRestored)
                return;

            OnGameInit();
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

            //cmd.CommandText = $"SELECT * FROM user_shortcuts WHERE ownerId={ObjID} and classId={ActiveClass.id}";
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

            SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.YourSoulCountHasIncreasedByS1NowAtS2);
            sm.AddNumber(count);
            sm.AddNumber(Souls);
            SendPacket(sm);
        }

        public void IncreaseSouls()
        {
            if (((Souls + 1) > 45) || (Souls == 45))
            {
                SendSystemMessage(SystemMessage.SystemMessageId.SoulCannotBeIncreasedAnymore);
                return;
            }

            AddSouls(1);
        }

        public bool IsCursed = false;

        public byte PartyState;
        public L2Player Requester;
        public int ItemDistribution;

        public void PendToJoinParty(L2Player asker, int askerItemDistribution)
        {
            PartyState = 1;
            Requester = asker;
            Requester.ItemDistribution = askerItemDistribution;
            SendPacket(new AskJoinParty(asker.Name, askerItemDistribution));
        }

        public void ClearPend()
        {
            PartyState = 0;
            Requester = null;
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
            int val = 0; //Inventory.getWeaponEnchanment();

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
            {
                if (!((((L2Summon)target).Owner != null) && (((L2Summon)target).Owner.ObjId == ObjId)))
                    color = ((L2Summon)target).Level - Level;
            }

            SendPacket(new MyTargetSelected(target.ObjId, color));
        }

        public override void OnOldTargetSelection(L2Object target)
        {
            double dis = Calcs.CalculateDistance(this, target, true);
            if (dis < 151)
                target.NotifyAction(this);
            else
                TryMoveTo(target.X, target.Y, target.Z);

            SendActionFailed();
        }

        private Timer _petSummonTime,
                      _nonpetSummonTime;
        private int _petId = -1;
        private L2Item _petControlItem;

        public void PetSummon(L2Item item, int npcId, bool isPet = true)
        {
            if (Summon != null)
            {
                SendSystemMessage(SystemMessage.SystemMessageId.YouAlreadyHaveAPet);
                return;
            }

            if (isPet)
            {
                if (_petSummonTime == null)
                {
                    _petSummonTime = new Timer
                    {
                        Interval = 5000
                    };
                    _petSummonTime.Elapsed += PetSummonEnd;
                }

                _petSummonTime.Enabled = true;
                SendSystemMessage(SystemMessage.SystemMessageId.SummonAPet);
            }
            else
            {
                if (_nonpetSummonTime == null)
                {
                    _nonpetSummonTime = new Timer
                    {
                        Interval = 5000
                    };
                    _nonpetSummonTime.Elapsed += NonpetSummonEnd;
                }

                _nonpetSummonTime.Enabled = true;
            }

            _petId = npcId;
            _petControlItem = item;

            BroadcastPacket(new MagicSkillUse(this, this, 1111, 1, 5000));
            SendPacket(new SetupGauge(ObjId, SetupGauge.SgColor.Blue, 4900));
        }

        private void PetSummonEnd(object sender, ElapsedEventArgs e)
        {
            L2Pet pet = new L2Pet();
            //pet.setTemplate(NpcTable.Instance.GetNpcTemplate(PetID));
            pet.SetOwner(this);
            pet.ControlItem = _petControlItem;
            // pet.sql_restore();
            pet.SpawmMe();

            _petSummonTime.Enabled = false;
        }

        private void NonpetSummonEnd(object sender, ElapsedEventArgs e)
        {
            L2Summon summon = new L2Summon();
            //summon.setTemplate(NpcTable.Instance.GetNpcTemplate(PetID));
            summon.SetOwner(this);
            summon.ControlItem = _petControlItem;
            summon.SpawmMe();

            _nonpetSummonTime.Enabled = false;
        }

        public override bool CantMove()
        {
            if ((_petSummonTime != null) && _petSummonTime.Enabled)
                return true;
            if ((_nonpetSummonTime != null) && _nonpetSummonTime.Enabled)
                return true;

            return base.CantMove();
        }

        public override L2Character[] GetPartyCharacters()
        {
            List<L2Character> chars = new List<L2Character>
            {
                this
            };
            if (Summon != null)
                chars.Add(Summon);

            if (Party == null)
                return chars.ToArray();

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
            if ((_petSummonTime != null) && _petSummonTime.Enabled)
                _petSummonTime.Enabled = false;

            if ((_nonpetSummonTime != null) && _nonpetSummonTime.Enabled)
                _nonpetSummonTime.Enabled = false;

            base.AbortCast();
        }

        public override void StartAi()
        {
            AiCharacter = new PlayerAi(this);
        }

        public List<SpecEffect> SpecEffects = new List<SpecEffect>();

        public bool IsSittingInProgress()
        {
            return (_sitTime != null) && _sitTime.Enabled;
        }

        public bool IsSitting()
        {
            return _isSitting;
        }

        private Timer _sitTime;
        private bool _isSitting;

        public void Sit()
        {
            if (_sitTime == null)
            {
                _sitTime = new Timer
                {
                    Interval = 2500
                };
                _sitTime.Elapsed += SitEnd;
            }

            _sitTime.Enabled = true;
            BroadcastPacket(new ChangeWaitType(this, ChangeWaitType.Sit));
        }

        public void Stand()
        {
            _sitTime.Enabled = true;
            BroadcastPacket(new ChangeWaitType(this, ChangeWaitType.Stand));
            //TODO stop relax effect
        }

        private void SitEnd(object sender, ElapsedEventArgs e)
        {
            _sitTime.Enabled = false;
            _isSitting = !_isSitting;

            if (_isSitting || (_chair == null))
                return;

            _chair.IsUsedAlready = false;
            _chair = null;
        }

        private L2Chair _chair;
        public L2Boat Boat;
        public int BoatX;
        public int BoatY;
        public int BoatZ;

        public void SetChair(L2Chair chairObj)
        {
            _chair = chairObj;
            _chair.IsUsedAlready = true;
            BroadcastPacket(new ChairSit(ObjId, chairObj.StaticId));
        }

        public bool IsOnShip()
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
            double timeAtk = CharacterStat.GetStat(EffectType.BAttackSpd);
            bool dual = false,
                 ranged = false,
                 ss = false;
            if (weapon != null)
                ss = weapon.Soulshot;
            else
            {
                timeAtk = (1362 * 345) / timeAtk;
                dual = true;
            }

            if (!Calcs.CheckIfInRange((int)dist, this, target, true))
            {
                SendMessage($"too far {dist}");
                TryMoveTo(target.X, target.Y, target.Z);
                return;
            }

            if ((reqMp > 0) && (reqMp > CurMp))
            {
                SendMessage($"no mp {CurMp} {reqMp}");
                SendActionFailed();
                return;
            }

            if (ranged)
            {
                SendPacket(new SetupGauge(ObjId, SetupGauge.SgColor.Red, (int)timeAtk));
                //Inventory.destroyItem(SecondaryWeaponSupport, 1, false, true);
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

        public override void AttackDoHit(object sender, ElapsedEventArgs e)
        {
            if ((CurrentTarget != null) && !CurrentTarget.Dead)
            {
                if (!Hit1.Miss)
                {
                    if (Hit1.Crit)
                        SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1LandedACriticalHit).AddPlayerName(Name));

                    SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1HasGivenC2DamageOfS3).AddPlayerName(Name).AddName(CurrentTarget).AddNumber(Hit1.Damage));
                    CurrentTarget.ReduceHp(this, Hit1.Damage);

                    if (CurrentTarget is L2Player)
                        CurrentTarget.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1HasReceivedS3DamageFromC2).AddName(CurrentTarget).AddName(this).AddNumber(Hit1.Damage));
                }
                else
                {
                    SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1AttackWentAstray).AddPlayerName(Name));

                    if (CurrentTarget is L2Player)
                    {
                        CurrentTarget.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1HasEvadedC2Attack).AddName(CurrentTarget).AddName(this));
                        ((L2Player)CurrentTarget).AiCharacter.NotifyEvaded(this);
                    }
                }
            }

            AttackToHit.Enabled = false;
        }

        public override void AttackDoHit2Nd(object sender, ElapsedEventArgs e)
        {
            if ((CurrentTarget != null) && !CurrentTarget.Dead)
            {
                if (!Hit2.Miss)
                {
                    if (Hit2.Crit)
                        SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1LandedACriticalHit).AddName(this));

                    SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1HasGivenC2DamageOfS3).AddName(this).AddName(CurrentTarget).AddNumber(Hit2.Damage));
                    CurrentTarget.ReduceHp(this, Hit2.Damage);

                    if (CurrentTarget is L2Player)
                        CurrentTarget.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1HasReceivedS3DamageFromC2).AddName(CurrentTarget).AddName(this).AddNumber(Hit2.Damage));
                }
                else
                {
                    SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1AttackWentAstray).AddPlayerName(Name));

                    if (CurrentTarget is L2Player)
                    {
                        CurrentTarget.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1HasEvadedC2Attack).AddName(CurrentTarget).AddName(this));
                        ((L2Player)CurrentTarget).AiCharacter.NotifyEvaded(this);
                    }
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
                if (TransformId > 0)
                    return Transform.Template.GetHeight(Sex);

                if (MountType > 0)
                    return MountedTemplate.CollisionHeight;

                return Sex == 0 ? BaseClass.CollisionHeight : BaseClass.CollisionHeightFemale;
            }
        }

        public List<int> AutoSoulshots = new List<int>();
        public List<int> SetKeyItems;
        public int SetKeyId;

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

        public void UnMount()
        {
            BroadcastPacket(new Ride(this, false));
            MountedTemplate = null;
            BroadcastUserInfo();
        }

        public SortedList<int, int> CurrentTrade;
        public int Sstt;

        public int AddItemToTrade(int objId, int num)
        {
            if (CurrentTrade == null)
                CurrentTrade = new SortedList<int, int>();

            if (CurrentTrade.ContainsKey(objId))
            {
                CurrentTrade[objId] += num;
                return CurrentTrade[objId];
            }

            CurrentTrade.Add(objId, num);
            return num;
        }

        public void NotifyDayChange(GameserverPacket pk)
        {
            SendPacket(pk);
            if (pk is SunSet) //включаем ночные скилы
                AiCharacter.NotifyOnStartNight();
            else
                AiCharacter.NotifyOnStartDay();
        }

        public int VehicleId => Boat?.ObjId ?? 0;

        public void Revive(double percent)
        {
            BroadcastPacket(new Revive(ObjId));
            Dead = false;
            StartRegeneration();
        }

        private DateTime _pingTimeout;
        private int _lastPingId;
        public int Ping = -1;
        public MultiSellList CustomMultiSellList;
        public int LastRequestedMultiSellId = -1;
        public int AttackingId;
        public SortedList<int, AcquireSkill> ActiveSkillTree;

        public void RequestPing()
        {
            _lastPingId = new Random().Next(int.MaxValue);
            NetPing ping = new NetPing(_lastPingId);
            _pingTimeout = DateTime.Now;
            SendPacket(ping);
        }

        public void UpdatePing(int id, int ms, int unk)
        {
            if (_lastPingId != id)
            {
                Log.Warn($"player fail to ping respond right {id} {_lastPingId} at {_pingTimeout.ToLocalTime()}");
                return;
            }

            Ping = ms;
            SendMessage($"Your connection latency is {ms}");
        }

        public void InstantTeleportWithItem(int x, int y, int z, int id, int cnt)
        {
            //Inventory.destroyItem(id, cnt, true, true);
        }

        public void RedistExp(L2Warrior mob)
        {
            double expPet = 0.0;
            if ((Summon != null) && (Summon.ConsumeExp > 0))
                expPet = (Summon.ConsumeExp / 100.0) + 1;

            double expReward = mob.Template.Exp / 1.0;
            int sp = mob.Template.Sp;
            SendMessage($"debug: expPet {expPet}");
            SendMessage($"debug: mob.Template {mob.Template.Exp} @");
            SendMessage($"debug: expReward {expReward}");
            SendMessage($"debug: sp {sp}");

            byte oldLvl = Level;
            Exp += (long)expReward;
            byte newLvl = Experience.GetLevel(Exp);
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

        public byte ClanLevel => Clan?.Level ?? (byte)0;

        public void BroadcastSkillUse(int skillId)
        {
            Skill skill = SkillTable.Instance.Get(skillId);
            BroadcastPacket(new MagicSkillUse(this, this, skill.SkillId, skill.Level, skill.SkillHitTime));
        }

        public bool ClanLeader => Clan?.LeaderId == ObjId;

        public bool HavePledgePower(int bit)
        {
            return (Clan != null) && Clan.HasRights(this, bit);
        }

        public override L2Item GetWeaponItem()
        {
            return null;
        }

        public void UpdateAgathionEnergy(int count)
        {
            SendMessage($"@UpdateAgathionEnergy {count}");
        }

        public List<Cubic> Cubics = new List<Cubic>();

        public void StopCubic(Cubic cubic)
        {
            foreach (Cubic cub in Cubics.Where(cub => cub.Template.Id == cubic.Template.Id))
            {
                lock (Cubics)
                    Cubics.Remove(cub);

                BroadcastUserInfo();
                break;
            }
        }

        public void AddCubic(Cubic cubic, bool update)
        {
            int max = (int)CharacterStat.GetStat(EffectType.PCubicMastery);
            if (max == 0)
                max = 1;

            if (Cubics.Count == max)
            {
                Cubic cub = Cubics[0];
                cub.OnEnd(false);
                lock (Cubics)
                    Cubics.RemoveAt(0);
            }

            foreach (Cubic cub in Cubics.Where(cub => cub.Template.Id == cubic.Template.Id))
            {
                lock (Cubics)
                {
                    cub.OnEnd(false);
                    Cubics.Remove(cub);
                }
                break;
            }

            cubic.OnSummon();
            Cubics.Add(cubic);
            if (update)
                BroadcastUserInfo();
        }

        public override void DoDie(L2Character killer, bool bytrigger)
        {
            if (Cubics.Count > 0)
            {
                Cubics.ForEach(cub => cub.OnEnd(false));
                Cubics.Clear();
            }

            base.DoDie(killer, bytrigger);
        }

        public bool CharDeleteTimeExpired() => (DeleteTime > 0) && (DeleteTime <= Utilz.CurrentTimeMillis());

        public int RemainingDeleteTime() => AccessLevel > -100 ? (DeleteTime > 0 ? (int)((DeleteTime - Utilz.CurrentTimeMillis()) / 1000) : 0) : -1;

        public void SetCharDeleteTime() => DeleteTime = Utilz.CurrentTimeMillis() + (Config.Config.Instance.GameplayConfig.Server.Client.DeleteCharAfterDays * 86400000L);
    }
}