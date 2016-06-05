using System.Collections.Generic;
using System.IO;
using System.Xml;
using L2dotNET.GameService.Enums;
using L2dotNET.GameService.templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace L2dotNET.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private Dictionary<int, PcTemplate> templates = new Dictionary<int, PcTemplate>();

        [TestMethod]
        public void TestMethod1()
        {
            string folderPath = @"./data/xml/classes";
            foreach (string file in Directory.EnumerateFiles(folderPath, "*.xml"))
            {
                string contents = File.ReadAllText(file);
            }

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
                        ClassIds classId = (ClassIds)int.Parse(attrs.Item(0).Value);
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
                        ////PcTemplate pcTempl = new PcTemplate(classId, set);
                        ////templates.Add((int)pcTempl.ClassId, pcTempl);
                        //System.Diagnostics.Trace.WriteLine("Added template for: " + pcTempl.ClassId);
                    }
                }
            }
        }
    }
}