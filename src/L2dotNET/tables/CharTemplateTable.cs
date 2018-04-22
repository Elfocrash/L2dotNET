using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using log4net;
using L2dotNET.Enums;
using L2dotNET.Templates;

namespace L2dotNET.Tables
{
    sealed class CharTemplateTable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CharTemplateTable));

        private static volatile CharTemplateTable _instance;
        private static readonly object SyncRoot = new object();

        public Dictionary<int, PcTemplate> Templates { get; } = new Dictionary<int, PcTemplate>();

        public static CharTemplateTable Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new CharTemplateTable();
                }

                return _instance;
            }
        }

        public void Initialize()
        {
            XmlDocument doc = new XmlDocument();
            string[] xmlFilesArray = Directory.GetFiles(@"data\xml\classes\");
            foreach (string i in xmlFilesArray)
            {
                doc.Load(i);

                XmlNodeList nodes = doc.DocumentElement?.SelectNodes("/list/class");

                if (nodes == null)
                    continue;

                foreach (XmlNode node in nodes)
                {
                    XmlElement ownerElement = node.Attributes?[0].OwnerElement;
                    if ((ownerElement == null) || (node.Attributes == null) || !"class".Equals(ownerElement.Name))
                        continue;

                    XmlNamedNodeMap attrs = node.Attributes;
                    ClassId classId = ClassId.Values.FirstOrDefault(x => ((int)x.Id).Equals(Convert.ToInt32(attrs.Item(0).Value)));
                    StatsSet set = new StatsSet();

                    for (XmlNode cd = node.FirstChild; cd != null; cd = cd.NextSibling)
                        if ((cd.NextSibling != null) && "set".Equals(cd.NextSibling.Name) && (cd.NextSibling != null))
                        {
                            attrs = cd.NextSibling.Attributes;
                            if (attrs == null)
                                continue;

                            string name = attrs.GetNamedItem("name").Value;
                            string value = attrs.GetNamedItem("val").Value;
                            set.Set(name, value);
                        }
                        else
                            break;

                    PcTemplate pcTempl = new PcTemplate(classId, set);
                    Templates.Add((int)pcTempl.ClassId.Id, pcTempl);
                }
            }

            Log.Info($"Loaded {Templates.Count} character templates.");
        }

        public PcTemplate GetTemplate(ClassIds classId)
        {
            return Templates[(int)classId];
        }

        public PcTemplate GetTemplate(int classId)
        {
            return Templates[classId];
        }
    }
}