using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using L2dotNET.Attributes;
using L2dotNET.Commands;
using L2dotNET.Models.Player;
using L2dotNET.Utility;
using NLog;

namespace L2dotNET.Handlers
{
    public class AdminCommandHandler : IAdminCommandHandler
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly SortedList<string, AAdminCommand> _commands = new SortedList<string, AAdminCommand>();
        public bool Initialised { get; private set; }
        private readonly IServiceProvider _serviceProvider;

        public AdminCommandHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Initialise()
        {
            if (Initialised)
            {
                return;
            }

            IEnumerable<Type> typelist = Utilz.GetTypesInNamespace(Assembly.GetExecutingAssembly(), "L2dotNET.Commands.Admin");
            foreach (Type t in typelist)
                Register(Activator.CreateInstance(t, _serviceProvider));

            Log.Info($"Loaded {_commands.Count} commands.");
            Initialised = true;
        }

        public void Request(L2Player admin, string alias)
        {
            string cmd = alias;
            if (alias.Contains(" "))
                cmd = alias.Split(' ')[0];

            if (!_commands.ContainsKey(cmd))
            {
                admin.SendMessageAsync($"Command {cmd} not exists.");
                admin.SendActionFailedAsync();
                return;
            }

            AAdminCommand processor = _commands[cmd];
            try
            {
                processor.UseAsync(admin, alias);
            }
            catch (Exception sss)
            {
                admin.SendMessageAsync("Probably syntax eror.");
                Log.Error(sss);
            }
        }

        public void Register(object processor)
        {
            CommandAttribute attribute =
                (CommandAttribute) processor.GetType().GetCustomAttribute(typeof(CommandAttribute));
            _commands.Add(attribute.CommandName,(AAdminCommand) processor);
        }
    }
}