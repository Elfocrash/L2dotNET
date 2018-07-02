using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Enums;
using L2dotNET.Templates;
using NLog;

namespace L2dotNET.Tables
{
    public static class CharTemplateTable
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static Dictionary<int, PcTemplate> _templates;

        public static void Initialize()
        {
            _templates = new Dictionary<int, PcTemplate>();

            XmlDocument doc = new XmlDocument();
            string[] xmlFilesArray = Directory.GetFiles(@"data\xml\classes\");
            foreach (string i in xmlFilesArray)
            {
                doc.Load(i);

                XmlNodeList nodes = doc.DocumentElement?.SelectNodes("/list/class");

                if (nodes == null)
                {
                    continue;
                }

                foreach (XmlNode node in nodes)
                {
                    XmlElement ownerElement = node.Attributes?[0].OwnerElement;
                    if (ownerElement == null || node.Attributes == null || ownerElement.Name != "class")
                    {
                        continue;
                    }

                    XmlNamedNodeMap attrs = node.Attributes;
                    ClassId classId = ClassId.Values.FirstOrDefault(x => (int)x.Id == Convert.ToInt32(attrs.Item(0).Value));
                    StatsSet set = new StatsSet();

                    for (XmlNode cd = node.FirstChild; cd?.NextSibling != null; cd = cd.NextSibling)
                    {
                        if (cd.NextSibling.Name == "set")
                        {
                            attrs = cd.NextSibling.Attributes;

                            if (attrs == null)
                            {
                                continue;
                            }

                            string name = attrs.GetNamedItem("name").Value;
                            string value = attrs.GetNamedItem("val").Value;

                            set.Set(name, value);
                        }
                        else if (cd.NextSibling.Name == "items")
                        {
                            attrs = cd.NextSibling.Attributes;

                            if (attrs == null)
                            {
                                continue;
                            }

                            string value = attrs.GetNamedItem("val").Value;

                            set.Set("items", value);
                        }
                    }

                    PcTemplate pcTempl = new PcTemplate(classId, set);
                    _templates.Add((int)pcTempl.ClassId.Id, pcTempl);
                }
            }

            Log.Info($"Loaded {_templates.Count} character templates.");
        }

        public static PcTemplate GetTemplate(ClassIds classId)
        {
            return _templates[(int)classId];
        }

        public static PcTemplate GetTemplate(int classId)
        {
            return _templates[classId];
        }

        public static List<PcTemplate> GetTemplates()
        {
            return _templates.Values.ToList();
        }
    }
}