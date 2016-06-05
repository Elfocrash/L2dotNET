using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Model.Skills2
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