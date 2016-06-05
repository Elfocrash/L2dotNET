namespace L2dotNET.GameService.managers
{
    public class PointCmdManager
    {
        private static readonly PointCmdManager m = new PointCmdManager();

        public static PointCmdManager getInstance()
        {
            return m;
        }

        public bool pointed(L2Player player, string _text)
        {
            _text = _text.Substring(1);

            switch (_text)
            {
                case "traffic":
                    player.sendMessage("Down: " + player.Gameclient.TrafficDown / 1024 + " kb");
                    player.sendMessage("Up: " + player.Gameclient.TrafficUp / 1024 + " kb");
                    break;

                default:
                    player.sendMessage("accepted point cmd " + _text);
                    break;
            }

            return true;
        }
    }
}