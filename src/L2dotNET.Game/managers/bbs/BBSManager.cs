using L2dotNET.GameService.network.l2send;

namespace L2dotNET.GameService.managers.bbs
{
    public class BBSManager
    {
        private static volatile BBSManager instance;
        private static readonly object syncRoot = new object();

        public static BBSManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new BBSManager();
                        }
                    }
                }

                return instance;
            }
        }

        public BBSManager()
        {

        }

        public void RequestShow(L2Player player, int type)
        {
            if (Config.Instance.gameplayConfig.CommunityBoard.EnableCommunityBoard)
            {
                player.ShowHtmBBS("<html><body><br><br><center>Welcome to the community board</center><br><br></body></html>");
            }
            else
                player.sendPacket(new SystemMessage(SystemMessage.SystemMessageId.CB_OFFLINE));
        }
    }
}
