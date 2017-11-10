using System.Collections.Generic;
using L2dotNET.Attributes;
using L2dotNET.model.player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Commands.Admin
{
    [Command(CommandName = "set_skill_all")]
    class AdminGiveAllSkills : AAdminCommand
    {
        protected internal override void Use(L2Player admin, string alias)
        {
            
        }
    }
}