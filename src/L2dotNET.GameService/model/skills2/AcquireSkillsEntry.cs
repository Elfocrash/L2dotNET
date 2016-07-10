using System.Collections.Generic;

namespace L2dotNET.GameService.Model.Skills2
{
    public class AcquireSkillsEntry
    {
        public string Include = "",
                      Type;
        public List<AcquireSkill> Skills = new List<AcquireSkill>();
    }
}