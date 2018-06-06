using System;
using System.Threading.Tasks;
using L2dotNET.Attributes;
using L2dotNET.Logging.Abstraction;
using L2dotNET.Models.Player;

namespace L2dotNET.Commands.Admin
{
    [Command(CommandName = "createitem")]
    class AdminSpawnItem : AAdminCommand
    {
        private readonly ILog _log = LogProvider.GetCurrentClassLogger();

        protected internal override async Task UseAsync(L2Player admin, string alias)
        {
            await Task.Run(() =>
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
            });
        }

        public AdminSpawnItem(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}