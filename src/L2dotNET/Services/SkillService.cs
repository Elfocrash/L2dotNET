using System.Collections.Generic;
using L2dotNET.DataContracts;
using L2dotNET.Repositories.Contracts;
using L2dotNET.Services.Contracts;

namespace L2dotNET.Services
{
    public class SkillService : ISkillService
    {
        private readonly ISkillRepository _skillRepository;

        public SkillService(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }

        public List<SkillResponseContract> GetPlayerSkills(int charId)
        {
            return _skillRepository.GetPlayerSkills(charId);
        }
    }
}
