using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using L2dotNET.GameService.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace L2dotNET.Tests
{
    [TestClass]
    public class MapRegionTest
    {
        [TestMethod]
        public void Test()
        {
            Dictionary<int, NpcTemplate> _npcs = new Dictionary<int, NpcTemplate>();
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

                            _npcs.Add(npcId, new NpcTemplate(set));
                        }
                        set.Clear();
                    }
                }
            }
            catch (Exception e)
            {
                //_log.log(Level.SEVERE, "NpcTable: Error parsing NPC templates : ", e);
            }
        }
    }
}