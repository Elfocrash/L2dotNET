using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using L2dotNET.Templates;
using L2dotNET.Utility;
using NLog;

namespace L2dotNET.Tables
{
    static class NpcTable
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static readonly Dictionary<int, NpcTemplate> _npcs = new Dictionary<int, NpcTemplate>();

        public static void Initialize()
        {
            XmlDocument doc = new XmlDocument();
            string[] xmlFilesArray = Directory.GetFiles(@"data\xml\npcs\");
            try
            {
                StatsSet set = new StatsSet();

                foreach (string i in xmlFilesArray)
                {
                    doc.Load(i);

                    XmlNodeList nodes = doc.DocumentElement?.SelectNodes("/list/npc");

                    if (nodes == null)
                    {
                        continue;
                    }

                    foreach (XmlNode node in nodes)
                    {
                        XmlElement ownerElement = node.Attributes?[0].OwnerElement;
                        if (ownerElement != null && node.Attributes != null && ownerElement.Name == "npc")
                        {
                            XmlNamedNodeMap attrs = node.Attributes;
                            
                            int npcId = int.Parse(attrs.GetNamedItem("id").Value);
                            int templateId = attrs.GetNamedItem("idTemplate") == null ? npcId : int.Parse(attrs.GetNamedItem("idTemplate").Value);

                            set.Set("id", npcId);
                            set.Set("idTemplate", templateId);
                            set.Set("name", attrs.GetNamedItem("name").Value);
                            set.Set("title", attrs.GetNamedItem("title").Value);

                            foreach(XmlNode innerData in node.ChildNodes)
                            {
                                if(innerData.Attributes["name"] != null && innerData.Attributes["val"] != null)
                                {
                                    string value = innerData.Attributes["val"].Value;
                                    string name = innerData.Attributes["name"].Value;

                                    set.Set(name, value);
                                }
                            }

                            _npcs.Add(npcId, new NpcTemplate(set));
                        }
                        set.Clear();
                    }
                }

                Log.Info($"Loaded {_npcs.Count} npcs.");
            }
            catch (Exception e)
            {
                Log.Error(e, "Error parsing NPC templates: ");
            }
        }

        public static NpcTemplate GetTemplate(int id)
        {
            return _npcs[id];
        }

        public static NpcTemplate GetTemplateByName(string name)
        {
            return _npcs.Values.FirstOrDefault(npcTemplate => npcTemplate.Name.EqualsIgnoreCase(name));
        }

        public static List<NpcTemplate> GetAllNpcs()
        {
            return _npcs.Values.ToList();
        }
    }
}