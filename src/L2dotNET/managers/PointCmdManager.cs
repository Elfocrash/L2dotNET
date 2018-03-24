using L2dotNET.Models.player;

namespace L2dotNET.managers
{
    public class PointCmdManager
    {
        private static readonly PointCmdManager M = new PointCmdManager();

        public static PointCmdManager GetInstance()
        {
            return M;
        }

        public bool Pointed(L2Player player, string text)
        {
            text = text.Substring(1);

            switch (text)
            {
                case "traffic":
                    player.SendMessage($"Down: {player.Gameclient.TrafficDown / 1024} kb");
                    player.SendMessage($"Up: {player.Gameclient.TrafficUp / 1024} kb");
                    break;

                default:
                    player.SendMessage($"accepted point cmd {text}");
                    break;
            }

            return true;
        }
    }
}