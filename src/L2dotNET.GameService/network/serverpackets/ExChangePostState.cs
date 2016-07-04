namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExChangePostState : GameServerNetworkPacket
    {
        private readonly bool _receivedBoard;
        private readonly int[] _msgs;
        private readonly int _status;
        public static int Deleted = 0;
        public static int Readed = 1;
        public static int Rejected = 2;

        public ExChangePostState(bool p, int msgId, int status)
        {
            _receivedBoard = p;
            _msgs = new[] { msgId };
            _status = status;
        }

        protected internal override void Write()
        {
            WriteC(0xfe);
            WriteH(0xb3);
            WriteD(_receivedBoard ? 1 : 0);
            WriteD(_msgs.Length);
            foreach (int postId in _msgs)
            {
                WriteD(postId);
                WriteD(_status);
            }
        }
    }
}