using System.Collections.Generic;

namespace L2dotNET.model.skills2
{
    public class AcquireSkillsEntry
    {
        public string Include = string.Empty,
                      Type;
        public List<AcquireSkill> Skills = new List<AcquireSkill>();
    }
}