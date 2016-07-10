using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Model.Skills2
{
    public class SkillCond
    {
        public sbyte Retcode = -1;

        public virtual bool CanUse(L2Player player, Skill skill)
        {
            return true;
        }

        public virtual void Build(string str) { }
    }
}