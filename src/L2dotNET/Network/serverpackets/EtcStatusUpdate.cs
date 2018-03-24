using L2dotNET.Models.player;

namespace L2dotNET.Network.serverpackets
{
    class EtcStatusUpdate : GameserverPacket
    {
        private readonly int _force;
        private readonly int _weight;
        private readonly int _whisper;
        private readonly int _danger;
        private readonly int _grade;
        private int _death = 0;
        private int _souls;

        public EtcStatusUpdate(L2Player player)
        {
            _force = player.GetForceIncreased();
            _weight = player.PenaltyWeight;
            _whisper = player.WhisperBlock ? 1 : 0;
            _danger = player.IsInDanger ? 1 : 0;
            _grade = player.PenaltyGrade;
            //_death = player.DeathPenaltyLevel;
        }

        public override void Write()
        {
            WriteByte(0xF3);
            WriteInt(_force);
            WriteInt(_weight);
            WriteInt(_whisper);
            WriteInt(_danger); // 1 = danger area
            WriteInt(_grade);
            WriteInt(0); // 1 = charm of courage (no xp loss in siege..)
            //writeD(_death);
            //writeD(_souls);
        }
    }
}