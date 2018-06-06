using System;
using L2dotNET.Attributes;
using L2dotNET.Models.Items;
using L2dotNET.Models.Player;
using L2dotNET.Tables;
using Microsoft.Extensions.DependencyInjection;

namespace L2dotNET.Commands.Admin
{
    [Command(CommandName = "summon3")]
    class AdminSpawnItemRange : AAdminCommand
    {
        private readonly ItemTable _itemTable;

        public AdminSpawnItemRange(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _itemTable = serviceProvider.GetService<ItemTable>();
        }

        protected internal override void Use(L2Player admin, string alias)
        {
            int idmin = int.Parse(alias.Split(' ')[1]);
            int idmax = int.Parse(alias.Split(' ')[2]);

            if ((idmax - idmin) > 200)
            {
                admin.SendMessageAsync("Too big id range.");
                return;
            }

            bool x = false;
            for (int i = idmin; i <= idmax; i++)
            {
                ItemTemplate item = _itemTable.GetItem(i);

                if (item == null)
                    admin.SendMessageAsync($"Item with id {i} not exists.");
                else
                {
                    admin.AddItem(i, 1);
                    x = true;
                }
            }

            if (x)
                admin.SendItemList(true);
        }
    }
}