using System.Collections.Generic;
using L2dotNET.GameService.Model.Skills2;

namespace L2dotNET.GameService.Model.Player.Transformation
{
    public class TransformTemplate
    {
        public int Id,
                   NpcId;
        public double[] CollisionBox,
                        CollisionBoxF;
        public int[] MovingSpeed;

        public int[] skills;

        public int[] Action;

        public List<int[]> Skills;
        public bool OnCursedWeapon = false;
        public byte MoveMode = 0; //1- ride, 2-fly
        public int TransformDispelId = 619;

        public int BaseAttackRange;
        public int BaseRandomDamage;
        public int BaseAttackSpeed;
        public int BaseCriticalProb;
        public int BasePhysicalAttack;
        public int BaseMagicalAttack;

        public int[] BaseDefend,
                     BaseMagicDefend,
                     BasicStat;

        public virtual void OnTransformStart(L2Player player)
        {
            player.TransformId = Id;
            player.MountType = MoveMode;
            //player.MountedTemplate = NpcTable.Instance.GetNpcTemplate(npcId);
            player.BroadcastUserInfo();

            if ((Skills != null) && (Skills.Count > 0))
            {
                foreach (int[] s in Skills)
                {
                    Skill sk = SkillTable.Instance.Get(s[0], s[1]);
                    if (sk != null)
                    {
                        player.AddSkill(sk, false, false);
                    }
                }

                player.UpdateSkillList();
            }
        }

        public virtual void OnTransformEnd(L2Player player)
        {
            if (MoveMode > 0)
            {
                player.MountType = 0;
            }
            player.MountedTemplate = null;
            player.TransformId = 0;
            player.BroadcastUserInfo();

            if ((Skills != null) && (Skills.Count > 0))
            {
                foreach (int[] s in Skills)
                {
                    player.RemoveSkill(s[0], false, false);
                }

                player.UpdateSkillList();
            }
        }

        public virtual bool StartFailed(L2Player player)
        {
            return false;
        }

        public double GetRadius(byte sex)
        {
            switch (sex)
            {
                case 0:
                    return 0; //coll_r_male;
                default:
                    return 0; //coll_r_female == 0 ? coll_r_male : coll_r_female;
            }
        }

        public double GetHeight(byte sex)
        {
            switch (sex)
            {
                case 0:
                    return 0; //coll_h_male;
                default:
                    return 0; //coll_h_female == 0 ? coll_h_male : coll_h_female;
            }
        }
    }
}