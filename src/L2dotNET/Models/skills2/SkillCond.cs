using L2dotNET.model.player;

namespace L2dotNET.model.skills2
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