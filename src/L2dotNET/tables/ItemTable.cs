using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using L2dotNET.DataContracts;
using L2dotNET.Models.Items;
using L2dotNET.Models.Player;
using L2dotNET.Services.Contracts;
using L2dotNET.Templates;
using L2dotNET.World;
using Mapster;
using NLog;

namespace L2dotNET.Tables
{
    public class ItemTable : IInitialisable
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

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

            Log.Info($"Loaded #{Armors.Count} armors, #{Weapons.Count} weapons and #{EtcItems.Count} etc items.");
            Initialised = true;
        }

        public L2Item CreateItem(int itemId, int count, L2Player actor)
        {
            L2Item item = new L2Item(_itemCrudService, _idFactory, GetItem(itemId), _idFactory.NextId());

            L2World.AddObject(item);

            if (item.Template.Stackable && count > 1)
            {
                item.Count = count;
            }

            return item;
        }

        private async Task LoadArmorModels()
        {
            IEnumerable<ArmorContract> armorContracts = await _armorCrudService.GetAll();

            Armors = armorContracts.AsQueryable()
                .ProjectToType<Armor>()
                .ToDictionary(x => x.ItemId);
        }

        private async Task LoadEtcItemModels()
        {
            IEnumerable<EtcItemContract> etcItemContracts = await _etcItemCrudService.GetAll();

            EtcItems = etcItemContracts.AsQueryable()
                .ProjectToType<EtcItem>()
                .ToDictionary(x => x.ItemId);
        }

        private async Task LoadWeaponModels()
        {
            IEnumerable<WeaponContract> weaponContracts = await _weaponCrudService.GetAll();
            
            Weapons = weaponContracts.AsQueryable()
                .ProjectToType<Weapon>()
                .ToDictionary(x => x.ItemId);
        }
    }
}