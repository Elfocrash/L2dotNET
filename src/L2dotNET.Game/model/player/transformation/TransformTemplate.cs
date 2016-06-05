using System.Collections.Generic;
using L2dotNET.GameService.model.skills2;
using L2dotNET.GameService.tables;

namespace L2dotNET.GameService.model.player.transformation
{
    public class TransformTemplate
    {
        public int id,
                   npcId;
        public double[] collision_box,
                        collision_box_f;
        public int[] moving_speed,
                     skills,
                     action;
        public List<int[]> _skills;
        public bool _onCursedWeapon = false;
        public byte MoveMode = 0; //1- ride, 2-fly
        public int TransformDispelId = 619;

        public int base_attack_range;
        public int base_random_damage;
        public int base_attack_speed;
        public int base_critical_prob;
        public int base_physical_attack;
        public int base_magical_attack;

        public int[] base_defend,
                     base_magic_defend,
                     basic_stat;

        public virtual void onTransformStart(L2Player player)
        {
            player.TransformID = id;
            player.MountType = MoveMode;
            player.MountedTemplate = NpcTable.Instance.GetNpcTemplate(npcId);
            player.broadcastUserInfo();

            if (_skills != null && _skills.Count > 0)
            {
                foreach (int[] s in _skills)
                {
                    TSkill sk = TSkillTable.Instance.Get(s[0], s[1]);
                    if (sk != null)
                        player.addSkill(sk, false, false);
                }

                player.updateSkillList();
            }
        }

        public virtual void onTransformEnd(L2Player player)
        {
            if (MoveMode > 0)
                player.MountType = 0;
            player.MountedTemplate = null;
            player.TransformID = 0;
            player.broadcastUserInfo();

            if (_skills != null && _skills.Count > 0)
            {
                foreach (int[] s in _skills)
                {
                    player.removeSkill(s[0], false, false);
                }

                player.updateSkillList();
            }
        }

        public virtual bool startFailed(L2Player player)
        {
            return false;
        }

        public double getRadius(byte Sex)
        {
            switch (Sex)
            {
                case 0:
                    return 0; //coll_r_male;
                default:
                    return 0; //coll_r_female == 0 ? coll_r_male : coll_r_female;
            }
        }

        public double getHeight(byte Sex)
        {
            switch (Sex)
            {
                case 0:
                    return 0; //coll_h_male;
                default:
                    return 0; //coll_h_female == 0 ? coll_h_male : coll_h_female;
            }
        }
    }
}