using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace L2dotNET.Game.staticf
{
    public class pc_parameter
    {
        public string classn { get; set; }

        public int base_physical_attack;

        public void type(string current, string p)
        {
            switch (current)
            {
                case "base_race_id":
                    base_race_id = int.Parse(p); break;
                case "base_class_id":
                    base_class_id = int.Parse(p); break;
                case "base_physical_attack":
                    base_physical_attack = int.Parse(p); break;

            }

         //   Console.WriteLine("processed " + current + " " + p + " for class " + classn);
        }

        public int base_race_id { get; set; }

        public int base_class_id { get; set; }

        public uint baseSTR { get; set; }

        public uint baseDEX { get; set; }

        public uint baseCON { get; set; }

        public uint baseINT { get; set; }

        public uint baseWIT { get; set; }

        public uint baseMEN { get; set; }
    }
}
