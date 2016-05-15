
namespace L2dotNET.GameService.managers.bbs
{
    public class BBSManager
    {
        private static BBSManager instance = new BBSManager();
        public static BBSManager getInstance()
        {
            return instance;
        }

        public void RequestShow(L2Player player, int type)
        {
            player.ShowHtmBBS("<html><body><br><br><center>Welcome to the community board</center><br><br></body></html>");
        }
    }
}
