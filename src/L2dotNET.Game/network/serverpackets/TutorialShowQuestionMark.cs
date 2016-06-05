
namespace L2dotNET.GameService.network.l2send
{
    class TutorialShowQuestionMark : GameServerNetworkPacket
    {
        private readonly int QuestionID;
        public TutorialShowQuestionMark(int id)
        {
            QuestionID = id;
        }

        protected internal override void write()
        {
            writeC(0xa1);
            writeD(QuestionID);
        }
    }
}
