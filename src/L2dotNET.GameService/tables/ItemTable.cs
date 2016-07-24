using System;
using System.Collections.Generic;
using log4net;
using L2dotNET.Enums;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Templates;
using L2dotNET.Models;
using L2dotNET.Services.Contracts;
using L2dotNET.Utility;
using Ninject;

namespace L2dotNET.GameService.Tables
{
    public class ItemTable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ItemTable));

        [Inject]
        public IItemService ItemService => GameServer.Kernel.Get<IItemService>();

        internal ItemTemplate GetItem(int id)
        {
            return null;
        }

        private static volatile ItemTable _instance;
        private static readonly object SyncRoot = new object();
        public Dictionary<string, int> Slots = new Dictionary<string, int>();
        public Dictionary<int, Armor> Armors = new Dictionary<int, Armor>();
        //public Dictionary<int, Weapon> Weapons = new Dictionary<int, Weapon>();
        //public Dictionary<int, EtcItem> Armors = new Dictionary<int, Armor>();

        public static ItemTable Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new ItemTable();
                }

                return _instance;
            }
        }

        public void Initialize()
        {
            Slots.Add("chest", ItemTemplate.SlotChest);
            Slots.Add("fullarmor", ItemTemplate.SlotFullArmor);
            Slots.Add("alldress", ItemTemplate.SlotAlldress);
            Slots.Add("head", ItemTemplate.SlotHead);
            Slots.Add("hair", ItemTemplate.SlotHair);
            Slots.Add("face", ItemTemplate.SlotFace);
            Slots.Add("dhair", ItemTemplate.SlotHairall);
            Slots.Add("underwear", ItemTemplate.SlotUnderwear);
            Slots.Add("back", ItemTemplate.SlotBack);
            Slots.Add("neck", ItemTemplate.SlotNeck);
            Slots.Add("legs", ItemTemplate.SlotLegs);
            Slots.Add("feet", ItemTemplate.SlotFeet);
            Slots.Add("gloves", ItemTemplate.SlotGloves);
            Slots.Add("chest,legs", ItemTemplate.SlotChest | ItemTemplate.SlotLegs);
            Slots.Add("rhand", ItemTemplate.SlotRHand);
            Slots.Add("lhand", ItemTemplate.SlotLHand);
            Slots.Add("lrhand", ItemTemplate.SlotLrHand);
            Slots.Add("rear,lear", ItemTemplate.SlotREar | ItemTemplate.SlotLEar);
            Slots.Add("rfinger,lfinger", ItemTemplate.SlotRFinger | ItemTemplate.SlotLFinger);
            Slots.Add("none", ItemTemplate.SlotNone);
            Slots.Add("wolf", ItemTemplate.SlotWolf);
            Slots.Add("hatchling", ItemTemplate.SlotHatchling);
            Slots.Add("strider", ItemTemplate.SlotStrider);
            Slots.Add("babypet", ItemTemplate.SlotBabypet);

            Dictionary<int, ArmorModel> armorsModels = ItemService.GetAllArmorModelsDict();
            foreach (KeyValuePair<int, ArmorModel> modelPair in armorsModels)
            {
                StatsSet set = new StatsSet();
                ArmorModel model = modelPair.Value;
                Armor armor = new Armor(set)
                {
                    Type = Utilz.GetEnumFromString(model.ArmorType, ArmorTypeId.None),
                    ItemId = model.ItemId,
                    Name = model.Name,
                    BodyPart = Slots[model.BodyPart],
                    Sellable = model.Sellable,
                    Dropable = model.Dropable,
                    Destroyable = model.Destroyable,
                    Tradable = model.Tradeable,
                    Weight = model.Weight,
                    Duration = model.Duration                    
                };
                Armors.Add(modelPair.Key, armor);
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            Log.Info($"ItemTable: Loaded #{Armors.Count} armors.");
        }
    }
}