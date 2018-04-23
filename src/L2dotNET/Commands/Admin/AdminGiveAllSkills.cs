using System;
using L2dotNET.Attributes;
using L2dotNET.Models.Player;

namespace L2dotNET.Commands.Admin
{
    [Command(CommandName = "set_skill_all")]
    public class AdminGiveAllSkills : AAdminCommand
    {
        protected internal override void Use(L2Player admin, string alias)
        {
            
        }

        public AdminGiveAllSkills(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}