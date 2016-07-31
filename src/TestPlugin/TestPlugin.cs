using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Plugins;

namespace TestPlugin
{
    [Plugin(PluginName = "TestPlugin", Description = "A Test plugin", PluginVersion = "1.0", Author = "Elfocrash")]
    public class TestPlugin : Plugin
    {
        protected override void OnEnable()
        {
            Console.Out.WriteLine("Worked???");
        }

        public override void OnLogin(L2Player player)
        {
            player.SendMessage("Worked?");
        }
    }
}
