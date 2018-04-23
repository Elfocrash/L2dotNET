using System;
using L2dotNET.Attributes;
using L2dotNET.Models.Player;

namespace L2dotNET.Commands.Admin
{
    [Command(CommandName = "setskill")]
    class AdminAddSkill : AAdminCommand
    {
        protected internal override void Use(L2Player admin, string alias)
        {
            
        }

        public AdminAddSkill(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}