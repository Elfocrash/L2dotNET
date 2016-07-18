namespace L2dotNET.GameService.Network.Serverpackets
{
    class TutorialShowQuestionMark : GameserverPacket
    {
        private readonly int _questionId;

        public TutorialShowQuestionMark(int id)
        {
            _questionId = id;
        }

        protected internal override void Write()
        {
            WriteByte(0xa1);
            WriteInt(_questionId);
        }
    }
}