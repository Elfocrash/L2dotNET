using System.Collections.Generic;
using L2dotNET.GameService.Model.Npcs.Cubic.Data;
using L2dotNET.GameService.Model.Skills2;

namespace L2dotNET.GameService.Model.Npcs.Cubic
{
    public class CubicController
    {
        private static readonly CubicController instance = new CubicController();

        public static CubicController getController()
        {
            return instance;
        }

        public SortedList<int, CubicTemplate> cubics = new SortedList<int, CubicTemplate>();

        public CubicController()
        {
            //heal_cubic
            byte lv = 1;
            for (byte a = 1; a <= 16; a++)
            {
                if (lv == 8)
                    lv = 101;

                register(new heal_cubic(3, lv, 4051, a));
                lv++;
            }
            //nub heal
            register(new heal_cubic(3, 20, 4051, 20, 3600));

            register(new smart_cubic() { id = 10, delay = 10, max_count = 50, power = 2106, skill1 = TSkillTable.Instance.Get(4165, 9), skill1rate = 25, skill2 = TSkillTable.Instance.Get(4053, 8), skill2rate = 25, skill3 = TSkillTable.Instance.Get(5579, 1), skill3rate = 50, skill3target = "master" });

            register(new smart_cubic() { id = 11, delay = 10, max_count = 50, power = 2106, skill1 = TSkillTable.Instance.Get(5115, 4), skill1rate = 40, skill2 = TSkillTable.Instance.Get(4049, 8), skill2rate = 15, skill3 = TSkillTable.Instance.Get(5579, 1), skill3rate = 45, skill3target = "master" });

            register(new smart_cubic() { id = 12, delay = 13, max_count = 50, power = 2106, skill1 = TSkillTable.Instance.Get(4165, 9), skill1rate = 20, skill2 = TSkillTable.Instance.Get(4051, 1), skill2rate = 30, skill2target = "heal", skill3 = TSkillTable.Instance.Get(5579, 1), skill3rate = 50, skill3target = "master" });

            register(new smart_cubic() { id = 13, delay = 13, max_count = 50, power = 2106, skill1 = TSkillTable.Instance.Get(4049, 8), skill1rate = 25, skill2 = TSkillTable.Instance.Get(4166, 9), skill2rate = 25, skill3 = TSkillTable.Instance.Get(5579, 1), skill3rate = 50, skill3target = "master" });

            register(new smart_cubic() { id = 14, delay = 13, max_count = 50, power = 2106, skill1 = TSkillTable.Instance.Get(4049, 8), skill1rate = 30, skill2 = TSkillTable.Instance.Get(4052, 6), skill2rate = 20, skill3 = TSkillTable.Instance.Get(5579, 1), skill3rate = 50, skill3target = "master" });
        }

        public void register(CubicTemplate t)
        {
            cubics.Add(t.id * 65536 + t.level, t);
        }

        public CubicTemplate getCubic(int cubId, int skillLv)
        {
            int hash = cubId * 65536 + skillLv;
            if (cubics.ContainsKey(hash))
                return cubics[hash];
            else
                return null;
        }
    }
}