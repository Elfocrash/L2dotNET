using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2dotNET.GameService.Commands.Admin;
using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Handlers
{
    public interface IAdminCommandHandler
    {
        void Request(L2Player admin, string alias);

        void Register(AAdminCommand processor);
    }
}
