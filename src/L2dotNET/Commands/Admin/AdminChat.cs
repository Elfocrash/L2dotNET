using System;
using L2dotNET.Attributes;
using L2dotNET.Models.Player;

namespace L2dotNET.Commands.Admin
{
    [Command(CommandName = "chat")]
    class AdminChat : AAdminCommand
    {
        protected internal override void Use(L2Player admin, string alias)
        {
            
        }

        public AdminChat(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}