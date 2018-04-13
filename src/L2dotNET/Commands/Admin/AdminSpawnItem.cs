using System;
using log4net;
using L2dotNET.Attributes;
using L2dotNET.Models.Player;

namespace L2dotNET.Commands.Admin
{
    [Command(CommandName = "createitem")]
    class AdminSpawnItem : AAdminCommand
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(AAdminCommand));

        protected internal override void Use(L2Player admin, string alias)
        {

            int id = int.Parse(alias.Split(' ')[1]);
            int count = 1;

            try
            {
                count = int.Parse(alias.Split(' ')[2]);
            }
            catch (Exception e)
            {
                _log.Error($"AdminSpawnItem: {e.Message}");
            }

            admin.AddItem(id, count);
        }
    }
}