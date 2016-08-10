using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using log4net;

namespace L2dotNET.Plugins
{
    public class PluginManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(GameServer));

        private readonly List<object> _plugins = new List<object>();

        public List<object> Plugins => _plugins;

        private string _currentPath = null;

        private static volatile PluginManager instance;
        private static readonly object syncRoot = new object();

        public static PluginManager Instance
        {
            get
            {
                if (instance != null)
                    return instance;

                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new PluginManager();
                }

                return instance;
            }
        }

        public void Initialize(GameServer server)
        {
            LoadPlugins();
            ExecuteStartup(server);
            EnablePlugins(server);
        }

        internal void LoadPlugins()
        {
            // Default it is the directory we are executing, and below.
            string pluginDirectoryPaths = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            pluginDirectoryPaths = @"./Plugins/";//Config.GetProperty("PluginDirectory", pluginDirectoryPaths);

            foreach (string dirPath in pluginDirectoryPaths.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (dirPath == null) continue;

                string pluginDirectory = Path.GetFullPath(dirPath);

                _currentPath = pluginDirectory;

                AppDomain currentDomain = AppDomain.CurrentDomain;
                currentDomain.AssemblyResolve += MyResolveEventHandler;

                List<string> pluginPaths = new List<string>();
                pluginPaths.AddRange(Directory.GetFiles(pluginDirectory, "*.dll", SearchOption.AllDirectories));
                pluginPaths.AddRange(Directory.GetFiles(pluginDirectory, "*.exe", SearchOption.AllDirectories));

                foreach (string pluginPath in pluginPaths)
                {
                    Assembly newAssembly = Assembly.LoadFile(pluginPath);

                    Type[] types = newAssembly.GetExportedTypes();
                    foreach (Type type in types)
                    {
                        try
                        {
                            // If no PluginAttribute and does not implement IPlugin interface, not a valid plugin
                            if (!type.IsDefined(typeof(PluginAttribute), true) && !typeof(IPlugin).IsAssignableFrom(type)) continue;
                            if (type.IsDefined(typeof(PluginAttribute), true))
                            {
                                PluginAttribute pluginAttribute = Attribute.GetCustomAttribute(type, typeof(PluginAttribute), true) as PluginAttribute;
                                if (pluginAttribute != null)
                                {
                                   // if (!Config.GetProperty(pluginAttribute.PluginName + ".Enabled", true)) continue;
                                }
                            }
                            var ctor = type.GetConstructor(Type.EmptyTypes);
                            if (ctor != null)
                            {
                                var plugin = ctor.Invoke(null);
                                _plugins.Add(plugin);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.WarnFormat("Failed loading plugin type {0} as a plugin.", type);
                            Log.Debug("Plugin loader caught exception, but is moving on.", ex);
                        }
                    }
                }
            }
        }

        private Assembly MyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            if (_currentPath == null) return null;

            try
            {
                AssemblyName name = new AssemblyName(args.Name);
                string assemblyPath = _currentPath + "\\" + name.Name + ".dll";
                return Assembly.LoadFile(assemblyPath);
            }
            catch (Exception)
            {
                try
                {
                    AssemblyName name = new AssemblyName(args.Name);
                    string assemblyPath = _currentPath + "\\" + name.Name + ".exe";
                    return Assembly.LoadFile(assemblyPath);
                }
                catch (Exception)
                {
                    return Assembly.LoadFile(args.Name + ".dll");
                }
            }
        }

        internal void ExecuteStartup(GameServer server)
        {
            foreach (object plugin in _plugins)
            {
                IStartup startupClass = plugin as IStartup;
                if (startupClass == null) continue;

                try
                {
                    startupClass.Configure(server);
                }
                catch (Exception ex)
                {
                    Log.Warn("Execute Startup class failed", ex);
                }
            }
        }

        internal void EnablePlugins(GameServer server)
        {
            foreach (object plugin in _plugins.ToArray())
            {
                IPlugin enablingPlugin = plugin as IPlugin;
                if (enablingPlugin == null) continue;

                try
                {
                    enablingPlugin.OnEnable(new PluginContext(server, this));
                }
                catch (Exception ex)
                {
                    Log.Warn("On enable plugin", ex);
                }
            }
        }

        internal void DisablePlugins()
        {
            foreach (object plugin in _plugins)
            {
                IPlugin enablingPlugin = plugin as IPlugin;
                if (enablingPlugin == null) continue;

                try
                {
                    enablingPlugin.OnDisable();
                }
                catch (Exception ex)
                {
                    Log.Warn("On disable plugin", ex);
                }
            }
        }
    }
}