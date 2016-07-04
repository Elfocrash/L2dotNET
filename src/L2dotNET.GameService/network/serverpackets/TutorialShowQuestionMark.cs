namespace L2dotNET.GameService.Network.Serverpackets
{
    class TutorialShowQuestionMark : GameServerNetworkPacket
    {
        private readonly int _questionId;

        public TutorialShowQuestionMark(int id)
        {
            _questionId = id;
        }

        protected internal override void Write()
        {
            WriteC(0xa1);
            WriteD(_questionId);
        }
    }
}