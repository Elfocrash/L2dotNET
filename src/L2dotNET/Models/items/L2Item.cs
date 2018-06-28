using System;
using System.Linq;
using System.Threading.Tasks;
using L2dotNET.DataContracts;
using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;
using L2dotNET.Services.Contracts;
using L2dotNET.Tables;
using L2dotNET.Tools;
using L2dotNET.World;
using static L2dotNET.Models.Inventory.Inventory;

namespace L2dotNET.Models.Items
{
    public class L2Item : L2Object
    {
        public IItemService _itemService;

        public ItemTemplate Template;
        public int Count;
        public short IsEquipped { get; set; }

        public bool Equipped => IsEquipped > 0;

        public int Enchant;
        public int AugmentationId = 0;
        public int Durability;
        public ItemLocation Location;
        public int PaperdollSlot = -1;
        public int PetId = -1;
        public int Dropper;
        public int SlotLocation;

        public bool ExistsInDb { get; set; }
        public int OwnerId { get; set; }

        public short AttrAttackType = -2;
        public short AttrAttackValue = 0;

        public bool Blocked = false;
        public bool TempBlock = false;
        private readonly IdFactory _idFactory;
        public L2Item(IItemService itemService, IdFactory idFactory, ItemTemplate template , int objectId) : base(objectId)
        {
            _itemService = itemService;
            _idFactory = idFactory;
            ObjId = objectId != 0 ? objectId : _idFactory.NextId();
            Template = template;
            Count = 1;
            Location = ItemLocation.Void;
        }

        public void GenId()
        {
            ObjId = _idFactory.NextId();
        }

        public void ChangeCount(int count, L2Player creator)
        {
            if (count == 0)
                return;

            if ((count > 0) && (Count > (int.MaxValue - count)))
                Count = int.MaxValue;
            else
                Count = Count + count;

            if (Count < 0)
                Count = 0;
        }

        public void Unequip(L2Player owner)
        {
            PaperdollSlot = -1;
            Location = ItemLocation.Inventory;
            SlotLocation = -1;
            PaperdollSlot = -1;
            IsEquipped = 0;
        }

        public void Equip(L2Player owner)
        {
            Location = ItemLocation.Paperdoll;
            SlotLocation = GetPaperdollIndex((int) Template.BodyPart);
            PaperdollSlot = GetPaperdollIndex((int) Template.BodyPart);
            IsEquipped = 1;
        }

        public void NotifyStats(L2Player owner)
        {
        }

        private void TryEquipSecondary(L2Player owner) { }

        public void DropMe(int x, int y, int z, L2Character dropper, L2Character killer, int seconds)
        {
            X = x;
            Y = y;
            Z = z;
            DropItem pk = new DropItem(this);
            if (dropper != null)
                Dropper = dropper.ObjId;

            Location = ItemLocation.Void;

            killer?.AddKnownObject(this, pk, true);

            L2World.Instance.AddObject(this);
        }

        public void DropMe(int x, int y, int z)
        {
            DropMe(x, y, z, null, null, 0);
        }

        public override async Task OnActionAsync(L2Player player)
        {
            double dis = Calcs.CalculateDistance(this, player, true);
            await player.SendMessageAsync($"{AsString()} dis {(int)dis}");
            if (dis < 80)
            {
                foreach (L2Player o in KnownObjects.Values.OfType<L2Player>())
                {
                    await o.SendPacketAsync(new GetItem(player.ObjId, ObjId, X, Y, Z));
                    await o.SendPacketAsync(new DeleteObject(ObjId));
                }

                player.OnPickUp(this);

                L2World.Instance.RemoveObject(this);
            }
            else
                await player.TryMoveToAsync(X, Y, Z);
        }

        public override async Task OnForcedAttackAsync(L2Player player)
        {
            await player.SendActionFailedAsync();
        }

        public void UpdateDatabase()
        {
            if (ExistsInDb)
            {
                //if(OwnerId == 0 || Location == ItemLocation.Void || (Count == 0 && Location != ItemLocation.Lease))
                //    RemoveFromDb();
                //else
                UpdateInDb();
            }
            else
            {
                if ((OwnerId == 0) || (Location == ItemLocation.Void) || ((Count == 0) && (Location != ItemLocation.Lease)))
                    return;

                InsertInDb();
                ExistsInDb = true;
            }
        }

        private void UpdateInDb()
        {
            ItemContract contract = MapItemModel();

            _itemService.UpdateItem(contract);
        }

        private void InsertInDb()
        {
            Location = ItemLocation.Inventory;
            ItemContract contract = MapItemModel();

            _itemService.InsertNewItem(contract);
        }

        private ItemContract MapItemModel()
        {
            ItemContract contract = new ItemContract
            {
                ObjectId = ObjId,
                ItemId = Template.ItemId,
                Count = Count,
                CustomType1 = CustomType1,
                CustomType2 = CustomType2,
                Enchant = Enchant,
                LocationData = SlotLocation,
                Location = Location,
                CharacterId = OwnerId,
                ManaLeft = 0,
                Time = 0,
                TimeOfUse = null
            };
            return contract;
        }
        
        private bool _lifeTimeEndEnabled;
        private DateTime _lifeTimeEndTime;
        public int CustomType1;
        public int CustomType2;
        public bool Soulshot = false,
                    Spiritshot = false,
                    BlessSpiritshot = false;

        public int LifeTimeEnd()
        {
            if (!_lifeTimeEndEnabled)
                return -9999;

            TimeSpan ts = _lifeTimeEndTime - DateTime.Now;
            return (int)ts.TotalSeconds;
        }

        public void AddLimitedHour(int hours)
        {
            if (_lifeTimeEndEnabled)
                _lifeTimeEndTime = _lifeTimeEndTime.AddHours(hours);
            else
            {
                _lifeTimeEndEnabled = true;
                _lifeTimeEndTime = DateTime.Now.AddHours(hours);
            }
        }

        public void SetLimitedHour(string str)
        {
            if (str == "-1")
                return;

            string[] x1 = str.Split(' ');
            int yy = Convert.ToInt32(x1[0].Split('-')[0]);
            int mm = Convert.ToInt32(x1[0].Split('-')[1]);
            int dd = Convert.ToInt32(x1[0].Split('-')[2]);
            int hh = Convert.ToInt32(x1[1].Split('-')[0]);
            int m = Convert.ToInt32(x1[1].Split('-')[1]);
            int ss = Convert.ToInt32(x1[1].Split('-')[2]);

            DateTime dt = new DateTime(yy, mm, dd, hh, m, ss);
            if (dt <= DateTime.Now)
                return;

            _lifeTimeEndEnabled = true;
            _lifeTimeEndTime = dt;
            //TODO delete me
        }

        public bool IsEquipable()
        {
            return !(Template.BodyPart == 0);
        }

        public override string AsString()
        {
            return $"L2Item:{Template.ItemId}; count {Count}; enchant {Enchant}; id {ObjId}";
        }

        public bool NotForTrade()
        {
            return !Template.Tradable || (AugmentationId > 0) || (IsEquipped == 1);
        }

        public bool NotForSale()
        {
            return !Template.Tradable || (IsEquipped == 1);
        }
    }
}