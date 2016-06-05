using L2dotNET.GameService.Model.player;
using L2dotNET.GameService.network.serverpackets;

namespace L2dotNET.GameService.Managers.bbs
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

        public BBSManager() { }

        public void RequestShow(L2Player player, int type)
        {
            if (Config.Config.Instance.gameplayConfig.CommunityBoard.EnableCommunityBoard)
            {
                player.ShowHtmBBS("<html><body><br><br><center>Welcome to the community board</center><br><br></body></html>");
            }
            else
                player.sendPacket(new SystemMessage(SystemMessage.SystemMessageId.CB_OFFLINE));
        }
    }
}