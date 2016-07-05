using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using log4net;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Templates;

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

        public static ItemTable Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new ItemTable();
                        }
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

            int highest = 0;
            XmlDocument doc = new XmlDocument();
            string[] xmlFilesArray = Directory.GetFiles(@"data\xml\items\");
            NewItem currentItem;

            List<ItemTemplate> itemsInFile = new List<ItemTemplate>();

            foreach (string i in xmlFilesArray)
            {
                doc.Load(i);
                XmlNodeList nodes = doc.DocumentElement?.SelectNodes("/list/item");
                if (nodes != null)
                {
                    foreach (XmlNode node in nodes)
                    {
                        XmlElement ownerElement = node.Attributes?[0].OwnerElement;
                        if ((ownerElement != null) && ((node.Attributes != null) && "item".Equals(ownerElement.Name)))
                        {
                            XmlNamedNodeMap attrs = node.Attributes;
                            currentItem = new NewItem();
                            int itemId = int.Parse(attrs.GetNamedItem("id").Value);
                            string className = attrs.GetNamedItem("type").Value;
                            string itemName = attrs.GetNamedItem("name").Value;

                            currentItem.Id = itemId;
                            currentItem.Name = itemName;
                            currentItem.Type = className;
                            currentItem.Set = new StatsSet();
                            currentItem.Set.Set("item_id", itemId);
                            currentItem.Set.Set("name", itemName);

                            //itemsInFile.Add(currentItem.Item);
                        }
                    }
                }
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            Log.Info($"ItemTable: #{Armors.Count} armors.");
        }
    }

    public class NewItem
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public StatsSet Set { get; set; }
        public int CurrentLevel { get; set; }
        public ItemTemplate Item { get; set; }
    }
}