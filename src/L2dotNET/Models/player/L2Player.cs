using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Timers;
using log4net;
using L2dotNET.DataContracts;
using L2dotNET.Enums;
using L2dotNET.model.inventory;
using L2dotNET.model.items;
using L2dotNET.model.npcs;
using L2dotNET.model.npcs.decor;
using L2dotNET.model.player.General;
using L2dotNET.model.vehicles;
using L2dotNET.Models;
using L2dotNET.Models.Stats;
using L2dotNET.Models.Stats.Funcs;
using L2dotNET.Models.Status;
using L2dotNET.Network;
using L2dotNET.Network.serverpackets;
using L2dotNET.Services.Contracts;
using L2dotNET.tables;
using L2dotNET.tables.multisell;
using L2dotNET.templates;
using L2dotNET.tools;
using L2dotNET.Utility;
using L2dotNET.world;
using Ninject;

namespace L2dotNET.model.player
{
    [Synchronization]
    public class L2Player : L2Character
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(L2Player));

        public L2Player(int objectId, PcTemplate template) : base(objectId, template)
        {
            Template = template;
            Stats = new CharacterStat(this);
            InitializeCharacterStatus();
            Calculators = new Models.Stats.Calculator[Models.Stats.Stats.Values.Count()];
            AddFuncsToNewCharacter();
        }

        public L2Player() :base(0, null)
        {
        }

        [Inject]
        public IPlayerService PlayerService { get; set; } = GameServer.Kernel.Get<IPlayerService>();

        public new PlayerStatus Status => (PlayerStatus)base.Status;

        public string AccountName { get; set; }
        public ClassId ClassId { get; set; }
        public L2PartyRoom PartyRoom { get; set; }
        public L2Party Party { get; set; }
        public Gender Sex { get; set; }
        public HairStyleId HairStyleId { get; set; }
        public HairColor HairColor { get; set; }
        public Face Face { get; set; }
        public bool WhisperBlock { get; set; }
        public GameClient Gameclient { get; set; }
        public long Exp { get; set; }
        public long ExpOnDeath { get; set; }
        public long ExpAfterLogin { get; set; }
        public int Sp { get; set; }
        public override int MaxHp => Stats.MaxHp;
        public int MaxCp => Stats.MaxCp;
        public double CurCp
        {
            get => Status.CurrentCp;
            set => Status.SetCurrentCp(value);
        }
        public override int MaxMp => Stats.MaxMp;
        public int Karma { get; set; }
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
        public int CurrentWeight { get; set; }
        public double CollRadius { get; set; }
        public double CollHeight { get; set; }
        public int CursedWeaponLevel { get; set; }
        public long LastAccess { get; set; }
        public int IsIn7SDungeon { get; set; }
        public int PunishLevel { get; set; }
        public int PunishTimer { get; set; }
        public int PowerGrade { get; set; }
        public int Nobless { get; set; }
        public int Hero { get; set; }
        public long LastRecomDate { get; set; }
        public PcInventory Inventory { get; set; }
        public dynamic SessionData { get; set; }
        public new PcTemplate Template { get; set; }

        public byte Builder = 1;
        public byte Noblesse = 0;
        public byte Heroic = 0;

        public override void InitializeCharacterStatus()
        {
            base.Status = new PlayerStatus(this);
        }

        public byte PrivateStoreType = 0;

        public byte GetPrivateStoreType()
        {
            return PrivateStoreType;
        }

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

        public override void SendMessage(string p)
        {
            SendPacket(new SystemMessage(SystemMessage.SystemMessageId.S1).AddString(p));
        }

        public int CurrentFocusEnergy = 0;

        public int GetForceIncreased()
        {
            return CurrentFocusEnergy;
        }

        public void UpdateReuse()
        {
            SendPacket(new SkillCoolTime(this));
        }

        public override void AddFuncsToNewCharacter()
        {
            base.AddFuncsToNewCharacter();

            AddStatFunc(new FuncMaxCpMul());

            //Henna stuff go here
        }

        public bool Diet = false;
        
        public void OnGameInit()
        {
            //CStatsInit();
            //CharacterStat.SetTemplate(ActiveClass);
            ExpAfterLogin = 0;
        }

        public void ShowHtm(string file, L2Object o)
        {
            if (file.EndsWithIgnoreCase(".htm"))
            {
                SendPacket(new NpcHtmlMessage(this, $"./html/ {file}", o.ObjId, 0));
                L2Npc npc = o as L2Npc;
                if (npc != null)
                    FolkNpc = npc;
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
        
        public void UpdateAndBroadcastStatus(int broadcastType)
        {
            if (broadcastType == 1)
                SendPacket(new UserInfo(this));
            else if (broadcastType == 2)
            {
                BroadcastUserInfo();
            }
        }

        public void ShowHtmPlain(string plain, L2Object o)
        {
            SendPacket(new NpcHtmlMessage(this, plain, o?.ObjId ?? -1, true));
            if (o is L2Npc)
                FolkNpc = (L2Npc)o;
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
            //Need to Level up ?
            if(Level != Experience.GetLevel(Exp))
            {
                Level = Experience.GetLevel(Exp);
                this.BroadcastPacket(new SocialAction(ObjId, 15));
                this.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.YouIncreasedYourLevel));
            }

            StatusUpdate su = new StatusUpdate(this);
            su.Add(StatusUpdate.Exp, (int)Exp);
            su.Add(StatusUpdate.Sp, Sp);
            su.Add(StatusUpdate.Level, Level);
            SendPacket(su);
        }
        
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

        public PcTemplate BaseClass;
        public PcTemplate ActiveClass;

        public bool SubActive()
        {
            return ActiveClass != BaseClass;
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

        public override void BroadcastStatusUpdate()
        {
            StatusUpdate su = new StatusUpdate(this);
            su.Add(StatusUpdate.CurHp, (int)CurHp);
            su.Add(StatusUpdate.CurMp, (int)CurMp);
            su.Add(StatusUpdate.CurCp, (int)CurCp);
            su.Add(StatusUpdate.MaxCp, MaxCp);
            SendPacket(su);
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

            Online = 0;
            PlayerService.UpdatePlayer(this);
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

        public override void SetTarget(L2Character newTarget)
        {
            if (newTarget != null)
            {
                if (!newTarget.Visible)
                    newTarget = null;
            }

            L2Character oldTarget = Target;

            if (oldTarget != null)
            {
                if (oldTarget.Equals(newTarget))
                    return;

                oldTarget?.Status.RemoveStatusListener(this);
            }

            if (newTarget is L2StaticObject)
            {
                SendPacket(new MyTargetSelected(newTarget.ObjId, 0));
                SendPacket(new StaticObject((L2StaticObject) newTarget));
            }
            else
            {
                if (newTarget != null)
                {
                    if (newTarget.ObjId != ObjId)
                        SendPacket(new ValidateLocation(newTarget));

                    SendPacket(new MyTargetSelected(newTarget.ObjId, 0));

                    newTarget.Status.AddStatusListener(this);

                    StatusUpdate su = new StatusUpdate(newTarget);
                    su.Add(StatusUpdate.MaxHp, newTarget.MaxHp);
                    su.Add(StatusUpdate.CurHp, (int)newTarget.CurHp);
                    SendPacket(su);

                    BroadcastPacket(su, false);
                }
                
            }

            if (newTarget == null && Target != null)
            {
                BroadcastPacket(new TargetUnselected(this));
            }


            base.SetTarget(newTarget);
        }

        public override void OnAction(L2Player player)
        {
            if (player.Target != this)
                player.SetTarget(this);
            else
            {
                player.SendActionFailed();
                //follow
            }
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

            StatusUpdate su = new StatusUpdate(this);
            su.Add(StatusUpdate.CurLoad, CurrentWeight);
            SendPacket(su);

            int weightproc = (total * 1000) / 100; //max weight

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

            //if (newWeightPenalty > 0)
            //    AddSkill(4270, newWeightPenalty, false, true);
            //else
            //    RemoveSkill(4270, false, true);

            PenaltyWeight = newWeightPenalty;

            SendPacket(new EtcStatusUpdate(this));
        }

        public bool CheckFreeWeight(int weight)
        {
           // if ((CurrentWeight + weight) >= CharacterStat.GetStat(EffectType.BMaxWeight))
            //    return false;

            return true;
        }

        public bool CheckFreeWeight80(int weight)
        {
            //if ((CurrentWeight + weight) >= (CharacterStat.GetStat(EffectType.BMaxWeight) * .8))
            //    return false;

            return true;
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
            //RestoreSkills();
            //db_restoreQuests();
            //db_restoreRecipes();
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

        public bool IsInOlympiad = false;
        public L2Item EnchantScroll,
                      EnchantItem,
                      EnchantStone;
        public byte EnchantState = 0;

        // 0 cls, 1 violet, 2 blink
        public byte PvPStatus;

        public byte GetEnchantValue()
        {
            int val = Inventory.Paperdoll?[inventory.Inventory.PaperdollRhand]?.Enchant ?? 0;

            if (MountType > 0)
                return 0;

            if (val > 127)
                val = 127;

            return (byte)val;
        }

        public override void OnNewTargetSelection(L2Object target)
        {
            int color = 0;

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
            //L2Pet pet = new L2Pet();
            ////pet.setTemplate(NpcTable.Instance.GetNpcTemplate(PetID));
            //pet.SetOwner(this);
            //pet.ControlItem = _petControlItem;
            //// pet.sql_restore();
            //pet.SpawmMe();

            //_petSummonTime.Enabled = false;
        }

        private void NonpetSummonEnd(object sender, ElapsedEventArgs e)
        {
            //L2Summon summon = new L2Summon();
            ////summon.setTemplate(NpcTable.Instance.GetNpcTemplate(PetID));
            //summon.SetOwner(this);
            //summon.ControlItem = _petControlItem;
            //summon.SpawmMe();

            //_nonpetSummonTime.Enabled = false;
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

            if (Party == null)
                return chars.ToArray();

            foreach (L2Player pl in Party.Members.Where(pl => pl.ObjId != ObjId))
            {
                chars.Add(pl);
            }

            return chars.ToArray();
        }

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
        }

        public override void DoAttack(L2Character target)
        {
            if (target == null)
            {
                SendMessage("null");
                SendActionFailed();
                return;
            }

            if (target.Dead)
            {
                SendMessage("dead");
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
            double timeAtk = 100;//attackspeed
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
                TryMoveToAndHit(target.X, target.Y, target.Z,target);
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

            Target = target;

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
            if ((Target != null) && !Target.Dead)
            {
                if (!Hit1.Miss)
                {
                    if (Hit1.Crit)
                        SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1LandedACriticalHit).AddPlayerName(Name));

                    SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1HasGivenC2DamageOfS3).AddPlayerName(Name).AddName(Target).AddNumber(Hit1.Damage));
                    Target.ReduceHp(this, Hit1.Damage);

                    if (Target is L2Player)
                        Target.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1HasReceivedS3DamageFromC2).AddName(Target).AddName(this).AddNumber(Hit1.Damage));
                }
                else
                {
                    SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1AttackWentAstray).AddPlayerName(Name));

                    if (Target is L2Player)
                    {
                        Target.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1HasEvadedC2Attack).AddName(Target).AddName(this));
                    }
                }
            }

            AttackToHit.Enabled = false;
        }

        public override void AttackDoHit2Nd(object sender, ElapsedEventArgs e)
        {
            if ((Target != null) && !Target.Dead)
            {
                if (!Hit2.Miss)
                {
                    if (Hit2.Crit)
                        SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1LandedACriticalHit).AddName(this));

                    SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1HasGivenC2DamageOfS3).AddName(this).AddName(Target).AddNumber(Hit2.Damage));
                    Target.ReduceHp(this, Hit2.Damage);

                    if (Target is L2Player)
                        Target.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1HasReceivedS3DamageFromC2).AddName(Target).AddName(this).AddNumber(Hit2.Damage));
                }
                else
                {
                    SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1AttackWentAstray).AddPlayerName(Name));

                    if (Target is L2Player)
                    {
                        Target.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.C1HasEvadedC2Attack).AddName(Target).AddName(this));
                    }
                }
            }

            AttackToHitBonus.Enabled = false;
        }

        public override void AttackDoEnd(object sender, ElapsedEventArgs e)
        {
            AttackToEnd.Enabled = false;
            AttackToEnd.Stop();
            if(this.Target.Dead)
            {
                SendMessage("Target Killed");
                if(this.Target is L2Npc)
                {
                    try
                    {
                        L2Npc target = (L2Npc)this.Target;
                        target.DoDie(this);
                    }
                    catch (Exception ex)
                    {
                        Log.Debug(ex.Message);
                    }
                }
                
            }

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
            //if (pk is SunSet) //включаем ночные скилы
            //    AiCharacter.NotifyOnStartNight();
            //else
            //    AiCharacter.NotifyOnStartDay();
        }

        public int VehicleId => Boat?.ObjId ?? 0;

        public void Revive(double percent)
        {
            BroadcastPacket(new Revive(ObjId));
            Dead = false;
            Status.StartHpMpRegeneration();
        }

        private DateTime _pingTimeout;
        private int _lastPingId;
        public int Ping = -1;
        public MultiSellList CustomMultiSellList;
        public int LastRequestedMultiSellId = -1;
        public int AttackingId;

        public void RequestPing()
        {
            _lastPingId = new Random().Next();
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

        public void BroadcastSkillUse(int skillId)
        {
            //Skill skill = SkillTable.Instance.Get(skillId);
            //BroadcastPacket(new MagicSkillUse(this, this, skill.SkillId, skill.Level, skill.SkillHitTime));
        }

        public override L2Item GetWeaponItem()
        {
            return null;
        }

        public void UpdateAgathionEnergy(int count)
        {
            SendMessage($"@UpdateAgathionEnergy {count}");
        }

        
        public override void DoDie(L2Character killer)
        {
            base.DoDie(killer);
        }

        public bool CharDeleteTimeExpired() => (DeleteTime > 0) && (DeleteTime <= Utilz.CurrentTimeMillis());

        public int RemainingDeleteTime() => AccessLevel > -100 ? (DeleteTime > 0 ? (int)((DeleteTime - Utilz.CurrentTimeMillis()) / 1000) : 0) : -1;

        public void SetCharDeleteTime() => DeleteTime = Utilz.CurrentTimeMillis() + (Config.Config.Instance.GameplayConfig.Server.Client.DeleteCharAfterDays * 86400000L);

        public void SetCharLastAccess() => LastAccess = Utilz.CurrentTimeMillis();

        internal void DropItem(int objectId, int count, int x, int y, int z)
        {

        }

        public override string ToString()
        {
            return $"{Name} - {base.ToString()}";
        }

        public void SetupKnows()
        {
            foreach (var obj in L2World.Instance.GetObjects().Where(x=>x.Region == Region))
            {
                obj.BroadcastUserInfoToObject(this);
            }
        }
    }
}