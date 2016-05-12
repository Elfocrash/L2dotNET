using System.Collections.Generic;

namespace L2dotNET.GameService.model.skills2
{
    public class TAcquireSkillsEntry
    {
        public string include = "", type;
        public List<TAcquireSkill> skills = new List<TAcquireSkill>();
    }
}
