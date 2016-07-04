using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Managers
{
    public class PointCmdManager
    {
        private static readonly PointCmdManager m = new PointCmdManager();

        public static PointCmdManager getInstance()
        {
            return m;
        }

        public bool pointed(L2Player player, string text)
        {
            text = text.Substring(1);

            switch (text)
            {
                case "traffic":
                    player.SendMessage("Down: " + player.Gameclient.TrafficDown / 1024 + " kb");
                    player.SendMessage("Up: " + player.Gameclient.TrafficUp / 1024 + " kb");
                    break;

                default:
                    player.SendMessage("accepted point cmd " + text);
                    break;
            }

            return true;
        }
    }
}