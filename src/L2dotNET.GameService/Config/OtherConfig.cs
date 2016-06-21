namespace L2dotNET.GameService.Config
{
    public class OtherConfig
    {
        // --------------------------------------------------
        // Those "hidden" settings haven't configs to avoid admins to fuck their server
        // You still can experiment changing values here. But don't say I didn't warn you.
        // --------------------------------------------------

        /** Threads & Packets size */
        public const int THREAD_P_EFFECTS = 6; // default 6
        public const int THREAD_P_GENERAL = 15; // default 15
        public const int GENERAL_PACKET_THREAD_CORE_SIZE = 4; // default 4
        public const int IO_PACKET_THREAD_CORE_SIZE = 2; // default 2
        public const int GENERAL_THREAD_CORE_SIZE = 4; // default 4
        public const int AI_MAX_THREAD = 10; // default 10

        /** Reserve Host on LoginServerThread */
        public const bool RESERVE_HOST_ON_LOGIN = false; // default false

        /** MMO settings */
        public const int MMO_SELECTOR_SLEEP_TIME = 20; // default 20
        public const int MMO_MAX_SEND_PER_PASS = 12; // default 12
        public const int MMO_MAX_READ_PER_PASS = 12; // default 12
        public const int MMO_HELPER_BUFFER_COUNT = 20; // default 20

        /** Client Packets Queue settings */
        public const int CLIENT_PACKET_QUEUE_SIZE = 14; // default MMO_MAX_READ_PER_PASS + 2
        public const int CLIENT_PACKET_QUEUE_MAX_BURST_SIZE = 13; // default MMO_MAX_READ_PER_PASS + 1
        public const int CLIENT_PACKET_QUEUE_MAX_PACKETS_PER_SECOND = 80; // default 80
        public const int CLIENT_PACKET_QUEUE_MEASURE_INTERVAL = 5; // default 5
        public const int CLIENT_PACKET_QUEUE_MAX_AVERAGE_PACKETS_PER_SECOND = 40; // default 40
        public const int CLIENT_PACKET_QUEUE_MAX_FLOODS_PER_MIN = 2; // default 2
        public const int CLIENT_PACKET_QUEUE_MAX_OVERFLOWS_PER_MIN = 1; // default 1
        public const int CLIENT_PACKET_QUEUE_MAX_UNDERFLOWS_PER_MIN = 1; // default 1
        public const int CLIENT_PACKET_QUEUE_MAX_UNKNOWN_PER_MIN = 5; // default 5
    }
}
