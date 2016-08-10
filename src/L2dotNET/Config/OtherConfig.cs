namespace L2dotNET.Config
{
    public class OtherConfig
    {
        // --------------------------------------------------
        // Those "hidden" settings haven't configs to avoid admins to fuck their server
        // You still can experiment changing values here. But don't say I didn't warn you.
        // --------------------------------------------------

        /** Threads & Packets size */
        public const int ThreadPEffects = 6; // default 6
        public const int ThreadPGeneral = 15; // default 15
        public const int GeneralPacketThreadCoreSize = 4; // default 4
        public const int IoPacketThreadCoreSize = 2; // default 2
        public const int GeneralThreadCoreSize = 4; // default 4
        public const int AiMaxThread = 10; // default 10

        /** Reserve Host on LoginServerThread */
        public const bool ReserveHostOnLogin = false; // default false

        /** MMO settings */
        public const int MmoSelectorSleepTime = 20; // default 20
        public const int MmoMaxSendPerPass = 12; // default 12
        public const int MmoMaxReadPerPass = 12; // default 12
        public const int MmoHelperBufferCount = 20; // default 20

        /** Client Packets Queue settings */
        public const int ClientPacketQueueSize = 14; // default MMO_MAX_READ_PER_PASS + 2
        public const int ClientPacketQueueMaxBurstSize = 13; // default MMO_MAX_READ_PER_PASS + 1
        public const int ClientPacketQueueMaxPacketsPerSecond = 80; // default 80
        public const int ClientPacketQueueMeasureInterval = 5; // default 5
        public const int ClientPacketQueueMaxAveragePacketsPerSecond = 40; // default 40
        public const int ClientPacketQueueMaxFloodsPerMin = 2; // default 2
        public const int ClientPacketQueueMaxOverflowsPerMin = 1; // default 1
        public const int ClientPacketQueueMaxUnderflowsPerMin = 1; // default 1
        public const int ClientPacketQueueMaxUnknownPerMin = 5; // default 5
    }
}