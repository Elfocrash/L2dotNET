using System.Collections.Generic;
using L2dotNET.Models;

namespace L2dotNET.Repositories.Contracts
{
    public interface ISkillRepository
    {
        List<SkillResponseModel> GetPlayerSkills(int objID);
    }
}
