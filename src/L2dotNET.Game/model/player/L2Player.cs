using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Timers;
using L2dotNET.Game.model.communities;
using L2dotNET.Game.model.inventory;
using L2dotNET.Game.model.items;
using L2dotNET.Game.model.npcs;
using L2dotNET.Game.model.npcs.decor;
using L2dotNET.Game.model.playable;
using L2dotNET.Game.model.player;
using L2dotNET.Game.model.player.ai;
using L2dotNET.Game.model.player.partials;
using L2dotNET.Game.model.player.telebooks;
using L2dotNET.Game.model.player.transformation;
using L2dotNET.Game.model.skills2;
using L2dotNET.Game.model.skills2.effects;
using L2dotNET.Game.model.vehicles;
using L2dotNET.Game.network;
using L2dotNET.Game.network.l2send;
using L2dotNET.Game.network.loginauth;
using L2dotNET.Game.tables;
using L2dotNET.Game.tools;
using L2dotNET.Game.world;
using MySql.Data.MySqlClient;
using L2dotNET.Game.model.npcs.cubic;
using Ninject;
using L2dotNET.Services.Contracts;
using L2dotNET.Models;
using L2dotNET.Game.templates;
using L2dotNET.Game.Enums;
using log4net;

namespace L2dotNET.Game
{
    [Synchronization]
    public partial class L2Player : L2Character
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(L2Player));

        [Inject]
        public IPlayerService playerService { get { return GameServer.Kernel.Get<IPlayerService>(); } }


        public string AccountName { get; set; }
        public ClassId ClassId { get; set; }
        public L2PartyRoom PartyRoom { get; set; }
        public L2Party Party { get; set; }
        public byte Sex { get; set; }
        public int HairStyle { get; set; }
        public int HairColor { get; set; }
        public int Face { get; set; }
        private bool _whisperBlock = false;
        public bool WhieperBlock { get { return _whisperBlock; } set { _whisperBlock = value; } }
        public GameClient Gameclient { get; set; }
        public long Exp { get; set; }
        private long expOnDeath = 0;
        public long ExpOnDeath { get { return expOnDeath; } set { expOnDeath = value; } }
        private long expAfterLogin = 0;
        public long ExpAfterLogin { get { return expAfterLogin; } set { expAfterLogin = value; } }
        public int SP { get; set; }
        public int MaxCp { get; set; }
        public double CurCp { get; set; }
        public int MaxMp { get; set; }
        public double CurMp { get; set; }
        public int Karma = 0;
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

        public L2Player RestorePlayer(int id, GameClient client)
        {
            L2Player player = new L2Player();
            player.ObjID = id;

            player.Gameclient = client;

            PlayerModel playerModel = playerService.GetAccountByLogin(id);

            player.Name = playerModel.Name;
            player.Title = playerModel.Title;
            player.Level = (byte)playerModel.Level;
            player.CurHP = playerModel.CurHp;
            player.CurMP = playerModel.CurMp;
            player.CurCP = playerModel.CurCp;

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
            byte bclass = (byte)playerModel.BaseClass;
            byte aclass = (byte)playerModel.ClassId;

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


            player.Inventory = new InvPC();
            player.Inventory._owner = player;
            player.Refund = new InvRefund(player);

            player.CStatsInit();

            restoreItems(player);


            return player;
        }

        public static L2Player create()
        {
            L2Player player = new L2Player();
            player.ObjID = IdFactory.Instance.nextId();

            player.Inventory = new InvPC();
            player.Inventory._owner = player;

            return player;
        }

        public void UpdatePlayer()
        {
            PlayerModel playerModel = new PlayerModel()
            {
                ObjectId = ObjID,
                Level = Level,
                MaxHp = (int)MaxHp,
                CurHp = (int)CurHP,
                MaxCp = (int)MaxCp,
                CurCp = (int)CurCp,
                MaxMp = (int)MaxMp,
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
                Sp = SP,
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
                IsIn7sDungeon = IsIn7sDungeon,
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
                saveInventory();

                L2World.Instance.UnrealiseEntry(this, true);
            }
        }

        public int getPaperdollObjectId(int p)
        {
            return Inventory.getPaperdollObjectId(p); ;
        }

        public int getPaperdollItemId(int p)
        {
            return Inventory.getPaperdollId(p);
        }

        public int getWeaponAugmentation()
        {
            return Inventory.getWeaponAugmentation();
        }

        public int INT { get { return ActiveClass.BaseINT; } }

        public int STR { get { return ActiveClass.BaseSTR; } }

        public int CON { get { return ActiveClass.BaseCON; } }

        public int MEN { get { return ActiveClass.BaseMEN; } }

        public int DEX { get { return ActiveClass.BaseDEX; } }

        public int WIT { get { return ActiveClass.BaseWIT; } }

        public int getPaperdollAugmentationId(int p)
        {
            return Inventory.getPaperdollAugmentId(p);
        }

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

            set { Clan = ClanTable.getInstance().getClan(value); }
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

        public InvPC Inventory;
        public InvRefund Refund;

        public L2Item getItemByObjId(int itemobj)
        {
            try
            {
                return Inventory.getByObject(itemobj);
            }
            catch
            {
                Console.WriteLine("player: cant find item obj " + itemobj);
                return null;
            }
        }

        public void setPaperdoll(int pdollId, L2Item item, bool update)
        {
            Inventory.setPaperdoll(pdollId, item, update);
        }

        public override void sendPacket(GameServerNetworkPacket pk)
        {
            Gameclient.sendPacket(pk);
        }

        private ActionFailed af;

        public override void sendActionFailed()
        {
            if (af == null)
                af = new ActionFailed();

            sendPacket(af);
        }

        public override void sendSystemMessage(int id)
        {
            sendPacket(new SystemMessage(id));
        }

        public int _penaltyWeight = 0;
        public int _penalty_grade = 0;

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
            else
                player.sendActionFailed();
        }

        public long getAdena()
        {
            long adena = 0;

            foreach (L2Item item in Inventory.Items.Values)
            {
                if (item.Template.ItemID == 57)
                {
                    adena = item.Count;
                    break;
                }
            }

            return adena;
        }

        public void addAdena(long count, bool msg, bool update)
        {
            this.Inventory.addItem(57, count, msg, update);
        }

        public override void broadcastUserInfo()
        {
            sendPacket(new UserInfo(this));
            updateAbnormalEventEffect();

            broadcastPacket(new CharInfo(this), true);
        }

        public override void sendMessage(string p)
        {
            sendPacket(new SystemMessage(1987).AddString(p));
        }

        public int _currentFocusEnergy = 0;
        public int getForceIncreased()
        {
            return _currentFocusEnergy;
        }

        public static void restoreItems(L2Player player)
        {
            //MySqlConnection connection = SQLjec.getInstance().conn();
            //MySqlCommand cmd = connection.CreateCommand();

            //connection.Open();

            //cmd.CommandText = "SELECT * FROM user_items WHERE ownerId=" + player.ObjID;
            //cmd.CommandType = CommandType.Text;

            //MySqlDataReader reader = cmd.ExecuteReader();

            //while (reader.Read())
            //{
            //    int itemId = reader.GetInt32("itemId");
            //    ItemTemplate template = ItemTable.getInstance().getItem(itemId);
            //    if (template == null)
            //        continue;

            //    L2Item item = new L2Item(template, true);
            //    item.ObjID = reader.GetInt32("iobjectId");
            //    item.Count = reader.GetInt32("icount");
            //    item.Enchant = reader.GetInt16("ienchant");
            //    item.AugmentationID = reader.GetInt32("iaugment");
            //    item.Durability = reader.GetInt32("imana");
            //    item.SetLimitedHour(reader.GetString("lifetime"));
            //    item._isEquipped = reader.GetInt16("iequipped");
            //    item._paperdollSlot = reader.GetInt32("iequip_data");

            //    string location = reader.GetString("ilocation");
            //    item.Location = (L2Item.L2ItemLocation)Enum.Parse(typeof(L2Item.L2ItemLocation), location);

            //    switch (item.Location)
            //    {
            //        case L2Item.L2ItemLocation.paperdoll:
            //        case L2Item.L2ItemLocation.inventory:
            //            {
            //                item.SlotLocation = reader.GetInt32("iloc_data");
            //                player.Inventory.Items.Add(item.ObjID, item);
            //                if (item._isEquipped == 1)
            //                {
            //                    player.Inventory.setPaperdollDirect(item._paperdollSlot, item);
            //                }
            //            }
            //            break;
            //        case L2Item.L2ItemLocation.warehouse:
            //            {
            //                if (player._warehouse == null)
            //                {
            //                    player._warehouse = new InvPrivateWarehouse(player);
            //                }

            //                player._warehouse.dbLoad(item);
            //            }
            //            break;
            //        case L2Item.L2ItemLocation.pet:
            //            {
            //                item._petId = reader.GetInt32("iloc_data");
            //                player.Inventory.Items.Add(item.ObjID, item);
            //            }
            //            break;
            //    }

            //    //item.CustomType1 = reader.GetInt32("ict1");
            //    //item.CustomType2 = reader.GetInt32("ict2");
            //}

            //reader.Close();
            //connection.Close();
        }

        public void saveInventory()
        {
            foreach (L2Item item in Inventory.Items.Values)
            {
                item.sql_update();
            }
        }

        public TSkill getSkill(int _magicId)
        {
            if (_skills.ContainsKey(_magicId))
                return _skills[_magicId];

            return null;
        }

        public override bool isCastingNow()
        {
            if (petSummonTime != null)
                return petSummonTime.Enabled;


            if (nonpetSummonTime != null)
                return nonpetSummonTime.Enabled;

            return base.isCastingNow();
        }

        public void updateReuse()
        {
            sendPacket(new SkillCoolTime(this));
        }

        public void castSkill(TSkill skill, bool _ctrlPressed, bool _shiftPressed)
        {
            if (isCastingNow())
            {
                sendActionFailed();
                return;
            }

            if (isSittingInProgress() || isSitting())
            {
                sendActionFailed();
                return;
            }

            if (!skill.ConditionOk(this))
            {
                sendActionFailed();
                return;
            }

            L2Character target = skill.getTargetCastId(this);

            if (target == null)
            {
                sendSystemMessage(50); //Your target cannot be found.
                sendActionFailed();
                return;
            }

            if (skill.cast_range != -1)
            {
                double dis = Calcs.calculateDistance(this, target, true);
                if (dis > skill.cast_range)
                {
                    tryMoveTo(target.X, target.Y, target.Z);
                    sendActionFailed();
                    return;
                }
            }

            if (skill.reuse_delay > 0)
            {
                if (_reuse.ContainsKey(skill.skill_id))
                {
                    TimeSpan ts = _reuse[skill.skill_id].stopTime - DateTime.Now;

                    if (ts.TotalMilliseconds > 0)
                    {
                        if (ts.TotalHours > 0)
                        {
                            //There are $s2 hour(s), $s3 minute(s), and $s4 second(s) remaining in $s1's re-use time.
                            SystemMessage sm = new SystemMessage(2305);
                            sm.AddSkillName(skill.skill_id, skill.level);
                            sm.AddNumber((int)ts.Hours);
                            sm.AddNumber((int)ts.Minutes);
                            sm.AddNumber((int)ts.Seconds);
                            sendPacket(sm);
                        }
                        else if (ts.TotalMinutes > 0)
                        {
                            //There are $s2 minute(s), $s3 second(s) remaining in $s1's re-use time.
                            SystemMessage sm = new SystemMessage(2304);
                            sm.AddSkillName(skill.skill_id, skill.level);
                            sm.AddNumber((int)ts.Minutes);
                            sm.AddNumber((int)ts.Seconds);
                            sendPacket(sm);
                        }
                        else
                        {
                            //There are $s2 second(s) remaining in $s1's re-use time.
                            SystemMessage sm = new SystemMessage(2303);
                            sm.AddSkillName(skill.skill_id, skill.level);
                            sm.AddNumber((int)ts.Seconds);
                            sendPacket(sm);
                        }

                        sendActionFailed();
                        return;
                    }
                }
            }

            if (skill.mp_consume1 > 0 || skill.mp_consume2 > 0)
            {
                if (CurMP < skill.mp_consume1 + skill.mp_consume2)
                {
                    sendSystemMessage(24); //Not enough MP.
                    sendActionFailed();
                    return;
                }
            }

            if (skill.hp_consume > 0)
            {
                if (CurHP < skill.hp_consume)
                {
                    sendSystemMessage(23); //Not enough HP.
                    sendActionFailed();
                    return;
                }
            }

            if (skill.ConsumeItemId != 0)
            {
                long count = Inventory.getItemCount(skill.ConsumeItemId);
                if (count < skill.ConsumeItemCount)
                {
                    //$s1 cannot be used due to unsuitable terms.
                    sendPacket(new SystemMessage(113).AddSkillName(skill.skill_id, skill.level));
                    sendActionFailed();
                    return;
                }
            }

            byte blowOk = 0;
            if (skill.effects.Count > 0)
            {
                bool fail = false;
                foreach (TEffect ef in skill.effects)
                {
                    if (!ef.canUse(this))
                    {
                        sendActionFailed();
                        fail = true;
                        break;
                    }

                    if (ef is i_fatal_blow && blowOk == 0)
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
                _reuse.Add(reuse.id, reuse);
                updateReuse();
            }

            //You use $s1.
            sendPacket(new SystemMessage(46).AddSkillName(skill.skill_id, skill.level));

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

            if (skill.ConsumeItemId != 0)
            {
                Inventory.destroyItem(skill.ConsumeItemId, skill.ConsumeItemCount, true, true);
            }

            int hitTime = (int)skill.skill_hit_time;

            int hitT = hitTime > 0 ? (int)(hitTime * 0.95) : 0;
            currentCast = skill;

            if (hitTime > 0)
            {
                sendPacket(new SetupGauge(ObjID, SetupGauge.SG_color.blue, hitTime - 20));
            }

            broadcastPacket(new MagicSkillUse(this, target, skill, hitTime == 0 ? 20 : hitTime, blowOk));
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
        }

        private void castEnd(object sender = null, ElapsedEventArgs e = null)
        {
            if (currentCast.mp_consume2 > 0)
            {
                if (CurMP < currentCast.mp_consume2)
                {
                    sendSystemMessage(24); //Not enough MP.
                    sendActionFailed();

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
                    sendSystemMessage(748); //The distance is too far and so the casting has been stopped.
                    sendActionFailed();

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
                castTime.Enabled = false;
        }

        public bool _diet = false;

        public override void updateMagicEffectIcons()
        {
            MagicEffectIcons m = new MagicEffectIcons();
            PartySpelled p = null;

            if (Party != null)
                p = new PartySpelled(this);

            List<AbnormalEffect> nulled = new List<AbnormalEffect>();
            foreach (AbnormalEffect ei in _effects)
            {
                if (ei != null)
                {
                    if (ei.active == 1)
                    {
                        int time = ei.getTime();
                        m.addIcon(ei.id, ei.lvl, time);

                        if (p != null)
                            p.addIcon(ei.id, ei.lvl, time);
                    }
                    else
                        nulled.Add(ei);
                }
            }

            lock (_effects)
                foreach (AbnormalEffect ei in nulled)
                    _effects.Remove(ei);

            nulled.Clear();
            sendPacket(m);

            if (p != null && Party != null)
                Party.broadcastToMembers(p);
        }

        public void onGameInit()
        {
            CStatsInit();
            if (!insrestored)
                CharacterStat.setTemplate(ActiveClass);

            ExpAfterLogin = 0;
        }

        public List<QuestInfo> _quests = new List<QuestInfo>();

        public void quest_Talk(L2Citizen npc, int questId)
        {
            foreach (QuestInfo qi in _quests)
            {
                if (qi.completed)
                    continue;

                qi._template.onTalkToNpc(this, npc, qi.stage);
            }
        }

        public bool hasAllOfThisItems(params int[] id)
        {
            return Inventory.hasAllOfThis(id);
        }

        public bool hasSomeOfThisItems(params int[] id)
        {
            return Inventory.hasSomeOfThis(id);
        }

        public bool hasItem(int id, long count = 1)
        {
            return Inventory.getItemCount(id) >= count;
        }

        public void addItem(int id, long count)
        {
            Inventory.addItem(id, count, true, true);
        }

        public void addItemQuest(int id, long count)
        {
            sendPacket(new PlaySound("ItemSound.quest_itemget"));
            Inventory.addItem(id, count, true, true);
        }

        public void addItem(int id)
        {
            Inventory.addItem(id, 1, true, true);
        }

        public void ShowHtm(string file, L2Object o)
        {
            if (file.EndsWith(".htm"))
            {
                sendPacket(new NpcHtmlMessage(this, file, o.ObjID, 0));
                if (o is L2Citizen)
                    this.FolkNpc = (L2Citizen)o;
            }
            else
                this.ShowHtmPlain(file, o);
        }

        public void ShowHtm(string file, L2Citizen npc, int questId)
        {
            if (file.EndsWith(".htm"))
            {
                NpcHtmlMessage htm = new NpcHtmlMessage(this, file, npc.ObjID, 0);
                htm.replace("<?quest_id?>", questId);
                sendPacket(htm);
                FolkNpc = npc;
            }
            else
                ShowHtmPlain(file, npc);
        }

        public bool questComplete(int questId)
        {
            foreach (QuestInfo qi in _quests)
            {
                if (qi.id == questId)
                    return qi.completed;
            }

            return false;
        }

        public bool questInProgress(int questId)
        {
            foreach (QuestInfo qi in _quests)
            {
                if (qi.id == questId && !qi.completed)
                    return true;
            }

            return false;
        }

        public void questAccept(QuestInfo qi)
        {
            _quests.Add(qi);
            sendPacket(new PlaySound("ItemSound.quest_accept"));
            sendQuestList();

            //SQL_Block sqb = new SQL_Block("user_quests");
            //sqb.param("ownerId", ObjID);
            //sqb.param("qid", qi.id);
            //sqb.sql_insert(false);
        }

        public void ShowHtmPlain(string plain, L2Object o)
        {
            sendPacket(new NpcHtmlMessage(this, plain, o == null ? -1 : o.ObjID, true));
            if (o is L2Citizen)
                this.FolkNpc = (L2Citizen)o;
        }

        public void takeItem(int id, long count)
        {
            if (count > 1)
                Inventory.destroyItem(id, count, true, true);
            else
                Inventory.destroyItemAll(id, true, true);
        }

        public void changeQuestStage(int questId, int stage)
        {
            foreach (QuestInfo qi in _quests)
            {
                if (qi.id == questId)
                {
                    qi.stage = stage;
                    sendPacket(new PlaySound("ItemSound.quest_middle"));

                    //SQL_Block sqb = new SQL_Block("user_quests");
                    //sqb.param("qstage", stage);
                    //sqb.where("ownerId", ObjID);
                    //sqb.where("qid", qi.id);
                    //sqb.sql_update(false);
                    break;
                }
            }

            sendQuestList();
        }

        public List<QuestInfo> getAllActiveQuests()
        {
            List<QuestInfo> qu = new List<QuestInfo>();
            foreach (QuestInfo qi in _quests)
            {
                if (qi.completed)
                {
                    continue;
                }
                qu.Add(qi);
            }

            return qu;
        }

        public void sendQuestList()
        {
            sendPacket(new QuestList(this));
        }

        public void addExpSp(int exp, int sp, bool msg)
        {
            //You have earned $s1 experience and $s2 SP.
            SystemMessage sm = new SystemMessage(95);
            sm.AddNumber(exp);
            sm.AddNumber(sp);
            sendPacket(sm);

            Exp += exp;
            SP += sp;

            StatusUpdate su = new StatusUpdate(ObjID);
            su.add(StatusUpdate.EXP, (int)Exp);
            su.add(StatusUpdate.SP, SP);
            sendPacket(su);
        }

        public void finishQuest(int questId)
        {
            foreach (QuestInfo qi in _quests)
            {
                if (qi.id == questId)
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
                            _quests.Remove(qi);
                    }

                    sendPacket(new PlaySound("ItemSound.quest_finish"));
                    break;
                }
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

        public override void addSkill(TSkill newsk, bool updDb, bool update)
        {
            base.addSkill(newsk, updDb, update);

            if (update)
                updateSkillList();

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

        public override void removeSkill(int id, bool updDb, bool update)
        {
            base.removeSkill(id, updDb, update);

            if (update)
                updateSkillList();

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
            sendPacket(new PlaySound("ItemSound.quest_giveup"));
        }

        public InvPrivateWarehouse _warehouse;
        public void db_restoreQuests()
        {
            //MySqlConnection connection = SQLjec.getInstance().conn();
            //MySqlCommand cmd = connection.CreateCommand();

            //connection.Open();

            //cmd.CommandText = "SELECT qid,qstage,qfin FROM user_quests WHERE ownerId=" + ObjID + " ORDER BY tact ASC";
            //cmd.CommandType = CommandType.Text;

            //MySqlDataReader reader = cmd.ExecuteReader();

            //while (reader.Read())
            //{
            //    int qid = reader.GetInt32("qid");
            //    int stage = reader.GetInt32("qstage");
            //    int fin = reader.GetInt32("qfin");

            //    QuestInfo qi = new QuestInfo(qid, stage, fin);
            //    _quests.Add(qi);
            //}

            //reader.Close();
            //connection.Close();
        }

        public L2Item[] getAllNonQuestItems()
        {
            var sort = from item in Inventory.Items.Values where item.Template.Type != ItemTemplate.L2ItemType.questitem select item;
            return sort.ToArray();
        }

        public L2Item[] getAllWeaponArmorNonQuestItems()
        {
            var sort = from item in Inventory.Items.Values where item.Template.Type != ItemTemplate.L2ItemType.questitem && (item.Template.Type == ItemTemplate.L2ItemType.armor || item.Template.Type == ItemTemplate.L2ItemType.weapon) select item;
            return sort.ToArray();
        }

        public L2Item[] getAllQuestItems()
        {
            var sort = from item in Inventory.Items.Values where item.Template.Type == ItemTemplate.L2ItemType.questitem select item;
            return sort.ToArray();
        }

        public L2Item[] getAllItems()
        {
            return Inventory.Items.Values.ToArray();
        }

        public ICollection getAllWarehouseItems()
        {
            return _warehouse.Items;
        }

        public L2Clan Clan;

        public int
            ItemLimit_Inventory = 80,
            ItemLimit_Selling = 5,
            ItemLimit_Buying = 5,
            ItemLimit_RecipeDwarven = 50,
            ItemLimit_RecipeCommon = 50,
            ItemLimit_Warehouse = 120,
            ItemLimit_ClanWarehouse = 150,
            ItemLimit_Extra = 0,
            ItemLimit_Quest = 20;

        public L2Citizen FolkNpc;
        public int last_x1 = -4;
        public int last_y1;

        public void reduceAdena(long count, bool msg, bool upd)
        {
            Inventory.destroyItem(57, count, msg, upd);
        }

        public override void updateSkillList()
        {
            sendPacket(new SkillList(this, _p_block_act, _p_block_spell, _p_block_skill));
        }

        System.Timers.Timer _timerTooFar;
        public string _locale = "en";

        public void timer()
        {
            _timerTooFar = new System.Timers.Timer(30 * 1000);
            _timerTooFar.Elapsed += new ElapsedEventHandler(_timeToFarTimerTask);
            _timerTooFar.Interval = 10000;
            _timerTooFar.Enabled = true;
        }

        public void _timeToFarTimerTask(object sender, ElapsedEventArgs e)
        {
            validateVisibleObjects(X, Y, true);
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
                foreach (L2Recipe r in _recipeBook)
                {
                    if (r.RecipeID == rec.RecipeID)
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

                        sendPacket(new RecipeBookItemList(this, rec._iscommonrecipe));
                        break;
                    }
                }
            }
        }

        public bool isAlikeDead()
        {
            return false;
        }

        public int getClanCrestLargeId()
        {
            if (Clan == null)
                return 0;

            return Clan.LargeCrestID;
        }

        public List<L2Shortcut> _shortcuts = new List<L2Shortcut>();
        public int zoneId = -1;
        public int _obsx = -1;
        public int _obsy;
        public int _obsz;

        public void registerShortcut(int _slot, int _page, int _type, int _id, int _level, int _characterType)
        {
            lock (_shortcuts)
            {
                foreach (L2Shortcut sc in _shortcuts)
                {
                    if (sc.Slot == _slot && sc.Page == _page)
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
            }

            {
                L2Shortcut sc = new L2Shortcut(_slot, _page, _type, _id, _level, _characterType);
                _shortcuts.Add(sc);

                sendPacket(new ShortCutRegister(sc));

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

        public int Souls = 0;
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
            foreach (QuestInfo qi in _quests)
            {
                if (qi.id == p)
                {
                    return qi.completed;
                }
            }

            return false;
        }

        public int getQuestCond(int questId)
        {
            foreach (QuestInfo qi in _quests)
            {
                if (qi.id == questId)
                {
                    return qi.stage;
                }
            }

            return 0;
        }

        public override void onPickUp(L2Item item)
        {
            item.Location = L2Item.L2ItemLocation.inventory;
            Inventory.addItem(item, true, true);
        }

        public override string asString()
        {
            return "L2Player:" + Name;
        }

        public override void onRemObject(L2Object obj)
        {
            sendPacket(new DeleteObject(obj.ObjID));
        }

        public override void onAddObject(L2Object obj, GameServerNetworkPacket pk, string msg = null)
        {
            if (obj is L2Citizen)
            {
                sendPacket(new NpcInfo((L2Citizen)obj));
            }
            else if (obj is L2Player)
            {
                sendPacket(new CharInfo((L2Player)obj));

                if (msg != null)
                    ((L2Player)obj).sendMessage(msg);
            }
            else if (obj is L2Item)
            {
                sendPacket(pk == null ? new SpawnItem((L2Item)obj) : pk);
            }
            else if (obj is L2Summon)
            {
                sendPacket(pk == null ? new PetInfo((L2Summon)obj) : pk);
            }
            else if (obj is L2Chair)
            {
                sendPacket(new StaticObject((L2Chair)obj));
            }
            else if (obj is L2StaticObject)
            {
                sendPacket(new StaticObject((L2StaticObject)obj));
            }
            else if (obj is L2Boat)
            {
                sendPacket(new VehicleInfo((L2Boat)obj));
            }
        }

        public void untransform()
        {
            if (Transform != null)
            {
                Transform.Template.onTransformEnd(this);
                Transform = null;
            }
        }

        public void setTransform(L2Transform tr)
        {
            Transform = tr;
            Transform.owner = this;
            Transform.Template.onTransformStart(this);
        }

        public SortedList<int, db_InstanceReuse> InstanceReuse = new SortedList<int, db_InstanceReuse>();
        bool insrestored = false;
        public int ViewingAdminPage;
        public int ViewingAdminTeleportGroup = -1;
        public int TeleportPayID;
        public int LastMinigameScore;
        public short ClanType;
        public int Fame;


        public void db_restoreInstanceReuse()
        {
            if (insrestored)
                return;

            //MySqlConnection connection = SQLjec.getInstance().conn();
            //MySqlCommand cmd = connection.CreateCommand();

            //connection.Open();

            //cmd.CommandText = "SELECT * FROM user_instances WHERE ownerId=" + ObjID + "";
            //cmd.CommandType = CommandType.Text;

            //MySqlDataReader reader = cmd.ExecuteReader();

            //while (reader.Read())
            //{
            //    string name = reader.GetString("name");
            //    int id = reader.GetInt32("instanceId");
            //    string time = reader.GetString("disabledTo");

            //    DateTime dt = DateTime.Parse(time);
            //    if (dt.CompareTo(DateTime.Now) == 1)
            //    {
            //        db_InstanceReuse db = new db_InstanceReuse();
            //        db.dt = dt;
            //        db.id = id;
            //        db.name = name;
            //        InstanceReuse.Add(id, db);
            //    }
            //}

            //reader.Close();
            //connection.Close();
            insrestored = true;
        }

        public void ShowHtmAdmin(string val, bool plain)
        {
            if (plain)
                sendPacket(new TutorialShowHtml(this, val, true, true));
            else
                sendPacket(new TutorialShowHtml(this, val, true));

            ViewingAdminPage = 1;
        }

        public void ShowHtmBBS(string val)
        {
            ShowBoard.separateAndSend(val, this);
        }

        public void sendItemList(bool open = false)
        {
            sendPacket(new ItemList(this, open));
            sendPacket(new ExQuestItemList(this));
        }

        public void updateWeight()
        {
            long oldweight = CurrentWeight;
            long total = 0;
            if (!_diet)
            {
                foreach (L2Item it in Inventory.Items.Values)
                {
                    if (it.Template.Weight == 0)
                        continue;

                    if (it.Template.isStackable())
                    {
                        total += it.Template.Weight * it.Count;
                    }
                    else
                        total += it.Template.Weight;
                }
            }

            CurrentWeight = total >= int.MaxValue ? int.MaxValue : (int)total;

            if (oldweight != total)
            {
                StatusUpdate su = new StatusUpdate(ObjID);
                su.add(StatusUpdate.CUR_LOAD, CurrentWeight);
                sendPacket(su);

                long weightproc = total * 1000 / (int)CharacterStat.getStat(TEffectType.b_max_weight);

                int newWeightPenalty;
                if (weightproc < 500)
                {
                    newWeightPenalty = 0;
                }
                else if (weightproc < 666)
                {
                    newWeightPenalty = 1;
                }
                else if (weightproc < 800)
                {
                    newWeightPenalty = 2;
                }
                else if (weightproc < 1000)
                {
                    newWeightPenalty = 3;
                }
                else
                {
                    newWeightPenalty = 4;
                }

                if (_penaltyWeight != newWeightPenalty)
                {
                    if (newWeightPenalty > 0)
                    {
                        addSkill(4270, newWeightPenalty, false, true);
                    }
                    else
                    {
                        removeSkill(4270, false, true);
                    }

                    _penaltyWeight = newWeightPenalty;

                    sendPacket(new EtcStatusUpdate(this));
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

        public bool CheckFreeSlotsInventory(ItemTemplate item, int count)
        {
            int check = 0;

            if (item.isStackable())
            {
                if (Inventory.getItemById(item.ItemID) == null)
                    check = 1;
            }
            else
                check = count;

            if (getAllNonQuestItems().Length + check >= ItemLimit_Inventory)
                return false;

            return true;
        }

        public bool CheckFreeSlotsInventory80(ItemTemplate item, long count)
        {
            long check = 0;

            if (item.isStackable())
            {
                if (Inventory.getItemById(item.ItemID) == null)
                    check = 1;
            }
            else
                check = count;

            if (getAllNonQuestItems().Length + check >= (ItemLimit_Inventory * .8))
                return false;

            return true;
        }

        public bool CheckFreeSlotsInventory80(int id, long count, bool msg)
        {
            long check = 0;
            L2Item item = Inventory.getItemById(id);

            if (item != null)
            {
                if (!item.Template.isStackable())
                    check = 1;
                else
                    check = count;
            }
            else
                check = 1;

            if (getAllNonQuestItems().Length + check >= (ItemLimit_Inventory * .8))
            {
                sendSystemMessage(2981); //You could not receive because your inventory is full.
                return false;
            }

            return true;
        }

        public byte ClanRank()
        {
            if (ClanId == 0 || Clan == null)
                return 0;

            bool leader = Clan.LeaderID == ObjID;
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
                                else if (Clan.isSubLeader(ObjID, new e_ClanType[] { e_ClanType.CLAN_KNIGHT1, e_ClanType.CLAN_KNIGHT2 }) != e_ClanType.None)
                                    rank = e_ClanRank._4;
                                else
                                    rank = e_ClanRank._3;
                                break;
                            case 7:
                                if (leader)
                                    rank = e_ClanRank._7;
                                else if (Clan.isSubLeader(ObjID, new e_ClanType[] { e_ClanType.CLAN_KNIGHT1, e_ClanType.CLAN_KNIGHT2 }) != e_ClanType.None)
                                    rank = e_ClanRank._6;
                                else if (Clan.isSubLeader(ObjID, new e_ClanType[] { e_ClanType.CLAN_KNIGHT3, e_ClanType.CLAN_KNIGHT4, e_ClanType.CLAN_KNIGHT5, e_ClanType.CLAN_KNIGHT6 }) != e_ClanType.None)
                                    rank = e_ClanRank._5;
                                else
                                    rank = e_ClanRank._4;
                                break;
                            case 8:
                                if (leader)
                                    rank = e_ClanRank._8;
                                else if (Clan.isSubLeader(ObjID, new e_ClanType[] { e_ClanType.CLAN_KNIGHT1, e_ClanType.CLAN_KNIGHT2 }) != e_ClanType.None)
                                    rank = e_ClanRank._7;
                                else if (Clan.isSubLeader(ObjID, new e_ClanType[] { e_ClanType.CLAN_KNIGHT3, e_ClanType.CLAN_KNIGHT4, e_ClanType.CLAN_KNIGHT5, e_ClanType.CLAN_KNIGHT6 }) != e_ClanType.None)
                                    rank = e_ClanRank._6;
                                else
                                    rank = e_ClanRank._5;
                                break;
                            case 9:
                                if (leader)
                                    rank = e_ClanRank._9;
                                else if (Clan.isSubLeader(ObjID, new e_ClanType[] { e_ClanType.CLAN_KNIGHT1, e_ClanType.CLAN_KNIGHT2 }) != e_ClanType.None)
                                    rank = e_ClanRank._8;
                                else if (Clan.isSubLeader(ObjID, new e_ClanType[] { e_ClanType.CLAN_KNIGHT3, e_ClanType.CLAN_KNIGHT4, e_ClanType.CLAN_KNIGHT5, e_ClanType.CLAN_KNIGHT6 }) != e_ClanType.None)
                                    rank = e_ClanRank._7;
                                else
                                    rank = e_ClanRank._6;
                                break;
                            case 10:
                                if (leader)
                                    rank = e_ClanRank._10;
                                else if (Clan.isSubLeader(ObjID, new e_ClanType[] { e_ClanType.CLAN_KNIGHT1, e_ClanType.CLAN_KNIGHT2 }) != e_ClanType.None)
                                    rank = e_ClanRank._9;
                                else if (Clan.isSubLeader(ObjID, new e_ClanType[] { e_ClanType.CLAN_KNIGHT3, e_ClanType.CLAN_KNIGHT4, e_ClanType.CLAN_KNIGHT5, e_ClanType.CLAN_KNIGHT6 }) != e_ClanType.None)
                                    rank = e_ClanRank._8;
                                else
                                    rank = e_ClanRank._7;
                                break;
                            case 11:
                                if (leader)
                                    rank = e_ClanRank._11;
                                else if (Clan.isSubLeader(ObjID, new e_ClanType[] { e_ClanType.CLAN_KNIGHT1, e_ClanType.CLAN_KNIGHT2 }) != e_ClanType.None)
                                    rank = e_ClanRank._10;
                                else if (Clan.isSubLeader(ObjID, new e_ClanType[] { e_ClanType.CLAN_KNIGHT3, e_ClanType.CLAN_KNIGHT4, e_ClanType.CLAN_KNIGHT5, e_ClanType.CLAN_KNIGHT6 }) != e_ClanType.None)
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

            if (Noblesse == 1 && (byte)rank < 5)
                rank = e_ClanRank._5;

            if (Heroic == 1 && (byte)rank < 8)
                rank = e_ClanRank._8;

            return (byte)rank;
        }

        public override void updateAbnormalEventEffect()
        {
            broadcastPacket(new ExBrExtraUserInfo(ObjID, AbnormalBitMaskEvent));
        }

        public override void updateAbnormalExEffect()
        {
            broadcastUserInfo();
        }

        public string Penalty_ClanCreate = "0";
        public string Penalty_ClanJoin = "0";

        public void setPenalty_ClanCreate(DateTime time, bool sql)
        {
            if (DateTime.Now < time)
                Penalty_ClanCreate = time.ToString("yyyy-MM-dd HH-mm-ss");
            else
                Penalty_ClanCreate = "0";

            if (sql)
            {
               
            }
        }

        public void setPenalty_ClanJoin(DateTime time, bool sql)
        {
            if (DateTime.Now < time)
                Penalty_ClanJoin = time.ToString("yyyy-MM-dd HH-mm-ss");
            else
                Penalty_ClanJoin = "0";

            if (sql)
            {
               
            }
        }

        public bool IsRestored = false;
        public void TotalRestore()
        {
            if (IsRestored)
                return;

            onGameInit();
            db_restoreSkills();
            db_restoreQuests();
            db_restoreRecipes();
            db_restoreInstanceReuse();
            db_restoreTelbooks();
           // db_restoreShortcuts(); elfo to be added

            IsRestored = true;
        }

        public TeleportBook Telbook;
        public byte TelbookLimit = 0, TelbookLimitMax = 25;
        public bool IsFlying;

        public void db_restoreTelbooks()
        {
            if (TelbookLimit == 0)
                return;

            //MySqlConnection connection = SQLjec.getInstance().conn();
            //MySqlCommand cmd = connection.CreateCommand();

            //connection.Open();

            //cmd.CommandText = "SELECT * FROM user_telbooks WHERE ownerId=" + ObjID;
            //cmd.CommandType = CommandType.Text;

            //MySqlDataReader reader = cmd.ExecuteReader();

            //Telbook = new TeleportBook();
            //while (reader.Read())
            //{
            //    TelBook_Mark mark = new TelBook_Mark();
            //    mark.id = (byte)reader.GetInt16("id");
            //    mark.x = reader.GetInt32("locx");
            //    mark.y = reader.GetInt32("locy");
            //    mark.z = reader.GetInt32("locz");
            //    mark.icon = reader.GetInt32("icon");
            //    mark.name = reader.GetString("name");
            //    mark.tag = reader.GetString("tag");

            //    Telbook.bookmarks.Add(mark.id, mark);
            //}

            //reader.Close();
            //connection.Close();
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
            sendPacket(new EtcStatusUpdate(this));
        }

        public void AddSouls(byte count)
        {
            Souls += count;
            sendPacket(new EtcStatusUpdate(this));

            SystemMessage sm = new SystemMessage(2162);
            sm.AddNumber(count);
            sm.AddNumber(Souls);
            sendPacket(sm);//Your soul count has increased by $s1. It is now at $s2.
        }

        public void IncreaseSouls()
        {
            byte soul = 1;

            if (Souls + soul > 45 || Souls == 45)
            {
                sendSystemMessage(2163); //Soul cannot be increased anymore.
                return;
            }

            AddSouls(soul);
        }

        public bool IsCursed = false;

        public byte PartyState = 0;
        public L2Player requester = null;
        public int itemDistribution;

        public void PendToJoinParty(L2Player asker, int itemDistribution)
        {
            PartyState = 1;
            requester = asker;
            requester.itemDistribution = itemDistribution;
            sendPacket(new AskJoinParty(asker.Name, itemDistribution));
        }

        public void ClearPend()
        {
            PartyState = 0;
            requester = null;
        }

        public L2Summon Summon;
        public bool IsInOlympiad = false;
        public L2Item EnchantScroll, EnchantItem, EnchantStone;
        public byte EnchantState = 0;

        // 0 cls, 1 violet, 2 blink
        public byte PvPStatus;

        public byte GetEnchantValue()
        {
            int val = Inventory.getWeaponEnchanment();

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
                if (!(((L2Summon)target).Owner != null && ((L2Summon)target).Owner.ObjID == ObjID))
                    color = ((L2Summon)target).Level - Level;
            }

            sendPacket(new MyTargetSelected(target.ObjID, color));
        }

        public override void OnOldTargetSelection(L2Object target)
        {
            double dis = Calcs.calculateDistance(this, target, true);
            if (dis < 151)
                target.NotifyAction(this);
            else
                tryMoveTo(target.X, target.Y, target.Z);

            sendActionFailed();
        }

        private System.Timers.Timer petSummonTime, nonpetSummonTime;
        private int PetID = -1;
        private L2Item PetControlItem = null;

        public void PetSummon(L2Item item, int NpcID, bool isPet = true)
        {
            if (Summon != null)
            {
                sendSystemMessage(543);//You already have a pet.
                return;
            }

            if (isPet)
            {
                if (petSummonTime == null)
                {
                    petSummonTime = new System.Timers.Timer();
                    petSummonTime.Interval = 5000;
                    petSummonTime.Elapsed += new ElapsedEventHandler(PetSummonEnd);
                }

                petSummonTime.Enabled = true;
                sendSystemMessage(547);//Summoning your pet…
            }
            else
            {
                if (nonpetSummonTime == null)
                {
                    nonpetSummonTime = new System.Timers.Timer();
                    nonpetSummonTime.Interval = 5000;
                    nonpetSummonTime.Elapsed += new ElapsedEventHandler(NonpetSummonEnd);
                }

                nonpetSummonTime.Enabled = true;
            }

            this.PetID = NpcID;
            this.PetControlItem = item;

            broadcastPacket(new MagicSkillUse(this, this, 1111, 1, 5000));
            sendPacket(new SetupGauge(ObjID, SetupGauge.SG_color.blue, 4900));
        }

        private void PetSummonEnd(object sender, ElapsedEventArgs e)
        {
            L2Pet pet = new L2Pet();
            pet.setTemplate(NpcTable.Instance.getNpcTemplate(PetID));
            pet.setOwner(this);
            pet.ControlItem = PetControlItem;
           // pet.sql_restore();
            pet.SpawmMe();

            petSummonTime.Enabled = false;
        }

        private void NonpetSummonEnd(object sender, ElapsedEventArgs e)
        {
            L2Summon summon = new L2Summon();
            summon.setTemplate(NpcTable.Instance.getNpcTemplate(PetID));
            summon.setOwner(this);
            summon.ControlItem = PetControlItem;
            summon.SpawmMe();

            nonpetSummonTime.Enabled = false;
        }

        public override bool cantMove()
        {
            if (petSummonTime != null && petSummonTime.Enabled)
                return true;
            if (nonpetSummonTime != null && nonpetSummonTime.Enabled)
                return true;

            return base.cantMove();
        }

        public override L2Character[] getPartyCharacters()
        {
            List<L2Character> chars = new List<L2Character>();
            chars.Add(this);
            if (Summon != null)
                chars.Add(Summon);

            if (Party != null)
            {
                if (chars == null)
                    chars = new List<L2Character>();

                foreach (L2Player pl in Party.Members)
                {
                    if (pl.ObjID == this.ObjID)
                        continue;

                    chars.Add(pl);

                    if (pl.Summon != null)
                        chars.Add(pl.Summon);
                }
            }

            return chars.ToArray();
        }

        public override void abortCast()
        {
            if (petSummonTime != null && petSummonTime.Enabled)
                petSummonTime.Enabled = false;

            if (nonpetSummonTime != null && nonpetSummonTime.Enabled)
                nonpetSummonTime.Enabled = false;

            base.abortCast();
        }

        public override void StartAI()
        {
            AICharacter = new PlayerAI(this);
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

        private System.Timers.Timer sitTime;
        private bool IsSitting = false;
        public void Sit()
        {
            if (sitTime == null)
            {
                sitTime = new System.Timers.Timer();
                sitTime.Interval = 2500;
                sitTime.Elapsed += new ElapsedEventHandler(SitEnd);
            }

            sitTime.Enabled = true;
            broadcastPacket(new ChangeWaitType(this, ChangeWaitType.SIT));
        }

        public void Stand()
        {
            sitTime.Enabled = true;
            broadcastPacket(new ChangeWaitType(this, ChangeWaitType.STAND));
            //TODO stop relax effect
        }

        private void SitEnd(object sender, ElapsedEventArgs e)
        {
            sitTime.Enabled = false;
            IsSitting = !IsSitting;

            if (!IsSitting && chair != null)
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

        public void SetChair(L2Chair chair)
        {
            this.chair = chair;
            this.chair.IsUsedAlready = true;
            broadcastPacket(new ChairSit(ObjID, chair.StaticID));
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

        public override L2Item ActiveWeapon
        {
            get { return Inventory.getWeapon(); }
            set { ActiveWeapon = value; }
        }

        public override void abortAttack()
        {
            base.abortAttack();
            AICharacter.StopAutoAttack();
        }

        public override void doAttack(L2Character target)
        {
            if (target == null)
            {
                sendMessage("null");
                AICharacter.StopAutoAttack();
                sendActionFailed();
                return;
            }

            if (target.Dead)
            {
                sendMessage("dead");
                AICharacter.StopAutoAttack();
                sendActionFailed();
                return;
            }

            if (attack_ToHit != null && attack_ToHit.Enabled)
            {
                sendActionFailed();
                return;
            }

            if (attack_ToEnd != null && attack_ToEnd.Enabled)
            {
                sendActionFailed();
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

                    if (SecondaryWeaponSupport == null || SecondaryWeaponSupport.Count < weapon.Template.SoulshotCount)
                    {
                        //Not enough bolts, You have run out of arrows.
                        sendSystemMessage(weapon.Template.WeaponType == ItemTemplate.L2ItemWeaponType.bow ? 112 : 2226);
                        sendActionFailed();
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
                sendMessage("too far " + dist);
                tryMoveTo(target.X, target.Y, target.Z);
                return;
            }

            if (reqMp > 0 && reqMp > CurMP)
            {
                sendMessage("no mp " + CurMP + " " + reqMp);
                sendActionFailed();
                return;
            }

            if (ranged)
            {
                this.sendPacket(new SetupGauge(ObjID, SetupGauge.SG_color.red, (int)timeAtk));
                Inventory.destroyItem(SecondaryWeaponSupport, 1, false, true);
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

        public override void AttackDoHit(object sender, ElapsedEventArgs e)
        {
            if (CurrentTarget != null && !CurrentTarget.Dead)
            {
                if (!hit1.miss)
                {
                    if (hit1.crit)
                        sendPacket(new SystemMessage(2266).AddPlayerName(Name));//$c1 landed a critical hit!

                    //$c1 has given $c2 damage of $s3.
                    sendPacket(new SystemMessage(2261).AddPlayerName(Name).AddName(CurrentTarget).AddNumber(hit1.damage));
                    CurrentTarget.reduceHp(this, hit1.damage);

                    if (CurrentTarget is L2Player) //$c1 has received $s3 damage from $c2.
                        CurrentTarget.sendPacket(new SystemMessage(2262).AddName(CurrentTarget).AddName(this).AddNumber(hit1.damage));
                }
                else
                {
                    sendPacket(new SystemMessage(2265).AddPlayerName(Name));//$c1's attack went astray.

                    if (CurrentTarget is L2Player) //$c1 has evaded $c2's attack.
                    {
                        CurrentTarget.sendPacket(new SystemMessage(2264).AddName(CurrentTarget).AddName(this));
                        ((L2Player)CurrentTarget).AICharacter.NotifyEvaded(this);
                    }
                }
            }

            attack_ToHit.Enabled = false;
        }

        public override void AttackDoHit2nd(object sender, ElapsedEventArgs e)
        {
            if (CurrentTarget != null && !CurrentTarget.Dead)
            {
                if (!hit2.miss)
                {
                    if (hit2.crit)
                        sendPacket(new SystemMessage(2266).AddName(this));//$c1 landed a critical hit!

                    //$c1 has given $c2 damage of $s3.
                    sendPacket(new SystemMessage(2261).AddName(this).AddName(CurrentTarget).AddNumber(hit2.damage));
                    CurrentTarget.reduceHp(this, hit2.damage);

                    if (CurrentTarget is L2Player) //$c1 has received $s3 damage from $c2.
                        CurrentTarget.sendPacket(new SystemMessage(2262).AddName(CurrentTarget).AddName(this).AddNumber(hit2.damage));
                }
                else
                {
                    sendPacket(new SystemMessage(2265).AddPlayerName(Name));//$c1's attack went astray.

                    if (CurrentTarget is L2Player) //$c1 has evaded $c2's attack.
                    {
                        CurrentTarget.sendPacket(new SystemMessage(2264).AddName(CurrentTarget).AddName(this));
                        ((L2Player)CurrentTarget).AICharacter.NotifyEvaded(this);
                    }
                }
            }

            attack_toHitBonus.Enabled = false;
        }

        public override void AttackDoEnd(object sender, ElapsedEventArgs e)
        {
            attack_ToEnd.Enabled = false;

            L2Item weapon = Inventory.getWeapon();
            if (weapon != null)
            {
                if (weapon.Soulshot)
                    weapon.Soulshot = false;

                foreach (int sid in weapon.Template.getSoulshots())
                    if (autoSoulshots.Contains(sid))
                    {
                        if (Inventory.getItemCount(sid) < weapon.Template.SoulshotCount)
                        {
                            sendPacket(new SystemMessage(1435).AddItemName(sid));//Due to insufficient $s1, the automatic use function has been deactivated.

                            lock (autoSoulshots)
                            {
                                autoSoulshots.Remove(sid);
                                sendPacket(new ExAutoSoulShot(sid, 0));
                            }
                        }
                        else
                        {
                            Inventory.destroyItem(sid, weapon.Template.SoulshotCount, false, true);
                            weapon.Soulshot = true;
                            broadcastSoulshotUse(sid);
                        }

                        break;
                    }
            }
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
            broadcastPacket(new Ride(this, true, npc.NpcId));
            MountedTemplate = npc;
            broadcastUserInfo();
        }

        public void MountPet()
        {
            if (Summon != null)
                Mount(Summon.Template);
        }

        public void unMount()
        {
            broadcastPacket(new Ride(this, false));
            MountedTemplate = null;
            broadcastUserInfo();
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
            else
                currentTrade.Add(objId, num);

            return num;
        }

        public void NotifyDayChange(GameServerNetworkPacket pk)
        {
            sendPacket(pk);
            if (pk is SunSet) //включаем ночные скилы
                AICharacter.NotifyOnStartNight();
            else
                AICharacter.NotifyOnStartDay();

        }

        public int VehicleId
        {
            get
            {
                if (Boat != null)
                    return Boat.ObjID;

                return 0;
            }
        }

        public void Revive(double percent)
        {
            broadcastPacket(new Revive(ObjID));
            Dead = false;
            StartRegeneration();
        }

        private DateTime pingTimeout;
        private int lastPingId;
        public int Ping = -1;
        public L2dotNET.Game.tables.multisell.MultiSellList CustomMultiSellList;
        public int LastRequestedMultiSellId = -1;
        public int AttackingId;
        public SortedList<int, TAcquireSkill> ActiveSkillTree;
        public void RequestPing()
        {
            lastPingId = new Random().Next(int.MaxValue);
            NetPing ping = new NetPing(lastPingId);
            pingTimeout = DateTime.Now;
            sendPacket(ping);
        }

        public void UpdatePing(int id, int ms, int unk)
        {
            if (lastPingId != id)
            {
                log.Warn($"player fail to ping respond right { id } { lastPingId } at { pingTimeout.ToLocalTime() }");
                return;
            }

            Ping = ms;
            sendMessage("Your connection latency is " + ms);
        }

        public void InstantTeleportWithItem(int x, int y, int z, int id, long cnt)
        {
            Inventory.destroyItem(id, cnt, true, true);
            teleport(x, y, z);
        }

        public void RedistExp(L2Warrior mob)
        {
            double expPet = 0.0;
            if (Summon != null && Summon.ConsumeExp > 0)
                expPet = Summon.ConsumeExp / 100.0 + 1;

            double expReward = mob.Template.exp / (Level * mob.Template.acquire_exp_rate < 1.0 ? 1.0 : mob.Template.acquire_exp_rate);
            int sp = mob.Template.acquire_sp;
            int rp = mob.Template.acquire_rp;
            sendMessage("debug: expPet " + expPet);
            sendMessage("debug: mob.Template " + mob.Template.exp + " @" + mob.Template.acquire_exp_rate);
            sendMessage("debug: expReward " + expReward);
            sendMessage("debug: sp " + sp);
            sendMessage("debug: rp " + rp);

            byte oldLvl = Level;
            Exp += (long)expReward;
            byte newLvl = Experience.getLevel(Exp);
            bool lvlChanged = oldLvl != newLvl;

            Exp += (long)expReward;
            if (lvlChanged)
            {
                Level = newLvl;
                broadcastPacket(new SocialAction(ObjID, 2122));
            }

            if (!lvlChanged)
            {
                sendPacket(new UserInfo(this));
            }
            else
                broadcastUserInfo();
        }

        public byte ClanLevel
        {
            get
            {
                if (Clan == null)
                    return 0;

                return Clan.Level;
            }
        }

        public void broadcastSkillUse(int skillId)
        {
            TSkill skill = TSkillTable.Instance.Get(skillId);
            broadcastPacket(new MagicSkillUse(this, this, skill.skill_id, skill.level, skill.skill_hit_time));
        }

        public bool ClanLeader
        {
            get
            {
                if (Clan == null)
                    return false;
                else
                    return Clan.LeaderID == ObjID;
            }
        }

        public bool HavePledgePower(int bit)
        {
            if (Clan == null)
                return false;

            return Clan.hasRights(this, bit);
        }

        public override L2Item getWeaponItem()
        {
            return this.Inventory.getWeapon();
        }

        public void UpdateAgathionEnergy(int count)
        {
            sendMessage("@UpdateAgathionEnergy "+count);
        }

        public List<Cubic> cubics = new List<Cubic>();
        public void StopCubic(Cubic cubic)
        {
            foreach(Cubic cub in cubics)
                if (cub.template.id == cubic.template.id)
                {
                    lock (cubics)
                        cubics.Remove(cub);

                    this.broadcastUserInfo();
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
                lock(cubics)
                    cubics.RemoveAt(0);
            }

            foreach(Cubic cub in cubics)
                if (cub.template.id == cubic.template.id)
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
            if(update)
                this.broadcastUserInfo();
        }

        public override void doDie(L2Character killer, bool bytrigger)
        {
            if (cubics.Count > 0)
            {
                foreach (Cubic cub in cubics)
                    cub.OnEnd(false);

                cubics.Clear();
            }

            base.doDie(killer, bytrigger);
        }
    }
}
