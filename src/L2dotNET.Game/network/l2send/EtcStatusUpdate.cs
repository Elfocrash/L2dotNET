using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using L2dotNET.Game.tables;

namespace L2dotNET.Game.network.l2send
{
    class EtcStatusUpdate : GameServerNetworkPacket
    {
        private int _force;
        private int _weight;
        private int _whisper;
        private int _danger;
        private int _grade;
        private int _death;
        private int _souls;
        public EtcStatusUpdate(L2Player player)
        {
            _force = player.getForceIncreased();
            _weight = player._penaltyWeight;
            _whisper = player._whisperBlock ? 1 : 0;
            _danger = player.isInDanger ? 1 : 0;
            _grade = player._penalty_grade;
            _death = player.death_penalty_level;
            _souls = player.Souls;
        }

        protected internal override void write()
        {
            writeC(0xF3);
            writeD(_force);
            writeD(_weight);
            writeD(_whisper);
            writeD(_danger); // 1 = danger area
            writeD(_grade);
            writeD(0); // 1 = charm of courage (no xp loss in siege..)
            //writeD(_death);
            //writeD(_souls);
        }
    }
}
