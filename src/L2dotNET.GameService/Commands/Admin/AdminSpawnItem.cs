using System;
using log4net;
using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Commands.Admin
{
    class AdminSpawnItem : AAdminCommand
    {
        private readonly ILog log = LogManager.GetLogger(typeof(AAdminCommand));

        public AdminSpawnItem()
        {
            Cmd = "summon";
        }

        protected internal override void Use(L2Player admin, string alias)
        {
            //summon [id | name] [количество] -- призывает предмет\нпц [id] . Если количество не задано , призывает 1

            int id = int.Parse(alias.Split(' ')[1]);
            int count = 1;

            try
            {
                count = int.Parse(alias.Split(' ')[2]);
            }
            catch (Exception e)
            {
                log.Error($"AdminSpawnItem: {e.Message}");
            }

            admin.Inventory.addItem(id, count, true, true);
        }
    }
}