using L2dotNET.GameService.Model.player;

namespace L2dotNET.GameService.Model.skills2
{
    public class TSkillCond
    {
        public sbyte retcode = -1;

        public virtual bool CanUse(L2Player player, TSkill skill)
        {
            return true;
        }

        public virtual void build(string str) { }
    }
}