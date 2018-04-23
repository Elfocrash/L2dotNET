using System.Collections.Generic;
using L2dotNET.DataContracts;
using L2dotNET.Repositories.Contracts;
using L2dotNET.Services.Contracts;

namespace L2dotNET.Services
{
    public class SkillService : ISkillService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SkillService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<SkillResponseContract> GetPlayerSkills(int charID)
        {
            return _unitOfWork.SkillRepository.GetPlayerSkills(charID);
        }
    }
}
