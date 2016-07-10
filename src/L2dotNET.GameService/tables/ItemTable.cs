using System;
using System.Collections.Generic;
using log4net;
using L2dotNET.GameService.Model.Items;

namespace L2dotNET.GameService.Tables
{
    class ItemTable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ItemTable));

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
                {
                    return _instance;
                }

                lock (SyncRoot)
                {
                    if (_instance == null)
                    {
                        _instance = new ItemTable();
                    }
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
            Slots.Add("hairall", ItemTemplate.SlotHairall);
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
            Slots.Add("rear;lear", ItemTemplate.SlotREar | ItemTemplate.SlotLEar);
            Slots.Add("rfinger;lfinger", ItemTemplate.SlotRFinger | ItemTemplate.SlotLFinger);
            Slots.Add("none", ItemTemplate.SlotNone);
            Slots.Add("wolf", ItemTemplate.SlotWolf);
            Slots.Add("hatchling", ItemTemplate.SlotHatchling);
            Slots.Add("strider", ItemTemplate.SlotStrider);
            Slots.Add("babypet", ItemTemplate.SlotBabypet);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            Log.Info($"ItemTable: #{Armors.Count} armors.");
        }
    }
}