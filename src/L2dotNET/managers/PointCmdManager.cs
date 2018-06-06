using L2dotNET.Models.Player;

namespace L2dotNET.Managers
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
                    player.SendMessageAsync($"Down: {player.Gameclient.TrafficDown / 1024} kb");
                    player.SendMessageAsync($"Up: {player.Gameclient.TrafficUp / 1024} kb");
                    break;

                default:
                    player.SendMessageAsync($"accepted point cmd {text}");
                    break;
            }

            return true;
        }
    }
}