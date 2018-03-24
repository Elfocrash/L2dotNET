using L2dotNET.Attributes;
using L2dotNET.Models.player;

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