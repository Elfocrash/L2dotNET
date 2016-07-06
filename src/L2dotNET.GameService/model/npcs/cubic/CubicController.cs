using System.Collections.Generic;
using L2dotNET.GameService.Model.Npcs.Cubic.Data;
using L2dotNET.GameService.Model.Skills2;

namespace L2dotNET.GameService.Model.Npcs.Cubic
{
    public class CubicController
    {
        private static readonly CubicController Instance = new CubicController();

        public static CubicController GetController()
        {
            return Instance;
        }

        public SortedList<int, CubicTemplate> Cubics = new SortedList<int, CubicTemplate>();

        public CubicController()
        {
            //heal_cubic
            byte lv = 1;
            for (byte a = 1; a <= 16; a++)
            {
                if (lv == 8)
                {
                    lv = 101;
                }

                Register(new HealCubic(3, lv, 4051, a));
                lv++;
            }
            //nub heal
            Register(new HealCubic(3, 20, 4051, 20, 3600));

            Register(new SmartCubic
                     {
                         Id = 10,
                         Delay = 10,
                         MaxCount = 50,
                         Power = 2106,
                         Skill1 = SkillTable.Instance.Get(4165, 9),
                         Skill1Rate = 25,
                         Skill2 = SkillTable.Instance.Get(4053, 8),
                         Skill2Rate = 25,
                         Skill3 = SkillTable.Instance.Get(5579, 1),
                         Skill3Rate = 50,
                         Skill3Target = "master"
                     });

            Register(new SmartCubic
                     {
                         Id = 11,
                         Delay = 10,
                         MaxCount = 50,
                         Power = 2106,
                         Skill1 = SkillTable.Instance.Get(5115, 4),
                         Skill1Rate = 40,
                         Skill2 = SkillTable.Instance.Get(4049, 8),
                         Skill2Rate = 15,
                         Skill3 = SkillTable.Instance.Get(5579, 1),
                         Skill3Rate = 45,
                         Skill3Target = "master"
                     });

            Register(new SmartCubic
                     {
                         Id = 12,
                         Delay = 13,
                         MaxCount = 50,
                         Power = 2106,
                         Skill1 = SkillTable.Instance.Get(4165, 9),
                         Skill1Rate = 20,
                         Skill2 = SkillTable.Instance.Get(4051, 1),
                         Skill2Rate = 30,
                         Skill2Target = "heal",
                         Skill3 = SkillTable.Instance.Get(5579, 1),
                         Skill3Rate = 50,
                         Skill3Target = "master"
                     });

            Register(new SmartCubic
                     {
                         Id = 13,
                         Delay = 13,
                         MaxCount = 50,
                         Power = 2106,
                         Skill1 = SkillTable.Instance.Get(4049, 8),
                         Skill1Rate = 25,
                         Skill2 = SkillTable.Instance.Get(4166, 9),
                         Skill2Rate = 25,
                         Skill3 = SkillTable.Instance.Get(5579, 1),
                         Skill3Rate = 50,
                         Skill3Target = "master"
                     });

            Register(new SmartCubic
                     {
                         Id = 14,
                         Delay = 13,
                         MaxCount = 50,
                         Power = 2106,
                         Skill1 = SkillTable.Instance.Get(4049, 8),
                         Skill1Rate = 30,
                         Skill2 = SkillTable.Instance.Get(4052, 6),
                         Skill2Rate = 20,
                         Skill3 = SkillTable.Instance.Get(5579, 1),
                         Skill3Rate = 50,
                         Skill3Target = "master"
                     });
        }

        public void Register(CubicTemplate t)
        {
            Cubics.Add((t.Id * 65536) + t.Level, t);
        }

        public CubicTemplate GetCubic(int cubId, int skillLv)
        {
            int hash = (cubId * 65536) + skillLv;
            return Cubics.ContainsKey(hash) ? Cubics[hash] : null;
        }
    }
}