using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Managers.BBS
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
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new BBSManager();
                    }

                return instance;
            }
        }

        public BBSManager() { }

        public void RequestShow(L2Player player, int type)
        {
            if (Config.Config.Instance.GameplayConfig.Server.CommunityBoard.EnableCommunityBoard)
                player.ShowHtmBBS("<html><body><br><br><center>Welcome to the community board</center><br><br></body></html>");
            else
                player.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.CB_OFFLINE));
        }
    }
}