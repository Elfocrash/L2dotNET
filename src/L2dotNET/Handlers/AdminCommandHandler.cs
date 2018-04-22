using System;
using System.Collections.Generic;
using System.Reflection;
using log4net;
using L2dotNET.Attributes;
using L2dotNET.Commands;
using L2dotNET.Models.Player;
using L2dotNET.Utility;

namespace L2dotNET.Handlers
{
    public class AdminCommandHandler : IAdminCommandHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AdminCommandHandler));
        private readonly SortedList<string, AAdminCommand> _commands = new SortedList<string, AAdminCommand>();

        private static volatile AdminCommandHandler _instance;
        private static readonly object SyncRoot = new object();

        public static AdminCommandHandler Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new AdminCommandHandler();
                }

                return _instance;
            }
        }

        public void Initialize()
        {
            IEnumerable<Type> typelist = Utilz.GetTypesInNamespace(Assembly.GetExecutingAssembly(), "L2dotNET.Commands.Admin");
            foreach (Type t in typelist)
                Register(Activator.CreateInstance(t));

            Log.Info($"Loaded {_commands.Count} commands.");
        }

        public void Request(L2Player admin, string alias)
        {
            string cmd = alias;
            if (alias.Contains(" "))
                cmd = alias.Split(' ')[0];

            if (!_commands.ContainsKey(cmd))
            {
                admin.SendMessage($"Command {cmd} not exists.");
                admin.SendActionFailed();
                return;
            }

            AAdminCommand processor = _commands[cmd];
            try
            {
                processor.Use(admin, alias);
            }
            catch (Exception sss)
            {
                admin.SendMessage("Probably syntax eror.");
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