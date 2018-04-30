using System;
using System.Collections.Generic;
using log4net;
using L2dotNET.DataContracts;
using L2dotNET.Enums;
using L2dotNET.Models.Items;
using L2dotNET.Models.Player;
using L2dotNET.Services.Contracts;
using L2dotNET.Templates;
using L2dotNET.Utility;
using L2dotNET.World;

namespace L2dotNET.Tables
{
    public class ItemTable : IInitialisable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ItemTable));

        private readonly IItemService _itemService;
        private readonly IdFactory _idFactory;
        public bool Initialised { get; private set; }

        public ItemTable(IItemService itemService, IdFactory idFactory)
        {
            _itemService = itemService;
            _idFactory = idFactory;
        }

        internal ItemTemplate GetItem(int id)
        {
            if (Armors.ContainsKey(id))
                return Armors[id];

            if (Weapons.ContainsKey(id))
                return Weapons[id];

            if (EtcItems.ContainsKey(id))
                return EtcItems[id];

            return null;
        }

        public Dictionary<string, int> Slots = new Dictionary<string, int>();
        public Dictionary<int, Armor> Armors = new Dictionary<int, Armor>();
        public Dictionary<int, Weapon> Weapons = new Dictionary<int, Weapon>();
        public Dictionary<int, EtcItem> EtcItems = new Dictionary<int, EtcItem>();

        public void Initialise()
        {
            if (Initialised)
                return;

            Slots = ItemSlots.ToDictionary();
            LoadArmorModels();
            LoadWeaponModels();
            LoadEtcItemModels();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            Log.Info($"Loaded #{Armors.Count} armors, #{Weapons.Count} weapons and #{EtcItems.Count} etc items.");
            Initialised = true;
        }

        public L2Item CreateItem(int itemId, int count, L2Player actor)
        {
            L2Item item = new L2Item(_itemService, _idFactory, GetItem(itemId), _idFactory.NextId());

            L2World.Instance.AddObject(item);

            if (item.Template.Stackable && (count > 1))
                item.Count = count;

            return item;
        }

        private void LoadArmorModels()
        {
            Dictionary<int, ArmorContract> armorsModels = _itemService.GetAllArmorModelsDict();
            foreach (KeyValuePair<int, ArmorContract> modelPair in armorsModels)
            {
                StatsSet set = new StatsSet();
                ArmorContract contract = modelPair.Value;
                Armor armor = new Armor(set)
                {
                    Type = Utilz.GetEnumFromString(contract.ArmorType, ArmorTypeId.None),
                    ItemId = contract.ItemId,
                    Name = contract.Name,
                    BodyPart = Slots[contract.BodyPart],
                    Sellable = contract.Sellable,
                    Dropable = contract.Dropable,
                    Destroyable = contract.Destroyable,
                    Tradable = contract.Tradeable,
                    Weight = contract.Weight,
                    Duration = contract.Duration
                };
                Armors.Add(modelPair.Key, armor);
            }
        }

        private void LoadEtcItemModels()
        {
            Dictionary<int, EtcItemContract> etcItemModels = _itemService.GetAllEtcItemModelsDict();
            foreach (KeyValuePair<int, EtcItemContract> modelPair in etcItemModels)
            {
                StatsSet set = new StatsSet();
                EtcItemContract contract = modelPair.Value;
                EtcItem etcItem = new EtcItem(set)
                {
                    Type = Utilz.GetEnumFromString(contract.ItemType, EtcItemTypeId.None),
                    ItemId = contract.ItemId,
                    Name = contract.Name,
                    Sellable = contract.Sellable,
                    Dropable = contract.Dropable,
                    Destroyable = contract.Destroyable,
                    Tradable = contract.Tradeable,
                    Weight = contract.Weight,
                    Duration = contract.Duration
                };
                EtcItems.Add(modelPair.Key, etcItem);
            }
        }

        private void LoadWeaponModels()
        {
            Dictionary<int, WeaponContract> weaponModels = _itemService.GetAllWeaponModelsDict();
            foreach (KeyValuePair<int, WeaponContract> modelPair in weaponModels)
            {
                StatsSet set = new StatsSet();
                WeaponContract contract = modelPair.Value;
                Weapon weapon = new Weapon(set)
                {
                    Type = Utilz.GetEnumFromString(contract.WeaponType, WeaponTypeId.None),
                    ItemId = contract.ItemId,
                    Name = contract.Name,
                    BodyPart = Slots[contract.BodyPart],
                    Sellable = contract.Sellable,
                    Dropable = contract.Dropable,
                    Destroyable = contract.Destroyable,
                    Tradable = contract.Tradeable,
                    Weight = contract.Weight,
                    Duration = contract.Duration,
                    ReferencePrice = contract.Price,
                    SpiritshotCount = contract.Spiritshots,
                    SoulshotCount = contract.Soulshots,
                    PDam = contract.Pdam,
                    RndDam = contract.RndDam,
                    Critical = contract.Critical,
                    HitModifier = contract.HitModify,
                    AvoidModifier = contract.AvoidModify,
                    ShieldDef = contract.ShieldDef,
                    ShieldDefRate = contract.ShieldDefRate,
                    AtkSpeed = contract.AtkSpeed,
                    MpConsume = contract.MpConsume,
                    MDam = contract.Mdam
                };
                Weapons.Add(modelPair.Key, weapon);
            }
        }
    }
}