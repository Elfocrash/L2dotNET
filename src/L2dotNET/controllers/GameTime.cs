using System;
using System.Threading.Tasks;
using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Models.Player;
using L2dotNET.Network;
using L2dotNET.Network.serverpackets;
using L2dotNET.World;
using NLog;

namespace L2dotNET.Controllers
{
    public class GameTime
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public static bool Night { get; private set; }

        public static int IngameTime => (int)localTime.TotalSeconds / 6;

        private static readonly GameserverPacket _dayPk = new SunRise();
        private static readonly GameserverPacket _nightPk = new SunSet();

        private static TimeSpan localTime;

        // TODO: move to config
        private static readonly int TimeSpeed = 1;
        private static readonly int DayStart = 6;
        private static readonly int DayEnd = 22;
        

        public static void Initialize()
        {
            UpdateIngameTime();

            Task.Factory.StartNew(UpdateTime);

            Log.Info($"[GameTime] Time is set to {localTime.Hours}:{localTime.Minutes}");
        }

        private static async void UpdateTime()
        {
            while (true)
            {
                if (UpdateIngameTime())
                {
                    UpdateTimeForAll();
                }

                await Task.Delay(60 * 1000 / TimeSpeed);
            }
        }

        private static bool UpdateIngameTime()
        {
            int dayLength = 60 * 24 / TimeSpeed;
            int localTimeInMinutes = (int)DateTime.Now.TimeOfDay.TotalMinutes % dayLength * TimeSpeed;

            localTime = TimeSpan.FromMinutes(localTimeInMinutes);

            bool night = localTimeInMinutes < DayStart && localTimeInMinutes >= DayEnd;

            if (night != Night)
            {
                Night = night;
                return true;
            }

            return false;
        }

        private static void UpdateTimeForAll()
        {
            L2World.GetPlayers().ForEach(UpdateTimeForPlayer);
        }

        public static void UpdateTimeForPlayer(L2Player p)
        {
            p.SendPacketAsync(Night ? _nightPk : _dayPk);
        }

        public static async Task ShowInfoAsync(L2Player player)
        {
            DateTime dt = new DateTime(2000, 1, 1, 0, 0, 0).AddSeconds(IngameTime * 6);

            SystemMessage sm = new SystemMessage(Night
                ? SystemMessageId.TimeS1S2InTheNight
                : SystemMessageId.TimeS1S2InTheDay);
            sm.AddString(dt.ToString("hh"));
            sm.AddString(dt.ToString("mm:ss"));
            await player.SendPacketAsync(sm);
        }
    }
}