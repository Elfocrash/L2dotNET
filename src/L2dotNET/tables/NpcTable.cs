using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using log4net;
using L2dotNET.templates;
using L2dotNET.Utility;

namespace L2dotNET.tables
{
    class NpcTable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(NpcTable));
        private static volatile NpcTable _instance;
        private static readonly object SyncRoot = new object();

        private readonly Dictionary<int, NpcTemplate> _npcs = new Dictionary<int, NpcTemplate>();

        public static NpcTable Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new NpcTable();
                }

                return _instance;
            }
        }

        public NpcTemplate GetTemplate(int id)
        {
            return _npcs[id];
        }

        public NpcTemplate GetTemplateByName(string name)
        {
            return _npcs.Values.FirstOrDefault(npcTemplate => npcTemplate.Name.EqualsIgnoreCase(name));
        }

        public List<NpcTemplate> GetAllNpcs()
        {
            return _npcs.Values.ToList();
        }

        public void Initialize()
        {
            XmlDocument doc = new XmlDocument();
            string[] xmlFilesArray = Directory.GetFiles(@"data\xml\npcs\");
            try
            {
                StatsSet set = new StatsSet();
                //StatsSet petSet = new StatsSet();

                foreach (string i in xmlFilesArray)
                {
                    doc.Load(i);

                    XmlNodeList nodes = doc.DocumentElement?.SelectNodes("/list/npc");

                    if (nodes == null)
                        continue;

                    foreach (XmlNode node in nodes)
                    {
                        XmlElement ownerElement = node.Attributes?[0].OwnerElement;
                        if ((ownerElement != null) && (node.Attributes != null) && "npc".Equals(ownerElement.Name))
                        {
                            XmlNamedNodeMap attrs = node.Attributes;

                            int npcId = int.Parse(attrs.GetNamedItem("id").Value);
                            int templateId = attrs.GetNamedItem("idTemplate") == null ? npcId : int.Parse(attrs.GetNamedItem("idTemplate").Value);

                            set.Set("id", npcId);
                            set.Set("idTemplate", templateId);
                            set.Set("name", attrs.GetNamedItem("name").Value);
                            set.Set("title", attrs.GetNamedItem("title").Value);

                            _npcs.Add(npcId, new NpcTemplate(set));
                        }
                        set.Clear();
                    }
                }

                Log.Info($"Loaded {_npcs.Count} npcs.");
            }
            catch (Exception e)
            {
                Log.Error("NpcTable: Error parsing NPC templates : ", e);
            }
        }
    }
}