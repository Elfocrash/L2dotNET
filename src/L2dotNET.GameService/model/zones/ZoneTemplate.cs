using System.Collections.Generic;
using System.Linq;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.GameService.Model.Zones.Forms;

namespace L2dotNET.GameService.Model.Zones
{
    public class ZoneTemplate
    {
        public string _map_no;
        public ZoneType Type;
        public ZoneForm Territory;
        public ZoneTarget _target = ZoneTarget.all;
        public string _affect_race = "all";
        public int _entering_message_no;
        public int _leaving_message_no;
        public int _move_bonus = 0;
        public bool DefaultStatus = true;
        public int _event_id;
        public int _damage_on_hp = 0;
        public int _damage_on_mp = 0;
        public int _message_no;

        public int _skill_prob;
        public int _unit_tick = 9;
        public int _initial_delay = 1;
        public List<TSkill> _skills;
        public TSkill _skill;
        public string Name;
        public int _hp_regen_bonus;
        public int _mp_regen_bonus;
        public int[] _x;
        public int[] _y;
        public int _z1,
                   _z2;
        public int _exp_penalty_per;
        public bool _item_drop;

        public enum ZoneTarget
        {
            npc,
            pc,
            all,
            only_pc
        }

        public enum ZoneType
        {
            mother_tree,
            peace_zone,
            battle_zone,
            poison,
            water,
            no_restart,
            ssq_zone,
            swamp,
            damage,
            instant_skill,
            instant_buff,

            hideout,
            monster_race
        }

        public void setSkillList(string val)
        {
            if (_skills == null)
                _skills = new List<TSkill>();

            //string d1 = val.Substring(1).Replace("}", "").Replace("@", "");

            //foreach (string sk in d1.Split(';'))
            //{
            //    int id = int.Parse(sk);

            //    TSkill skill = TSkillTable.getInstance().get(sk);
            //    if (skill != null)
            //        _skills.Add(skill);
            //    else
            //        CLogger.error("areatable: null skill " + sk + " for zone " + Name);
            //}
        }

        public void setSkill(string p)
        {
            //_skill = TSkillTable.getInstance().get(p);
            //if (_skill == null)
            //    CLogger.error("areatable: null skill " + p + " for default swamps");
        }

        public void setRange(string val)
        {
            string d1 = val.Substring(2).Replace("};{", "\f").Replace("}}", "");
            int s = d1.Split('\f').Length;
            _x = new int[s];
            _y = new int[s];
            int y = 0;
            foreach (string[] xyz in d1.Split('\f').Select(loc => loc.Split(';')))
            {
                _x[y] = int.Parse(xyz[0]);
                _y[y] = int.Parse(xyz[1]);
                _z1 = int.Parse(xyz[2]);
                _z2 = int.Parse(xyz[3]);
                y++;
            }
        }

        public void setRange(List<int[]> zoneLoc)
        {
            _x = new int[zoneLoc.Count];
            _y = new int[zoneLoc.Count];
            int y = 0;

            foreach (int[] l in zoneLoc)
            {
                _x[y] = l[0];
                _y[y] = l[1];
                _z1 = l[2];
                _z2 = l[3];
                y++;
            }
        }
    }
}