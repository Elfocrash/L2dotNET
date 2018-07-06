using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using L2dotNET.DataContracts;
using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Enums;
using L2dotNET.Models.Inventory;
using L2dotNET.Models.Items;
using L2dotNET.Models.Npcs;
using L2dotNET.Models.Npcs.Decor;
using L2dotNET.Models.Player.General;
using L2dotNET.Models.Stats;
using L2dotNET.Models.Stats.Funcs;
using L2dotNET.Models.Status;
using L2dotNET.Models.Vehicles;
using L2dotNET.Network;
using L2dotNET.Network.serverpackets;
using L2dotNET.Services.Contracts;
using L2dotNET.Templates;
using L2dotNET.Tools;
using L2dotNET.Utility;
using L2dotNET.World;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace L2dotNET.Models.Player
{
    
    public class L2Player : L2Character
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public readonly ICharacterService _characterService;

        public new PlayerStatus CharStatus => (PlayerStatus) base.CharStatus;

        public AccountContract Account { get; set; }
        public GameClient Gameclient { get; set; }

        public ClassId ClassId { get; set; }
        public L2PartyRoom PartyRoom { get; set; }
        public L2Party Party { get; set; }
        public Gender Sex { get; set; }
        public HairStyleId HairStyleId { get; set; }
        public HairColor HairColor { get; set; }
        public Face Face { get; set; }
        public bool WhisperBlock { get; set; }
        public long Experience { get; set; }
        public long ExpOnDeath { get; set; }
        public long ExpAfterLogin { get; set; }
        public int Sp { get; set; }
        public double CurrentCp
        {
            get => CharStatus.CurrentCp;
            set => CharStatus.SetCurrentCp(value);
        }
        public int Karma { get; set; }
        public int PvpKills { get; set; }
        public DateTime? DeleteTime { get; set; }
        public int CanCraft { get; set; }
        public int PkKills { get; set; }
        public int RecomendationsLeft { get; set; }
        public int RecomandationsHave { get; set; }
        public int AccessLevel { get; set; }
        public int Online { get; set; }
        public int OnlineTime { get; set; }
        public int CharacterSlot { get; set; }
        public int CurrentWeight { get; set; }
        public double CollisionRadius { get; set; }
        public double CollisionHeight { get; set; }
        public int CursedWeaponLevel { get; set; }
        public DateTime? LastAccess { get; set; }
        public int IsIn7SDungeon { get; set; }
        public int? PunishLevel { get; set; }
        public DateTime? PunishTime { get; set; }
        public int PowerGrade { get; set; }
        public bool Nobless { get; set; }
        public bool Hero { get; set; }
        public DateTime? LastRecomendationDate { get; set; }
        public PcInventory Inventory { get; set; }
        public dynamic SessionData { get; set; }
        public new PcTemplate Template { get; set; }

        public byte Builder = 1;
        public byte Noblesse = 0;
        public byte Heroic = 0;

        public bool Diet = false;
        public int PenaltyWeight;
        public int PenaltyGrade = 0;
        public int CurrentFocusEnergy = 0;

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

        public int PCreateCommonItem = 0;
        public int PCreateItem = 0;
        public List<L2Shortcut> Shortcuts = new List<L2Shortcut>();
        public int ZoneId = -1;
        public int Obsx = -1;
        public int Obsy;
        public int Obsz;

        // arrow, bolt
        public L2Item SecondaryWeaponSupport;

        public override L2Item ActiveWeapon => null;
        public List<int> AutoSoulshots = new List<int>();
        public List<int> SetKeyItems;
        public int SetKeyId;

        public int MountType;
        public NpcTemplate MountedTemplate;
        public int TradeState;
        public SortedList<int, int> CurrentTrade;
        public int Sstt;
        private DateTime _pingTimeout;
        private int _lastPingId;
        public int Ping = -1;
        public int AttackingId;

        private L2Chair _chair;
        public L2Boat Boat;
        public int BoatX;
        public int BoatY;
        public int BoatZ;

        public PcTemplate BaseClass;
        public PcTemplate ActiveClass;

        private ActionFailed _af;

        public int ViewingAdminPage;
        public int ViewingAdminTeleportGroup = -1;
        public int TeleportPayId;
        public int LastMinigameScore;
        public short ClanType;
        public int Fame;


        private Timer _sitTime;
        private bool _isSitting;

        private Timer _petSummonTime,
                      _nonpetSummonTime;

        private int _petId = -1;
        private L2Item _petControlItem;

        public bool IsInOlympiad = false;

        public L2Item EnchantScroll,
                      EnchantItem,
                      EnchantStone;

        public byte EnchantState = 0;

        // 0 cls, 1 violet, 2 blink
        public byte PvPStatus;

        public bool IsCursed = false;
        public string PenaltyClanCreate = "0";
        public string PenaltyClanJoin = "0";
        public byte PartyState;
        public L2Player Requester;
        public int ItemDistribution;

        public int VehicleId => Boat?.ObjectId ?? 0;

        public L2Player(PcTemplate template, int objectId) : base(objectId, template)
        {
            Template = template;
            _characterService = GameServer.ServiceProvider.GetService<ICharacterService>();
            CharacterStat = new CharacterStat(this);
            AddFuncsToNewCharacter();
            InitializeCharacterStatus();

            Inventory = new PcInventory(this);
            Title = string.Empty;
            Experience = 0;
            Level = 1;
            ClassId = template.ClassId;
            BaseClass = template;
            ActiveClass = template;
            CharStatus.CurrentCp = MaxCp;
            CharStatus.SetCurrentHp(MaxHp, false);
            CharStatus.SetCurrentMp(MaxMp, false);
            X = template.SpawnX;
            Y = template.SpawnY;
            Z = template.SpawnZ;

            if (template.DefaultInventory != null)
            {
                //foreach (PC_item i in template._items)
                //{
                //    if (!i.item.isStackable())
                //    {
                //        for (long s = 0; s < i.count; s++)
                //        {
                //            L2Item item = new L2Item(i.item);
                //            item.Enchant = i.enchant;
                //            if (i.lifetime != -1)
                //                item.AddLimitedHour(i.lifetime);

                //            item.Location = L2Item.L2ItemLocation.inventory;
                //            player.Inventory.addItem(item, false, false);

                //            if (i.equip)
                //            {
                //                int pdollId = player.Inventory.getPaperdollId(item.Template);
                //                player.setPaperdoll(pdollId, item, false);
                //            }
                //        }
                //    }
                //    else
                //        player.addItem(i.item.ItemID, i.count);
                //}
            }
        }

        public L2Player(ICharacterService characterService) :base(0, null)
        {
            _characterService = characterService;
        }

        public override void InitializeCharacterStatus()
        {
            base.CharStatus = new PlayerStatus(this);
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

        public override async Task SendPacketAsync(GameserverPacket gameserverPacket)
        {
            await Gameclient.SendPacketAsync(gameserverPacket);
        }

        public override async Task SendActionFailedAsync()
        {
            if (_af == null)
                _af = new ActionFailed();

            await SendPacketAsync(_af);
        }

        public override async Task SendSystemMessage(SystemMessage.SystemMessageId msgId)
        {
            await SendPacketAsync(new SystemMessage(msgId));
        }

        public override async Task SendMessageAsync(string p)
        {
            await SendPacketAsync(new SystemMessage(SystemMessage.SystemMessageId.S1).AddString(p));
        }

        public int GetForceIncreased()
        {
            return CurrentFocusEnergy;
        }

        public void UpdateReuse()
        {
            SendPacketAsync(new SkillCoolTime(this));
        }

        public override void AddFuncsToNewCharacter()
        {
            base.AddFuncsToNewCharacter();

            CharacterStat.AddStatFunction(FuncMaxCpMul.Instance);

            //Henna stuff go here
        }

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
                SendPacketAsync(new NpcHtmlMessage(this, $"./html/{file}", o.ObjectId, 0));
                if (o is L2Npc npc)
                    FolkNpc = npc;
            }
            else
                ShowHtmPlain(file, o);
        }

        public void ShowHtm(string file, L2Npc npc, int questId)
        {
            if (file.EndsWithIgnoreCase(".htm"))
            {
                NpcHtmlMessage htm = new NpcHtmlMessage(this, file, npc.ObjectId, 0);
                htm.Replace("<?quest_id?>", questId);
                SendPacketAsync(htm);
                FolkNpc = npc;
            }
            else
                ShowHtmPlain(file, npc);
        }
        
        public void UpdateAndBroadcastStatus(int broadcastType)
        {
            if (broadcastType == 1)
                SendPacketAsync(new UserInfo(this));
            else if (broadcastType == 2)
            {
                BroadcastUserInfoAsync();
            }
        }

        public void ShowHtmPlain(string plain, L2Object o)
        {
            SendPacketAsync(new NpcHtmlMessage(this, plain, o?.ObjectId ?? -1, true));
            if (o is L2Npc)
                FolkNpc = (L2Npc)o;
        }

        public void SendQuestList()
        {
            SendPacketAsync(new QuestList(this));
        }

        public void AddExpSp(int exp, int sp, bool msg)
        {
            SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.YouEarnedS1ExpAndS2Sp);
            sm.AddNumber(exp);
            sm.AddNumber(sp);
            SendPacketAsync(sm);

            Experience += exp;
            Sp += sp;
            //Need to Level up ?
            if(Level != Player.Experience.GetLevel(Experience))
            {
                Level = Player.Experience.GetLevel(Experience);
                this.BroadcastPacketAsync(new SocialAction(ObjectId, 15));
                this.SendPacketAsync(new SystemMessage(SystemMessage.SystemMessageId.YouIncreasedYourLevel));
            }

            StatusUpdate su = new StatusUpdate(this);
            su.Add(StatusUpdate.Exp, (int)Experience);
            su.Add(StatusUpdate.Sp, Sp);
            su.Add(StatusUpdate.Level, Level);
            SendPacketAsync(su);
        }

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
        
        public bool IsAlikeDead()
        {
            return false;
        }

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

                SendPacketAsync(new ShortCutRegister(sc));

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
                SendPacketAsync(new SystemMessage(SystemMessage.SystemMessageId.EarnedS1Adena).AddNumber(count));

            if (count <= 0)
                return;

            InventoryUpdate iu = new InventoryUpdate();
            iu.AddNewItem(Inventory.AddItem(57, count, this));
            SendPacketAsync(iu);
        }

        public override string AsString()
        {
            return $"L2Player:{Name}";
        }

        public override void OnRemObject(L2Object obj)
        {
            SendPacketAsync(new DeleteObject(obj.ObjectId));
        }

        public override void OnAddObject(L2Object obj, GameserverPacket pk, string msg = null)
        {
            if (obj is L2Npc)
                SendPacketAsync(new NpcInfo((L2Npc)obj));
            else
            {
                if (obj is L2Player)
                {
                    SendPacketAsync(new CharInfo((L2Player)obj));

                    if (msg != null)
                        ((L2Player)obj).SendMessageAsync(msg);
                }
                else
                {
                    if (obj is L2Item)
                        SendPacketAsync(pk ?? new SpawnItem((L2Item)obj));
                    else
                    {
                       
                        {
                            if (obj is L2Chair)
                                SendPacketAsync(new StaticObject((L2Chair)obj));
                            else
                            {
                                if (obj is L2StaticObject)
                                    SendPacketAsync(new StaticObject((L2StaticObject)obj));
                                else
                                {
                                    if (obj is L2Boat)
                                        SendPacketAsync(new VehicleInfo((L2Boat)obj));
                                }
                            }
                        }
                    }
                }
            }
        }

        public override async Task BroadcastStatusUpdateAsync()
        {
            StatusUpdate statusUpdate = new StatusUpdate(this);
            statusUpdate.Add(StatusUpdate.CurHp, (int)CharStatus.CurrentHp);
            statusUpdate.Add(StatusUpdate.CurMp, (int)CharStatus.CurrentMp);
            statusUpdate.Add(StatusUpdate.CurCp, (int)CurrentCp);
            statusUpdate.Add(StatusUpdate.MaxCp, MaxCp);
            await SendPacketAsync(statusUpdate);
        }

        public async Task BroadcastCharInfoAsync()
        {
            foreach (L2Player player in L2World.GetPlayers().Where(player => player != this))
                await player.SendPacketAsync(new CharInfo(this));
        }

        public override async Task BroadcastUserInfoAsync()
        {
            await SendPacketAsync(new UserInfo(this));

            //if (getPolyType() == PolyType.NPC)
            //    Broadcast.toKnownPlayers(this, new AbstractNpcInfo.PcMorphInfo(this, getPolyTemplate()));
            //else
            await BroadcastCharInfoAsync();
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
            obj.SendInfoAsync(this);

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
            _characterService.UpdatePlayer(this);
            L2World.RemovePlayer(this);
            DecayMe();

        }

        public bool HasItem(int itemId, int count)
        {
            return Inventory.Items.Where(item => item.Template.ItemId == itemId).Any(item => item.Count >= count);
        }

        public override async Task SendInfoAsync(L2Player player)
        {
            //if (this.Boa isInBoat())
            //    getPosition().set(getBoat().getPosition());

            //if (getPolyType() == PolyType.NPC)
            //    activeChar.sendPacket(new AbstractNpcInfo.PcMorphInfo(this, getPolyTemplate()));
            //else
            //{
            await player.SendPacketAsync(new CharInfo(this));

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

        public override async Task SetTargetAsync(L2Character newTarget)
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

                oldTarget?.CharStatus.RemoveStatusListener(this);
            }

            if (newTarget is L2StaticObject)
            {
                await SendPacketAsync(new MyTargetSelected(newTarget.ObjectId, 0));
                await SendPacketAsync(new StaticObject((L2StaticObject) newTarget));
            }
            else
            {
                if (newTarget != null)
                {
                    if (newTarget.ObjectId != ObjectId)
                        await SendPacketAsync(new ValidateLocation(newTarget));

                    await SendPacketAsync(new MyTargetSelected(newTarget.ObjectId, Level - newTarget.Level));

                    newTarget.CharStatus.AddStatusListener(this);

                    StatusUpdate su = new StatusUpdate(newTarget);
                    su.Add(StatusUpdate.MaxHp, newTarget.MaxHp);
                    su.Add(StatusUpdate.CurHp, (int)newTarget.CharStatus.CurrentHp);
                    await SendPacketAsync(su);

                    await BroadcastPacketAsync(su, false);
                }
                
            }

            if (newTarget == null && Target != null)
            {
                await BroadcastPacketAsync(new TargetUnselected(this));
            }


            base.SetTargetAsync(newTarget);
        }

        public override async Task OnActionAsync(L2Player player)
        {
            if (player.Target != this)
                await player.SetTargetAsync(this);
            else
            {
                await player.SendActionFailedAsync();
                //follow
            }
        }

        public async Task ShowHtmAdminAsync(string val, bool plain)
        {
            await SendPacketAsync(new NpcHtmlMessage(this, val, this.ObjectId));

            ViewingAdminPage = 1;
        }

        public void ShowHtmBbs(string val)
        {
            ShowBoard.SeparateAndSendAsync(val, this);
        }

        public void SendItemList(bool open = false)
        {
            SendPacketAsync(new ItemList(this, open));
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
            SendPacketAsync(su);

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

            SendPacketAsync(new EtcStatusUpdate(this));
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
            BroadcastPacketAsync(new ExBrExtraUserInfo(ObjectId, AbnormalBitMaskEvent));
        }

        public override void UpdateAbnormalExEffect()
        {
            BroadcastUserInfoAsync();
        }

        public void SetPenalty_ClanCreate(DateTime time, bool sql)
        {
            PenaltyClanCreate = DateTime.Now < time ? time.ToString("yyyy-MM-dd HH-mm-ss") : "0";

            if (sql) { }
        }

        public void SetPenalty_ClanJoin(DateTime time, bool sql)
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

        public void DB_RestoreShortcuts()
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

        public void PendToJoinParty(L2Player asker, int askerItemDistribution)
        {
            PartyState = 1;
            Requester = asker;
            Requester.ItemDistribution = askerItemDistribution;
            SendPacketAsync(new AskJoinParty(asker.Name, askerItemDistribution));
        }

        public void ClearPend()
        {
            PartyState = 0;
            Requester = null;
        }

        public byte GetEnchantValue()
        {
            int val = Inventory.Paperdoll?[Models.Inventory.Inventory.PaperdollRhand]?.Enchant ?? 0;

            if (MountType > 0)
                return 0;

            if (val > 127)
                val = 127;

            return (byte)val;
        }

        public override void OnNewTargetSelection(L2Object target)
        {
            int color = 0;

            SendPacketAsync(new MyTargetSelected(target.ObjectId, color));
        }

        public override void OnOldTargetSelection(L2Object target)
        {
            double dis = Calcs.CalculateDistance(this, target, true);
            if (dis < 151)
                target.NotifyActionAsync(this);
            else
                TryMoveToAsync(target.X, target.Y, target.Z);

            SendActionFailedAsync();
        }

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

            BroadcastPacketAsync(new MagicSkillUse(this, this, 1111, 1, 5000));
            SendPacketAsync(new SetupGauge(ObjectId, SetupGauge.SgColor.Blue, 4900));
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

            foreach (L2Player pl in Party.Members.Where(pl => pl.ObjectId != ObjectId))
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

        public async Task SitAsync()
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
            await BroadcastPacketAsync(new ChangeWaitType(this, ChangeWaitType.Sit));
        }

        public async Task StandAsync()
        {
            _sitTime.Enabled = true;
            await BroadcastPacketAsync(new ChangeWaitType(this, ChangeWaitType.Stand));
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

        public void SetChair(L2Chair chairObj)
        {
            _chair = chairObj;
            _chair.IsUsedAlready = true;
            BroadcastPacketAsync(new ChairSit(ObjectId, chairObj.StaticId));
        }

        public bool IsOnShip()
        {
            return false;
        }

        public bool IsWard()
        {
            return false;
        }

        public override void AbortAttack()
        {
            base.AbortAttack();
        }

        public override async Task DoAttackAsync(L2Character target)
        {
            if (target == null)
            {
                await SendMessageAsync("null");
                await SendActionFailedAsync();
                return;
            }

            if (target.Dead)
            {
                await SendMessageAsync("dead");
                await SendActionFailedAsync();
                return;
            }

            if ((AttackToHit != null) && AttackToHit.Enabled)
            {
                await SendActionFailedAsync();
                return;
            }

            if ((AttackToEnd != null) && AttackToEnd.Enabled)
            {
                await SendActionFailedAsync();
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
                await SendMessageAsync($"too far {dist}");
                TryMoveToAndHitAsync(target.X, target.Y, target.Z,target);
                return;
            }

            if ((reqMp > 0) && (reqMp > CharStatus.CurrentMp))
            {
                await SendMessageAsync($"no mp {CharStatus.CurrentMp} {reqMp}");
                await SendActionFailedAsync();
                return;
            }

            if (ranged)
            {
                await SendPacketAsync(new SetupGauge(ObjectId, SetupGauge.SgColor.Red, (int)timeAtk));
                //Inventory.destroyItem(SecondaryWeaponSupport, 1, false, true);
            }

            Attack atk = new Attack(this, target, ss, 5);

            if (dual)
            {
                Hit1 = GenHitSimple(true, ss);
                atk.AddHit(target.ObjectId, (int)Hit1.Damage, Hit1.Miss, Hit1.Crit, Hit1.ShieldDef > 0);

                Hit2 = GenHitSimple(true, ss);
                atk.AddHit(target.ObjectId, (int)Hit2.Damage, Hit2.Miss, Hit2.Crit, Hit2.ShieldDef > 0);
            }
            else
            {
                Hit1 = GenHitSimple(false, ss);
                atk.AddHit(target.ObjectId, (int)Hit1.Damage, Hit1.Miss, Hit1.Crit, Hit1.ShieldDef > 0);
            }

            Target = target;

            if (AttackToHit == null)
            {
                AttackToHit = new Timer();
                AttackToHit.Elapsed += AttackDoHitAsync;
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

            BroadcastPacketAsync(atk);
        }

        public override async void AttackDoHitAsync(object sender, ElapsedEventArgs e)
        {
            if ((Target != null) && !Target.Dead)
            {
                if (!Hit1.Miss)
                {
                    if (Hit1.Crit)
                    {
                        await SendPacketAsync(new SystemMessage(SystemMessage.SystemMessageId.YouDidS1Dmg).AddNumber(Hit1.Damage));
                        await SendPacketAsync(new SystemMessage(SystemMessage.SystemMessageId.CriticalHit));
                    }

                    await SendPacketAsync(new SystemMessage(SystemMessage.SystemMessageId.YouDidS1Dmg).AddNumber(Hit1.Damage));
                    Target.CharStatus.ReduceHp(Hit1.Damage, this);

                    if (Target is L2Player)
                        await Target.SendPacketAsync(new SystemMessage(SystemMessage.SystemMessageId.C1HasReceivedS3DamageFromC2).AddName(Target).AddName(this).AddNumber(Hit1.Damage));
                }
                else
                {
                    await SendPacketAsync(new SystemMessage(SystemMessage.SystemMessageId.MissedTarget));

                    if (Target is L2Player)
                    {
                        await Target.SendPacketAsync(new SystemMessage(SystemMessage.SystemMessageId.C1HasEvadedC2Attack).AddName(Target).AddName(this));
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
                    {
                        SendPacketAsync(new SystemMessage(SystemMessage.SystemMessageId.YouDidS1Dmg).AddNumber(Hit2.Damage));
                        SendPacketAsync(new SystemMessage(SystemMessage.SystemMessageId.CriticalHit));
                    }

                    SendPacketAsync(new SystemMessage(SystemMessage.SystemMessageId.YouDidS1Dmg).AddNumber(Hit2.Damage));
                    Target.CharStatus.ReduceHp(Hit2.Damage, this);

                    if (Target is L2Player)
                        Target.SendPacketAsync(new SystemMessage(SystemMessage.SystemMessageId.C1HasReceivedS3DamageFromC2).AddName(Target).AddName(this).AddNumber(Hit2.Damage));
                }
                else
                {
                    SendPacketAsync(new SystemMessage(SystemMessage.SystemMessageId.MissedTarget));

                    if (Target is L2Player)
                    {
                        Target.SendPacketAsync(new SystemMessage(SystemMessage.SystemMessageId.C1HasEvadedC2Attack).AddName(Target).AddName(this));
                    }
                }
            }

            AttackToHitBonus.Enabled = false;
        }

        public override void AttackDoEnd(object sender, ElapsedEventArgs e)
        {
            AttackToEnd.Enabled = false;
            AttackToEnd.Stop();
            //if(this.Target.Dead)
            //{
            //    SendMessage("Target Killed");
            //    if(this.Target is L2Npc)
            //    {
            //        try
            //        {
            //            L2Npc target = (L2Npc)this.Target;
            //            target.DoDieAsync(this);
            //        }
            //        catch (Exception ex)
            //        {
            //            Log.Debug(ex.Message);
            //        }
            //    }
                
            //}

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
     
        public void Mount(NpcTemplate npc)
        {
            BroadcastPacketAsync(new Ride(this, true, npc.NpcId));
            MountedTemplate = npc;
            BroadcastUserInfoAsync();
        }

        public void MountPet()
        {
            
        }

        public void UnMount()
        {
            BroadcastPacketAsync(new Ride(this, false));
            MountedTemplate = null;
            BroadcastUserInfoAsync();
        }

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
            SendPacketAsync(pk);
            //if (pk is SunSet) //включаем ночные скилы
            //    AiCharacter.NotifyOnStartNight();
            //else
            //    AiCharacter.NotifyOnStartDay();
        }

        public void Revive(double percent)
        {
            BroadcastPacketAsync(new Revive(ObjectId));
            Dead = false;
            CharStatus.StartHpMpRegeneration();
        }

        public void RequestPing()
        {
            _lastPingId = RandomThreadSafe.Instance.Next();
            NetPing ping = new NetPing(_lastPingId);
            _pingTimeout = DateTime.Now;
            SendPacketAsync(ping);
        }

        public void UpdatePing(int id, int ms, int unk)
        {
            if (_lastPingId != id)
            {
                Log.Warn($"player fail to ping respond right {id} {_lastPingId} at {_pingTimeout.ToLocalTime()}");
                return;
            }

            Ping = ms;
            SendMessageAsync($"Your connection latency is {ms}");
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
            SendMessageAsync($"debug: expPet {expPet}");
            SendMessageAsync($"debug: mob.Template {mob.Template.Exp} @");
            SendMessageAsync($"debug: expReward {expReward}");
            SendMessageAsync($"debug: sp {sp}");

            byte oldLvl = Level;
            Experience += (long)expReward;
            byte newLvl = Player.Experience.GetLevel(Experience);
            bool lvlChanged = oldLvl != newLvl;

            Experience += (long)expReward;
            if (lvlChanged)
            {
                Level = newLvl;
                BroadcastPacketAsync(new SocialAction(ObjectId, 2122));
            }

            if (!lvlChanged)
                SendPacketAsync(new UserInfo(this));
            else
                BroadcastUserInfoAsync();
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
        
        public override async Task DoDieAsync(L2Character killer)
        {
            await base.DoDieAsync(killer);
        }

        internal void DropItem(int objectId, int count, int x, int y, int z)
        {

        }

        public override string ToString()
        {
            return $"{Name} - {base.ToString()}";
        }

        public async Task SetupKnowsAsync()
        {
            foreach (var obj in L2World.GetObjects().Where(x=>x.Region == Region))
            {
                await obj.BroadcastUserInfoToObjectAsync(this);
            }
        }
        public async Task SetupKnowsAsync(L2WorldRegion region)
        {
            var regions = region.GetSurroundingRegions();
            foreach (var reg in regions)
            {
                foreach (var obj in L2World.GetObjects().Where(x => x.Region == reg))
                {
                    await obj.BroadcastUserInfoToObjectAsync(this);
                }
            }
        }

        public bool CharDeleteTimeExpired() => DeleteTime.HasValue && DeleteTime.Value < DateTime.UtcNow;

        public int RemainingDeleteTime() => AccessLevel > -100
            ? (DeleteTime.HasValue ? (int) (DeleteTime.Value - DateTime.UtcNow).TotalSeconds : 0)
            : -1;

        public void SetCharDeleteTime() => DeleteTime = DateTime.UtcNow.AddDays(_characterService.GetDaysRequiredToDeletePlayer());

        public void SetCharLastAccess() => LastAccess = DateTime.UtcNow;
    }
}