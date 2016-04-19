using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using L2dotNET.Game.logger;
using L2dotNET.Game.model.items;
using L2dotNET.Game.model.player;
using L2dotNET.Game.staticf;

namespace L2dotNET.Game.tables
{
    class PClassess
    {
        private static PClassess pcs = new PClassess();
        public static PClassess getInstance()
        {
            return pcs;
        }

        public SortedList<byte, PlayerTemplate> _templates = new SortedList<byte, PlayerTemplate>();

        public PlayerTemplate getTemplate(byte id)
        {
            if (!_templates.ContainsKey(id))
            {
                CLogger.error("class template for " + id + " was not found!");
                return null;
            }

            return _templates[id];
        }

        public PClassess()
        {
            XElement xml = XElement.Parse(File.ReadAllText(@"scripts\classes.xml"));
            XElement ex = xml.Element("list");
            foreach (var m in ex.Elements())
            {
                if (m.Name == "class")
                {
                    PlayerTemplate template = new PlayerTemplate();

                    template.id = byte.Parse(m.Attribute("id").Value);
                    template.pch = (ClassId)Enum.Parse(typeof(ClassId), m.Attribute("pch").Value);

                    switch (m.Attribute("race").Value)
                    {
                        case "human":
                            template.race = 0;
                            break;
                        case "elf":
                            template.race = 1;
                            break;
                        case "darkelf":
                            template.race = 2;
                            break;
                        case "orc":
                            template.race = 3;
                            break;
                        case "dwarf":
                            template.race = 4;
                            break;
                        case "kamael":
                            template.race = 5;
                            break;
                    }

                    template.level = byte.Parse(m.Attribute("level").Value);
                    template.transfer = byte.Parse(m.Attribute("transfer").Value);

                    foreach (var stp in m.Elements())
                    {
                        if (stp.Name == "stats")
                        {
                            template.patk = int.Parse(stp.Attribute("patk").Value);
                            template.pdef = int.Parse(stp.Attribute("pdef").Value);
                            template.atkrange = int.Parse(stp.Attribute("atkrange").Value);
                            template.rndmg = int.Parse(stp.Attribute("rndmg").Value);
                            template.matk = int.Parse(stp.Attribute("matk").Value);
                            template.mdef = int.Parse(stp.Attribute("mdef").Value);
                            template.critical = int.Parse(stp.Attribute("critical").Value);
                            template.atkspd = int.Parse(stp.Attribute("atkspd").Value);
                            template.runspd = int.Parse(stp.Attribute("runspd").Value);
                            template.walkspd = int.Parse(stp.Attribute("walkspd").Value);
                            template.waterspd = int.Parse(stp.Attribute("waterspd").Value);
                            template.collr_f = double.Parse(stp.Attribute("collr_f").Value);
                            template.collh_f = double.Parse(stp.Attribute("collh_f").Value);
                            template.collr_m = double.Parse(stp.Attribute("collr_f").Value);
                            template.collh_m = double.Parse(stp.Attribute("collh_m").Value);
                            template.fall_f = int.Parse(stp.Attribute("fall_f").Value);
                            template.fall_m = int.Parse(stp.Attribute("fall_m").Value);
                            template.breath = int.Parse(stp.Attribute("breath").Value);
                            template._int = byte.Parse(stp.Attribute("int").Value);
                            template._str = byte.Parse(stp.Attribute("str").Value);
                            template._con = byte.Parse(stp.Attribute("con").Value);
                            template._men = byte.Parse(stp.Attribute("men").Value);
                            template._dex = byte.Parse(stp.Attribute("dex").Value);
                            template._wit = byte.Parse(stp.Attribute("wit").Value);
                        }
                        else if (stp.Name == "regen")
                        {
                            template._regHp = new double[9];
                            string[] hpa = stp.Attribute("hp").Value.Split(' ');
                            for (int i = 0; i < template._regHp.Length; i++)
                            {
                                template._regHp[i] = double.Parse(hpa[i]);
                            }

                            template._regMp = new double[9];
                            string[] mpa = stp.Attribute("mp").Value.Split(' ');
                            for (int i = 0; i < template._regMp.Length; i++)
                            {
                                template._regMp[i] = double.Parse(mpa[i]);
                            }
                        }
                        else if (stp.Name == "datavals")
                        {
                            byte ah = 1, am = 1, ac = 1;
                            foreach (var dvp in stp.Elements())
                            {
                                switch (dvp.Name.ToString())
                                {
                                    case "hp":
                                        {
                                            string[] vals = dvp.Value.Split(' ');
                                            template._hp = new double[vals.Length + 2];
                                            template._hp[0] = 20.0;
                                            foreach (string val in vals)
                                            {
                                                template._hp[ah] = double.Parse(val);
                                                ah++;
                                            }
                                        }
                                        break;
                                    case "mp":
                                        {
                                            string[] vals = dvp.Value.Split(' ');
                                            template._mp = new double[vals.Length + 2];
                                            template._mp[0] = 20.0;
                                            foreach (string val in vals)
                                            {
                                                template._mp[am] = double.Parse(val);
                                                am++;
                                            }
                                        }
                                        break;
                                    case "cp":
                                        {
                                            string[] vals = dvp.Value.Split(' ');
                                            template._cp = new double[vals.Length + 2];
                                            template._cp[0] = 20.0;
                                            foreach (string val in vals)
                                            {
                                                template._cp[ac] = double.Parse(val);
                                                ac++;
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                        else if (stp.Name == "items")
                        {
                            template._items = new List<PC_item>();
                            foreach (var itemp in stp.Elements())
                            {
                                ItemTemplate item = ItemTable.getInstance().getItem(Convert.ToInt32(itemp.Attribute("id").Value));
                                if (item != null)
                                {
                                    PC_item i = new PC_item();
                                    i.item = item;
                                    if (itemp.Attribute("count") != null)
                                        i.count = long.Parse(itemp.Attribute("count").Value);

                                    if (itemp.Attribute("equip") != null)
                                        i.equip = byte.Parse(itemp.Attribute("equip").Value) == 1;

                                    if (itemp.Attribute("enchant") != null)
                                        i.enchant = short.Parse(itemp.Attribute("enchant").Value);

                                    if (itemp.Attribute("lifetime") != null)
                                        i.lifetime = int.Parse(itemp.Attribute("lifetime").Value);

                                    template._items.Add(i);
                                }
                                else
                                {
                                    CLogger.error("start equipment for " + template.pch + " was not found!");
                                    continue;
                                }
                            }
                        }
                    }

                    if (_templates.ContainsKey(template.id))
                    {
                        CLogger.error("class id duplicate " + template.id);
                    }

                    _templates.Add(template.id, template);
                }
            }

            CLogger.info("Templates: loaded " + _templates.Count + " player templates.");

        }
    }

    public class PC_item
    {
        public ItemTemplate item;
        public long count = 1;
        public bool equip = false;
        public short enchant = 0;
        public int lifetime = -1;
    }
}
