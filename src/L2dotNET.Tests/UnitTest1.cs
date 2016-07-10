using System.Collections.Generic;
using System.IO;
using System.Xml;
using L2dotNET.GameService.Templates;
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
            //const string folderPath = @"./data/xml/classes";
            //foreach (string file in Directory.EnumerateFiles(folderPath, "*.xml"))
            //{
            //    //string contents = File.ReadAllText(file);
            //}

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
                    if ((ownerElement == null) || !"class".Equals(ownerElement.Name))
                    {
                        continue;
                    }

                    //ClassIds classId = (ClassIds)int.Parse(attrs.Item(0).Value);
                    StatsSet set = new StatsSet();

                    for (XmlNode cd = node.FirstChild; cd != null; cd = cd.NextSibling)
                        if ((cd.NextSibling != null) && "set".Equals(cd.NextSibling.Name) && (cd.NextSibling != null))
                        {
                            XmlNamedNodeMap attrs = cd.NextSibling.Attributes;
                            if (attrs == null)
                            {
                                continue;
                            }

                            string name = attrs.GetNamedItem("name").Value;
                            string value = attrs.GetNamedItem("val").Value;
                            set.Set(name, value);
                        }
                        else
                        {
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