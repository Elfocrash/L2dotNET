using L2dotNET.Models;
using L2dotNET.Repositories.Contracts;
using L2dotNET.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.Services
{
    public class ServerService : IServerService
    {
        IUnitOfWork unitOfWork;

        public ServerService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public List<ServerModel> GetServerList()
        {
            return this.unitOfWork.ServerRepository.GetServerList();
        }

        public List<int> GetPlayersObjectIdList()
        {
            return this.unitOfWork.ServerRepository.GetPlayersObjectIdList();
        }

        public List<AnnouncementModel> GetAnnouncementsList()
        {
            return this.unitOfWork.ServerRepository.GetAnnouncementsList();
        }

        public bool CheckDatabaseQuery()
        {
            return this.unitOfWork.ServerRepository.CheckDatabaseQuery();
        }

    }
}
