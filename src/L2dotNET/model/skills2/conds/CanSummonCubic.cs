using L2dotNET.model.player;

namespace L2dotNET.model.skills2.conds
{
    public class CanSummonCubic : SkillCond
    {
        public override bool CanUse(L2Player player, Skill skill)
        {
            //int len = player.cubics.Count;
            //int max = (int)player.CharacterStat.getStat(TEffectType.p_cubic_mastery);
            //if (max == 0)
            //    max = 1;

            //return !(len + 1 > max);

            return true;
        }
    }
}