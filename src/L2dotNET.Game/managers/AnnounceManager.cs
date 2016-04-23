using System.IO;
using L2dotNET.Game.model.player.basic;
using L2dotNET.Game.network.l2send;
using L2dotNET.Game.world;

namespace L2dotNET.Game.managers
{
    class AnnounceManager
    {
        private static AnnounceManager m = new AnnounceManager();

        public static AnnounceManager getInstance()
        {
            return m;
        }

        private string[] announcements;
        public AnnounceManager()
        {
            announcements = File.ReadAllLines(@"data\announcements.txt");
        }

        public void announce(string text)
        {
            CreatureSay cs = new CreatureSay(SayIDList.CHAT_ANNOUNCE, text);
            foreach (L2Player p in L2World.Instance.GetAllPlayers())
            {
                p.sendPacket(cs);
            }
        }

        public void criticalAnnounce(string text)
        {
            CreatureSay cs = new CreatureSay(SayIDList.CHAT_CRITICAL_ANNOUNCE, text);
            foreach (L2Player p in L2World.Instance.GetAllPlayers())
            {
                p.sendPacket(cs);
            }
        }

        public void screenAnnounce(string text)
        {
            CreatureSay cs = new CreatureSay(SayIDList.CHAT_SCREEN_ANNOUNCE, text);
            foreach (L2Player p in L2World.Instance.GetAllPlayers())
            {
                p.sendPacket(cs);
            }
        }

        public void onEnter(L2Player player)
        {
            if (announcements == null || announcements.Length == 0)
                return;

            CreatureSay cs = new CreatureSay(SayIDList.CHAT_ANNOUNCE);
            foreach (string t in announcements)
            {
                cs._text = t;
                player.sendPacket(cs);
            } 
        }
    }
}
