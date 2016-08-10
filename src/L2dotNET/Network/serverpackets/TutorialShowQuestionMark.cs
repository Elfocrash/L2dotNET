namespace L2dotNET.Network.serverpackets
{
    class TutorialShowQuestionMark : GameserverPacket
    {
        private readonly int _questionId;

        public TutorialShowQuestionMark(int id)
        {
            _questionId = id;
        }

        public override void Write()
        {
            WriteByte(0xa1);
            WriteInt(_questionId);
        }
    }
}