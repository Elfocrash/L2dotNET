using System.Collections.Generic;
using log4net;
using L2dotNET.GameService.model.player.basic;
using L2dotNET.GameService.network.l2send;
using L2dotNET.GameService.world;
using L2dotNET.Models;
using L2dotNET.Services.Contracts;
using Ninject;

namespace L2dotNET.GameService.Managers
{
    class AnnouncementManager
    {
        [Inject]
        public IServerService serverService
        {
            get { return GameServer.Kernel.Get<IServerService>(); }
        }

        private static readonly ILog Log = LogManager.GetLogger(typeof(AnnouncementManager));

        private static volatile AnnouncementManager instance;
        private static readonly object syncRoot = new object();

        public List<AnnouncementModel> Announcements { get; set; }

        public static AnnouncementManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new AnnouncementManager();
                        }
                    }
                }

                return instance;
            }
        }

        public void Initialize()
        {
            Announcements = serverService.GetAnnouncementsList();
            Log.Info($"Announcement manager: Loaded {Announcements.Count} annoucements.");
        }

        public AnnouncementManager() { }

        public void Announce(string text)
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

        public void OnEnter(L2Player player)
        {
            if (Announcements == null || Announcements.Count == 0)
                return;

            CreatureSay cs = new CreatureSay(SayIDList.CHAT_ANNOUNCE);
            foreach (AnnouncementModel announcement in Announcements)
            {
                if (announcement.Type == 0)
                {
                    cs.Text = announcement.Text;
                    player.sendPacket(cs);
                }
            }
        }
    }
}