using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using L2dotNET.DataContracts;
using L2dotNET.Logging.Abstraction;
using L2dotNET.Models.Items;
using L2dotNET.Models.Player;
using L2dotNET.Services.Contracts;
using L2dotNET.Templates;
using L2dotNET.World;

namespace L2dotNET.Tables
{
    public class ItemTable : IInitialisable
    {
        private static readonly ILog Log = LogProvider.GetCurrentClassLogger();

        private readonly ICrudService<ItemContract> _itemCrudService;
        private readonly ICrudService<ArmorContract> _armorCrudService;
        private readonly ICrudService<WeaponContract> _weaponCrudService;
        private readonly ICrudService<EtcItemContract> _etcItemCrudService;
        private readonly IdFactory _idFactory;

        private Dictionary<int, Armor> Armors { get; set; }
        private Dictionary<int, Weapon> Weapons { get; set; }
        private Dictionary<int, EtcItem> EtcItems { get; set; }

        public bool Initialised { get; private set; }

        public ItemTable(ICrudService<ItemContract> itemCrudService,
            ICrudService<ArmorContract> armorCrudService,
            ICrudService<WeaponContract> weaponCrudService,
            ICrudService<EtcItemContract> etcItemCrudService, 
            IdFactory idFactory)
        {
            _itemCrudService = itemCrudService;
            _armorCrudService = armorCrudService;
            _weaponCrudService = weaponCrudService;
            _etcItemCrudService = etcItemCrudService;
            _idFactory = idFactory;
        }

        internal ItemTemplate GetItem(int id)
        {
            if (Armors.ContainsKey(id))
            {
                return Armors[id];
            }

            if (Weapons.ContainsKey(id))
            {
                return Weapons[id];
            }

            if (EtcItems.ContainsKey(id))
            {
                return EtcItems[id];
            }

            return null;
        }

        public async Task Initialise()
        {
            if (Initialised)
            {
                return;
            }

            await LoadArmorModels();
            await LoadWeaponModels();
            await LoadEtcItemModels();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            Log.Info($"Loaded #{Armors.Count} armors, #{Weapons.Count} weapons and #{EtcItems.Count} etc items.");
            Initialised = true;
        }

        public L2Item CreateItem(int itemId, int count, L2Player actor)
        {
            L2Item item = new L2Item(_itemCrudService, _idFactory, GetItem(itemId), _idFactory.NextId());

            L2World.Instance.AddObject(item);

            if (item.Template.Stackable && (count > 1))
            {
                item.Count = count;
            }

            return item;
        }

        private async Task LoadArmorModels()
        {
            IEnumerable<ArmorContract> armorContracts = await _armorCrudService.GetAll();

            Armors = armorContracts.Select(x => new Armor(new StatsSet())
                    {
                        Type = x.ArmorType,
                        ItemId = x.ArmorId,
                        Name = x.Name,
                        BodyPart = x.BodyPart,
                        Sellable = x.Sellable,
                        Dropable = x.Dropable,
                        Destroyable = x.Destroyable,
                        Tradable = x.Tradeable,
                        Weight = x.Weight,
                        Duration = x.Duration
                    })
                .ToDictionary(x => x.ItemId);
        }

        private async Task LoadEtcItemModels()
        {
            IEnumerable<EtcItemContract> etcItemContracts = await _etcItemCrudService.GetAll();

            EtcItems = etcItemContracts.Select(x => new EtcItem(new StatsSet())
                    {
                        Type = x.ItemType,
                        ItemId = x.EtcItemId,
                        Name = x.Name,
                        Sellable = x.Sellable,
                        Dropable = x.Dropable,
                        Destroyable = x.Destroyable,
                        Tradable = x.Tradeable,
                        Weight = x.Weight,
                        Duration = x.Duration
                    })
                .ToDictionary(x => x.ItemId);
        }

        private async Task LoadWeaponModels()
        {
            IEnumerable<WeaponContract> weaponContracts = await _weaponCrudService.GetAll();

            Weapons = weaponContracts.Select(x => new Weapon(new StatsSet())
                    {
                        Type = x.WeaponType,
                        ItemId = x.WeaponId,
                        Name = x.Name,
                        BodyPart = x.BodyPart,
                        Sellable = x.Sellable,
                        Dropable = x.Dropable,
                        Destroyable = x.Destroyable,
                        Tradable = x.Tradeable,
                        Weight = x.Weight,
                        Duration = x.Duration,
                        ReferencePrice = x.Price,
                        SpiritshotCount = x.Spiritshots,
                        SoulshotCount = x.Soulshots,
                        PDam = x.Pdam,
                        RndDam = x.RndDam,
                        Critical = x.Critical,
                        HitModifier = x.HitModify,
                        AvoidModifier = x.AvoidModify,
                        ShieldDef = x.ShieldDef,
                        ShieldDefRate = x.ShieldDefRate,
                        AtkSpeed = x.AtkSpeed,
                        MpConsume = x.MpConsume,
                        MDam = x.Mdam
                    })
                .ToDictionary(x => x.ItemId);
        }
    }
}