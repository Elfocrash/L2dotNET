using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class EtcStatusUpdate : GameServerNetworkPacket
    {
        private readonly int _force;
        private readonly int _weight;
        private readonly int _whisper;
        private readonly int _danger;
        private readonly int _grade;
        private int _death;
        private int _souls;

        public EtcStatusUpdate(L2Player player)
        {
            _force = player.GetForceIncreased();
            _weight = player.PenaltyWeight;
            _whisper = player.WhisperBlock ? 1 : 0;
            _danger = player.IsInDanger ? 1 : 0;
            _grade = player.PenaltyGrade;
            _death = player.DeathPenaltyLevel;
            _souls = player.Souls;
        }

        protected internal override void Write()
        {
            WriteC(0xF3);
            WriteD(_force);
            WriteD(_weight);
            WriteD(_whisper);
            WriteD(_danger); // 1 = danger area
            WriteD(_grade);
            WriteD(0); // 1 = charm of courage (no xp loss in siege..)
            //writeD(_death);
            //writeD(_souls);
        }
    }
}