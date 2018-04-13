using System.Collections.Generic;
using L2dotNET.DataContracts;

namespace L2dotNET.Services.Contracts
{
    public interface ISkillService
    {
        List<SkillResponseContract> GetPlayerSkills(int charID);
    }
}
