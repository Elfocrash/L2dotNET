using System;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace L2dotNET.Tests
{
    [TestClass]
    public class MapRegionTest
    {
        private static readonly int REGIONS_X = 11;
        private static readonly int REGIONS_Y = 16;

        private static readonly int[,] _regions = new int[REGIONS_X, REGIONS_Y];

        [TestMethod]
        public void Test()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"data\xml\map_region.xml");
            XmlNodeList nodes = doc.DocumentElement.SelectNodes("/list/map");
            int count = 0;
            foreach (XmlNode node in nodes)
            {

                if (node.Attributes[0].OwnerElement.Name.Equals("map"))
                {
                    XmlNamedNodeMap attrs = node.Attributes;
                    //ClassId classId = ClassId.Values.FirstOrDefault(x => ((int)x.Id).Equals(Convert.ToInt32(attrs.Item(0).Value)));
                    int rY = Convert.ToInt32(attrs.GetNamedItem("geoY").Value) - 10;
                        for (int rX = 0; rX < REGIONS_X; rX++)
                        {
                            _regions[rX,rY] = Convert.ToInt32(attrs.GetNamedItem("geoX_" + (rX + 16)).Value);
                            count++;
                        }
                }

            }
        }
    }
}
