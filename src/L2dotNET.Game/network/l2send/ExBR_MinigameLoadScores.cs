using System.Collections.Generic;
using L2dotNET.Game.managers;

namespace L2dotNET.Game.network.l2send
{
    class ExBR_MinigameLoadScores : GameServerNetworkPacket
    {
        private List<MinigameMember> _members;
        private int MyRanking;
        private int MyScore;
        private int LastScore;

        public ExBR_MinigameLoadScores(List<MinigameMember> members, int MyRanking, int MyScore, int LastScore)
        {
            _members = members;
            this.MyRanking = MyRanking;
            this.MyScore = MyScore;
            this.LastScore = LastScore;
        }

        protected internal override void write()
        {
            writeC(0xFE);
            writeH(0xD2);

            writeD(MyRanking);
            writeD(MyScore);
            writeD(LastScore);

            writeD(_members.Count);

            byte pos = 1;
            foreach (MinigameMember m in _members)
            {
                writeD(pos); pos++;
                writeS(m.Name);
                writeD(m.Score);
            }
        }
    }
}
