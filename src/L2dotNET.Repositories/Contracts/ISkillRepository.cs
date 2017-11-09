using System.Collections.Generic;
using L2dotNET.DataContracts;

namespace L2dotNET.Repositories.Contracts
{
    public interface ISkillRepository
    {
        List<SkillResponseContract> GetPlayerSkills(int objID);
    }
}
