using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using log4net;
using L2dotNET.GameService.Templates;

namespace L2dotNET.GameService.Tables
{
    class NpcTable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NpcTable));
        private static volatile NpcTable instance;
        private static readonly object syncRoot = new object();

        private readonly Dictionary<int, NpcTemplate> npcs = new Dictionary<int, NpcTemplate>();

        public static NpcTable Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new NpcTable();
                        }
                    }
                }

                return instance;
            }
        }

        public NpcTemplate GetTemplate(int id)
        {
            return npcs[id];
        }

        public NpcTemplate GetTemplateByName(string name)
        {
            foreach (NpcTemplate npcTemplate in npcs.Values)
            {
                if (npcTemplate.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    return npcTemplate;
            }
            return null;
        }

        public List<NpcTemplate> GetAllNpcs()
        {
            return npcs.Values.ToList();
        }

        public void Initialize()
        {
            XmlDocument doc = new XmlDocument();
            string[] xmlFilesArray = Directory.GetFiles(@"data\xml\npcs\");
            try
            {
                StatsSet set = new StatsSet();
                StatsSet petSet = new StatsSet();

                for (int i = 0; i < xmlFilesArray.Length; i++)
                {
                    doc.Load(xmlFilesArray[i]);
                    XmlNodeList nodes = doc.DocumentElement.SelectNodes("/list/npc");

                    foreach (XmlNode node in nodes)
                    {
                        if ("npc".Equals(node.Attributes[0].OwnerElement.Name))
                        {
                            XmlNamedNodeMap attrs = node.Attributes;

                            int npcId = int.Parse(attrs.GetNamedItem("id").Value);
                            int templateId = attrs.GetNamedItem("idTemplate") == null ? npcId : int.Parse(attrs.GetNamedItem("idTemplate").Value);

                            set.Set("id", npcId);
                            set.Set("idTemplate", templateId);
                            set.Set("name", attrs.GetNamedItem("name").Value);
                            set.Set("title", attrs.GetNamedItem("title").Value);

                            npcs.Add(npcId, new NpcTemplate(set));
                        }
                        set.Clear();
                    }
                }
                log.Info($"Loaded {npcs.Count} npcs.");
            }
            catch (Exception e)
            {
                log.Error("NpcTable: Error parsing NPC templates : ", e);
            }
        }
    }
}