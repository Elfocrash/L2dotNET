
namespace L2dotNET.Game.network.l2send
{
    class TutorialShowQuestionMark : GameServerNetworkPacket
    {
        private int QuestionID;
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
