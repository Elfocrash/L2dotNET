
namespace L2dotNET.GameService.network.l2send
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
            _msgs = new int[] { msgId };
            _status = status;
        }

        protected internal override void write()
        {
		    writeC(0xfe);
		    writeH(0xb3);
		    writeD(_receivedBoard ? 1 : 0);
		    writeD(_msgs.Length);
		    foreach (int postId in _msgs)
		    {
			    writeD(postId);
                writeD(_status);
		    }
        }
    }
}
