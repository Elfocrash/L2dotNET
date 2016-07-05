using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Handlers
{
    public interface IChatHandler
    {
        void HandleChat(int type, L2Player player, string target, string text);
    }
}
