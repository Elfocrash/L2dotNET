using L2dotNET.Game.Enums;
using L2dotNET.Game.templates;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace L2dotNET.Game.tables
{
    sealed class CharTemplateTable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CharTemplateTable));

        private static volatile CharTemplateTable instance;
        private static object syncRoot = new object();

        private Dictionary<int, PcTemplate> templates = new Dictionary<int, PcTemplate>();
        public Dictionary<int, PcTemplate> Templates { get { return templates; } }
        public static CharTemplateTable Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new CharTemplateTable();
                        }
                    }
                }

                return instance;
            }
        }

        public CharTemplateTable()
        {

        }

        public void Initialize()
        {
            XmlDocument doc = new XmlDocument();
            string[] xmlFilesArray = Directory.GetFiles(@"data\xml\classes\");
            for (int i = 0; i < xmlFilesArray.Length; i++)
            {
                doc.Load(xmlFilesArray[i]);
                XmlNodeList nodes = doc.DocumentElement.SelectNodes("/list/class");

                foreach (XmlNode node in nodes)
                {

                    if ("class".Equals(node.Attributes[0].OwnerElement.Name))
                    {
                        XmlNamedNodeMap attrs = node.Attributes;
                        ClassId classId = ClassId.Values.FirstOrDefault(x => ((int)x.Id).Equals(Convert.ToInt32(attrs.Item(0).Value)));
                        StatsSet set = new StatsSet();

                        for (XmlNode cd = node.FirstChild; cd != null; cd = cd.NextSibling)
                        {

                            if ("set".Equals(cd.NextSibling.Name) && cd.NextSibling != null)
                            {
                                attrs = cd.NextSibling.Attributes;
                                string name = attrs.GetNamedItem("name").Value;
                                string value = attrs.GetNamedItem("val").Value;
                                set.Set(name, value);
                            }
                            else
                                break;
                        }
                        PcTemplate pcTempl = new PcTemplate(classId, set);
                        templates.Add((int)pcTempl.ClassId.Id, pcTempl);
                        
                    }

                }


            }
            Console.WriteLine($"CharTemplateTable: Loaded { templates.Count } character templates.");
        }

        public PcTemplate GetTemplate(ClassIds classId)
        {
            return templates[(int)classId];
        }

        public PcTemplate GetTemplate(int classId)
        {
            return templates[classId];
        }

    }
}
